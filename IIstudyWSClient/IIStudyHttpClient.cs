using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIstudyWSClient
{
    public class IIStudyHttpClient
    {
        private static HttpClient CreateClient()
        {
            SocketsHttpHandler handler = new SocketsHttpHandler();
            handler.PooledConnectionLifetime = TimeSpan.FromMinutes(10);
            handler.ConnectTimeout = TimeSpan.FromSeconds(15);
            return new HttpClient(handler);
        }

        private static readonly HttpClient httpClient = CreateClient();

        private IIStudyHttpClient() { }

        public static HttpClient Insatnce
        {
            get
            {
                return httpClient;
            }
        }

        
    }
}
