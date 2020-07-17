using System;
using TechTalk.SpecFlow;

namespace Squeaker.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class GetSqueakesSteps
    {
        private readonly ScenarioContext scenarioContext;

        public GetSqueakesSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Given(@"the following squeakes")]
        public void GivenTheFollowingSqueakes(Table table)
        {
            this.scenarioContext.Pending();
        }

        [When(@"I GET (.*)")]
        public void WhenIRequestPath(string path)
        {
            this.scenarioContext.Pending();
        }

        [Then(@"the response status should be (.*)")]
        public void ThenTheResponseStatusShouldBe(int statusCode)
        {
            this.scenarioContext.Pending();
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
