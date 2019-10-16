using System;
using System.Net.Http;

namespace KentekenClient.ApiConnect
{
    public class KentekenAPI
    {
        public HttpClient Initial()
        {
            //We maken centraal connectie met de Web Api
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:50970")
            };
            return client;
        }
    }
}
