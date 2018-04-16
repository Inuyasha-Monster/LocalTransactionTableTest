using Productor.Data;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
    }
}
