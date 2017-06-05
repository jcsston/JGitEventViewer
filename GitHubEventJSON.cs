using System;
using System.Collections.Generic;
using Windows.Data.Json;

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

        static public GitHubEvents Build(JsonArray a)
        {
            GitHubEvents e = new GitHubEvents();
            e.Events = new GitHubEvent[a.Count];
            for (int i = 0; i < a.Count; i++)
            {
                JsonObject o = a[i].GetObject();
                e.Events[i] = GitHubEvent.Build(o);
            }
            return e;
        }
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

        public override string ToString()
        {
            return String.Format("Created At: {0} Type: {1} Repo: {2} Actor: {3}", new object[] { created_at, type, repo, actor });
        }

        static public GitHubEvent Build(JsonObject o)
        {
            GitHubEvent e = new GitHubEvent();
            e.type = o.GetNamedString("type", "");
            e._public = o["public"].GetBoolean();
            if (o.ContainsKey("repo"))
            {
                e.repo = Repo.Build(o["repo"].GetObject());
            }
            if (o.ContainsKey("actor"))
            {
                e.actor = Actor.Build(o["actor"].GetObject());
            }
            if (o.ContainsKey("org"))
            {
                e.org = Org.Build(o["org"].GetObject());
            }
            e.created_at = DateTime.Parse(o["created_at"].GetString());
            e.id = o.GetNamedString("id", "");

            return e;
        }
    }

    public class Payload
    {
        static public Payload Build(JsonObject o)
        {
            Payload p = new Payload();
            // Payload is empty?
            return p;
        }
    }

    public class Repo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        
        public override string ToString()
        {
            return String.Format("{0} ({1})", name, id);
        }

        static public Repo Build(JsonObject o)
        {
            Repo r = new Repo();
            r.id = (int)o["id"].GetNumber();
            r.name = o["name"].GetString();
            r.url = o["url"].GetString();
            return r;
        }
    }

    public class Actor
    {
        public int id { get; set; }
        public string login { get; set; }
        public string gravatar_id { get; set; }
        public string avatar_url { get; set; }
        public string url { get; set; }

        public override string ToString()
        {
            return String.Format("{0} ({1})", login, id);
        }

        static public Actor Build(JsonObject o)
        {
            Actor a = new Actor();
            a.id = (int)o["id"].GetNumber();
            a.login = o["login"].GetString();
            a.gravatar_id = o["gravatar_id"].GetString();
            a.avatar_url = o["avatar_url"].GetString();
            a.url = o["url"].GetString();
            return a;
        }
    }

    public class Org
    {
        public int id { get; set; }
        public string login { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string avatar_url { get; set; }

        public override string ToString()
        {
            return String.Format("{0} ({1})", login, id);
        }

        static public Org Build(JsonObject o)
        {
            Org a = new Org();
            a.id = (int)o["id"].GetNumber();
            a.login = o["login"].GetString();
            a.gravatar_id = o["gravatar_id"].GetString();
            a.avatar_url = o["avatar_url"].GetString();
            a.url = o["url"].GetString();
            return a;
        }
    }

}
