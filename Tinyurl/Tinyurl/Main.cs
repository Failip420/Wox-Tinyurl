using System;
using System.Collections.Generic;
using Wox.Plugin;
using RestSharp;
using System.Windows.Forms;

namespace Tinyurl
{
    public class Main : IPlugin
    {
        string shit = "";
        List<Result> results = new List<Result>();
        private static string url = "http://tinyurl.com/api-create.php?url=";
        public void Init(PluginInitContext context) { }
        public List<Result> Query(Query query)
        {
            var data = query.Search;
            PostUrl(data);
            results.Clear();
            results.Add(Result(shit, data, Action(shit)));
            
            return results;
        }
        
        private static Func<ActionContext, bool> Action(String text)
        {
            return e =>
            {   

                CopyToClipboard(text);

                // return false to tell Wox don't hide query window, otherwise Wox will hide it automatically
                return false;
            };
        }
        public void PostUrl(string link)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri(url);
            client.AddDefaultQueryParameter("url", link);
            var req = new RestRequest(Method.POST);
            var respone = client.ExecuteAsync(req);
            string cont = respone.Result.Content;
            shit = cont;
        }
        public static void CopyToClipboard(String text)
        {
            Clipboard.SetText(text);
        }
        public static Result Result(String title, String subtitle, Func<ActionContext, bool> action)
        {
            return new Result()
            {
                Title = title,
                SubTitle = subtitle,
                IcoPath = "Images\\Tinyurl.png",
                Action = action
            };
        }
    }
}
