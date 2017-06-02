using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace JGitEventViewer
{
    /**
     * https://github.com/Praeses/techinterview-github
     * 
     * Class to parse GitHub Events Stream and return GitHubEvent objects.
     * Reference: https://developer.github.com/v3/activity/events/
     */
    class GitHubEventStream
    {
        private String url = null;
        
        public GitHubEventStream(String url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new ArgumentException("URL not well formed");
            }
            this.url = url;
            HttpWebRequest request = WebRequest.CreateHttp(url);
        }


    }
}
