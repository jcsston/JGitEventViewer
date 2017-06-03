using System;

namespace JGitEventViewer
{
    // Generated Classes for GitHub Event JSON (Paste Special -> Paste as JSON)
    // Example JSON Below
    /*
    [
      {
        "type": "Event",
        "public": true,
        "payload": {
        },
        "repo": {
          "id": 3,
          "name": "octocat/Hello-World",
          "url": "https://api.github.com/repos/octocat/Hello-World"
        },
        "actor": {
          "id": 1,
          "login": "octocat",
          "gravatar_id": "",
          "avatar_url": "https://github.com/images/error/octocat_happy.gif",
          "url": "https://api.github.com/users/octocat"
        },
        "org": {
          "id": 1,
          "login": "github",
          "gravatar_id": "",
          "url": "https://api.github.com/orgs/github",
          "avatar_url": "https://github.com/images/error/octocat_happy.gif"
        },
        "created_at": "2011-09-06T17:26:27Z",
        "id": "12345"
      }
    ]
    */
    public class GitHubEvents
    {
        public GitHubEvent[] Events { get; set; }
    }

    public class GitHubEvent
    {
        public string type { get; set; }
        public bool _public { get; set; }
        public Payload payload { get; set; }
        public Repo repo { get; set; }
        public Actor actor { get; set; }
        public Org org { get; set; }
        public DateTime created_at { get; set; }
        public string id { get; set; }
    }

    public class Payload
    {
    }

    public class Repo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Actor
    {
        public int id { get; set; }
        public string login { get; set; }
        public string gravatar_id { get; set; }
        public string avatar_url { get; set; }
        public string url { get; set; }
    }

    public class Org
    {
        public int id { get; set; }
        public string login { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string avatar_url { get; set; }
    }

}
