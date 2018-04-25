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

        [Fact]
        public async Task Test()
        {
            var fuckPolicy = Policy<bool>.Handle<FuckException>().RetryAsync(1);
            var shitPolicy = Policy<bool>.Handle<ShitException>().FallbackAsync<bool>(x =>
            {
                var task = new Task<bool>(() => false);
                task.Start();
                return task;
            });
            var policy = Policy.WrapAsync(fuckPolicy, shitPolicy);

            var result = await policy.ExecuteAsync(TestFuckAndShit);
            Console.WriteLine(result);
        }

        private static async Task<bool> TestFuckAndShit()
        {
            var random = new Random();
            var num = random.Next(100);
            if (num <= 40)
                throw new FuckException();
            else if (num >= 50)
                throw new ShitException();
            else
            {
                return await Task.Run(() => true);
            }
        }

    }

    public class FuckException : Exception { }
    public class ShitException : Exception { }

}
