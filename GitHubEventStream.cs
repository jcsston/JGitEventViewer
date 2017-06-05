using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Windows.UI.Xaml.Controls;
using Windows.Data.Json;

namespace JGitEventViewer
{
    /**
     * https://github.com/Praeses/techinterview-github
     * 
     * Class to parse GitHub Events Stream and return GitHubEvent objects.
     * Reference: https://developer.github.com/v3/activity/events/
     */
    public class GitHubEventStream
    {
        private Uri url = null;
        private int requestInterval = 30; // 30 seconds by default
        private object lastEventsLock = new object();
        private GitHubEvents lastEvents = null;
        private DateTime lastEventFetchTime = DateTime.Now;

        public GitHubEventStream(String url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new ArgumentException("URL not well formed");
            }
            this.url = new Uri(url);
        }

        private async Task<GitHubEvents> FetchJSONEvents()
        {
            // First check if we already have previously fetched Events object and it's within the GitHub Poll Internal
            lock (lastEventsLock)
            {
                if (lastEvents != null && (DateTime.Now - lastEventFetchTime).TotalSeconds < requestInterval)
                {
                    JEventSource.Log.Info("Returning cached GitHub Events");
                    return lastEvents;
                }
            }

            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            //Add a user-agent header to the GET request. 
            var headers = httpClient.DefaultRequestHeaders;
            headers.UserAgent.ParseAdd("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

            //Send the GET request asynchronously and retrieve the response as a string.
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(url);
                httpResponse.EnsureSuccessStatusCode();

                String xPollInterval = httpResponse.Headers["X-Poll-Interval"];
                if (xPollInterval != null && int.TryParse(xPollInterval, out requestInterval))
                {
                    JEventSource.Log.Info(String.Format("Found polling interval of {0}", requestInterval));
                }
                else
                {
                    JEventSource.Log.Warn(String.Format("No polling interval found, using default {0} seconds", requestInterval));
                }

                /*
                String contentType = httpResponse.Headers["Content-Type"];
                if (contentType != null && contentType.Contains("application/json"))
                {
                    JEventSource.Log.Info("Response claims to be in expected JSON format");
                }
                */

                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                JsonArray jsonData = JsonArray.Parse(httpResponseBody);
                GitHubEvents parsedData = GitHubEvents.Build(jsonData);

                lock (lastEventsLock)
                {
                    lastEvents = parsedData;
                    lastEventFetchTime = DateTime.Now;
                }
                return lastEvents;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to fetch JSON", ex);   
            }
        }

        /**
         * Start refresh
         */
        public async Task<GitHubEvents> RefreshAsync()
        {

            GitHubEvents result = await FetchJSONEvents();
            // TODO: More logic here?
            return result;
        }

        /**
         * Stop any background refreshes 
         */
        public void Suspend()
        {
            
        }
    }
}
