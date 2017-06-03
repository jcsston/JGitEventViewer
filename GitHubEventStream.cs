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

        public GitHubEventStream(String url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new ArgumentException("URL not well formed");
            }
            this.url = new Uri(url);
        }

        /**
         * Start refresh
         */
        public async Task<String> RefreshAsync()
        {
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

                JsonArray result = JsonArray.Parse(httpResponseBody);
                return result.GetObjectAt(0).ToString();

            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
            
            return httpResponseBody;
            /*
            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.CreateHttp(this.url);

            WebResponse response = request.GetResponse();
            String xPollInterval = response.Headers.Get("X-Poll-Interval");
            if (xPollInterval != null && int.TryParse(xPollInterval, out requestInterval))
            {
                Trace.TraceInformation(String.Format("Found polling interval of {0}", requestInterval));
            }
            else
            {
                Trace.TraceWarning(String.Format("No polling interval found, using default {0} seconds", requestInterval));
            }


            foreach (string headerName in response.Headers.AllKeys)
            {
                Trace.TraceInformation(String.Format("Header: {0} Value: {1}", new object[] { headerName, response.Headers[headerName] }));
            }

            Trace.TraceInformation(String.Format("Content Length is {0}", response.ContentLength));

            using (TextReader r = new StreamReader(response.GetResponseStream()))
            {
                String jsonBlob = r.ReadToEnd();
                Trace.TraceInformation(String.Format("JSON Blob: {0}", jsonBlob));

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GitHubEvents));

                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonBlob));
                GitHubEvents result = (GitHubEvents)serializer.ReadObject(ms);

                if (result.Events != null)
                {
                    foreach (GitHubEvent gitEvent in result.Events)
                    {
                        Trace.TraceInformation(String.Format("Git Event Type: {0} Date: {1} User: {2}", new object[] { gitEvent.type, gitEvent.created_at, gitEvent.actor.login }));
                    }
                }
            }
            */
        }

        /**
         * Stop any background refreshes 
         */
        public void Suspend()
        {
            
        }
    }
}
