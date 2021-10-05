using System;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace IpharmWebAppProject.Models
{
    public class API
    {
        [Key]
        [Required(ErrorMessage = "Posts are required")]
        [DataType(DataType.Text)]
        public string posts { get; set; }

        private string getPosts()
        {
            var url = "https://api.twitter.com/1.1/search/tweets.json?q=%23ipharm&result_type=recent";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Headers["Authorization"] = "Bearer AAAAAAAAAAAAAAAAAAAAABWVUQEAAAAA3pjPn0RXe93LEdgfdW7f1JdHhbI%3DTtCR0sZtohOeFmmo20FuQeBaXWnp26tWTktL4ZA20ziM81e5VA";


            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }

        public API()
        {
            posts = getPosts();
        }
    }
}