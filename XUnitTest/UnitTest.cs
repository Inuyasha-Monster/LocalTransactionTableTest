using Productor.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;
using Productor.Common;
using Productor.Model;
using RestSharp;
using Xunit;

namespace XUnitTest
{
    public class UnitTest
    {
        [Fact]
        public void Test_CreateOrder()
        {
            RestClient client = new RestClient("http://localhost:8888/");
            var request = new RestRequest("api/order/", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddBody(new OrderInput()
            {
                AppUser = "¿œ∂≈",
                Details = new List<OrderDetailInput>()
                {
                    new OrderDetailInput()
                    {
                        Sku="5456456456",
                        SkuName = "∫Ï…’ªÿπ¯»‚",
                        Price = 18,
                        Quantity = 2
                    }
                }
            });
            var reponse = client.Execute(request);
            var apiResult = JsonConvert.DeserializeObject<ApiResult>(reponse.Content);
            Assert.True(apiResult?.Successed);
        }

        [Fact]
        public async Task Test_Retry()
        {
            var policy = Policy.Handle<HttpRequestException>().Or<ArgumentNullException>().RetryAsync(3);
            var result = await policy.ExecuteAsync(GetGoogle);
            Assert.NotNull(result);
        }


        private async Task<HttpResponseMessage> GetGoogle()
        {
            var httpClient = new HttpClient();
            var result = await httpClient.GetAsync("http://www.google.com");
            return result;
        }
    }
}
