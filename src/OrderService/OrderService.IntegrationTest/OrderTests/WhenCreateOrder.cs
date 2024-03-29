﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OrderService.API;
using OrderService.API.Controllers.Requests;
using Xunit;

namespace OrderService.IntegrationTest.OrderTests
{
    public class WhenCreateOrder : IClassFixture<WebApplicationFactoryWithInMemorySqlite<Startup>>
    {
        private readonly WebApplicationFactoryWithInMemorySqlite<Startup> _factory;

        public WhenCreateOrder(WebApplicationFactoryWithInMemorySqlite<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ShouldCreated()
        {
            HttpClient client = _factory.CreateClient();

            var req = new CreateOrderRequest()
            {
                UserId = Guid.NewGuid().ToString(),
                UserName = "TestUser",
                City = "Beijing",
                ZipCode = "100000",
                OrderItems = new List<CreateOrderRequest.OrderItem>
                {
                    new CreateOrderRequest.OrderItem
                    {
                        ProductId = 1,
                        ProductName = "iPhone",
                        UnitPrice = 8888,
                        Units = 1
                    }
                }
            };

            // act
            using var content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PostAsync("/api/order", content);

            // assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
