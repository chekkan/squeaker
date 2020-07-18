using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Squeaker.Api;
using TechTalk.SpecFlow;
using Xunit;

namespace Squeaker.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class GetSqueakesSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly WebApplicationFactory<Startup> webAppFactory;
        private HttpResponseMessage response;

        public GetSqueakesSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
            this.webAppFactory = new WebApplicationFactory<Startup>();
        }

        [Given(@"the following squeakes")]
        public void GivenTheFollowingSqueakes(Table table)
        {
            // this.scenarioContext.Pending();
        }

        [When(@"I GET (.*)")]
        public async Task WhenIRequestPath(string path)
        {
            var client = this.webAppFactory.CreateClient();

            // Act
            this.response = await client.GetAsync(path);
        }

        [Then(@"the response status should be (.*)")]
        public void ThenTheResponseStatusShouldBe(int statusCode)
        {
            Assert.Equal(statusCode, (int)response.StatusCode);
        }

        [Then(@"the response body should be valid according to openapi description GetSqueakesListResponse in file \./src/Squeaker\.Api/www/squeaker-swagger-spec\.json")]
        public void ThenTheRespBodyShouldBeValidAccordingToOpenapiDescriptionInFile()
        {
            this.scenarioContext.Pending();
        }

        [Then(@"response header X-Total-Count should be (.*)")]
        public void ThenResponseHeaderX_Total_CountShouldBe(int p0)
        {
            this.scenarioContext.Pending();
        }
    }
}
