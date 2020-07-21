using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Squeaker.Api;
using Squeaker.Application;
using TechTalk.SpecFlow;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Squeaker.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class GetSqueakesSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly CustomWebApplicationFactory<Startup> webAppFactory;
        private HttpClient client;
        private HttpResponseMessage response;

        public GetSqueakesSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
            this.webAppFactory = new CustomWebApplicationFactory<Startup>();
        }

        [Given(@"the following squeakes")]
        public void GivenTheFollowingSqueakes(Table table)
        {
            this.client = this.webAppFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(async services =>
                {
                    var serviceProvider = services.BuildServiceProvider();

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices
                            .GetRequiredService<SqueakerContext>();

                        // empty the squeakes table
                        db.Squeakes.RemoveRange(db.Squeakes);

                        // add header as an entry
                        db.Squeakes.Add(new Squeake
                        {
                            Id = Guid.NewGuid().ToString(),
                            Text = table.Header.FirstOrDefault()
                        });

                        // add remaining rows
                        db.Squeakes.AddRange(table.Rows.Select(row => new Squeake
                        {
                            Id = Guid.NewGuid().ToString(),
                            Text = row[0]
                        }));

                        await db.SaveChangesAsync();
                    }
                });
            }).CreateClient();
        }

        [When(@"I GET (.*)")]
        public async Task WhenIRequestPath(string path)
        {
            // Act
            this.response = await this.client.GetAsync(path);
        }

        [Then(@"the response status should be (.*)")]
        public void ThenTheResponseStatusShouldBe(int statusCode)
        {
            Assert.Equal(statusCode, (int)response.StatusCode);
        }

        [Then(@"response body path (.*) should be (.*)")]
        public async Task ThenResponseBodyPathShouldBe(string path, string value)
        {
            var body = await this.response.Content.ReadAsStringAsync();
            JArray array = JArray.Parse(body);
            JToken token = array.SelectToken(path);
            Assert.Equal(value, token.ToObject<string>());
        }

        [Then(@"response body should be valid according to schema file (.*)")]
        public async Task ThenResponseBodyShouldBeValidAccordingToSchemaFile(
                                                             string filepath)
        {
            string schemaContent;
            using (var streamReader = new StreamReader(filepath))
            {
                schemaContent = await streamReader.ReadToEndAsync();
            }
            JSchema schema = JSchema.Parse(schemaContent);

            var body = await this.response.Content.ReadAsStringAsync();

            JArray json = JArray.Parse(body);

            bool isValid = json.IsValid(schema);
            Assert.True(isValid, "doesn't match schema");
        }

        [Then(@"response header X-Total-Count should be (.*)")]
        public void ThenResponseHeaderX_Total_CountShouldBe(string value)
        {
            var totalCount = response.Headers.GetValues("X-Total-Count")
                .FirstOrDefault();
            Assert.Equal(value, totalCount);
        }
    }
}