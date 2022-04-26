using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Helpers
{
    public class Requesters
    {
        public static async Task<HttpStatusCode> GetStatusFromUrl(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                return response.StatusCode;
            }
            catch (HttpRequestException)
            {
                return HttpStatusCode.NotFound;
            }
            
        }
    }
}
