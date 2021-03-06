﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Squeaker.Api;
using Squeaker.Application;
using TechTalk.SpecFlow;
using Xunit;

namespace Squeaker.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class GetSqueakesSteps
    {
        private readonly ScenarioContext scenarioContext;
        private static HttpClient client = SetupClient();
        private HttpResponseMessage response;

        public GetSqueakesSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        public static HttpClient SetupClient()
            => new CustomWebApplicationFactory<Startup>().CreateClient();

        [Given(@"the following squeakes")]
        public void GivenTheFollowingSqueakes(Table table)
        {
        }

        [When(@"I GET (.*)")]
        public async Task WhenIRequestPath(string path)
        {
            this.response = await client.GetAsync(path);
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
            JToken parsed = JToken.Parse(body);
            JToken token = parsed.SelectToken(path);
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

            JToken json = JToken.Parse(body);

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