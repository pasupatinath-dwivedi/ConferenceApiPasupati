using System.Threading.Tasks;
using ConferenceApi.Controllers;
using ConferenceServiceLibs;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ConferenceApiTest
{
    public class ConferenceControllerTest
    {
        [Fact]
        public async Task SpeakersAndSessionsAsyncShouldReturnOkResult()
        {
            //Arrange
            var mockConferenceService = new Mock<IConferenceService>();
            mockConferenceService.Setup(service => service.GetSpeakersAndSessionsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new JObject());

            var controller = new ConferenceController(mockConferenceService.Object);

            //Act  
            var data = await controller.SpeakersAndSessionsAsync();

            //Assert  
            Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(data);
        }

        [Fact]
        public async Task SessionShouldReturnOKResult()
        {
            //Arrange
            var mockConferenceService = new Mock<IConferenceService>();
        
            mockConferenceService.Setup(service => service.GetSessionByIdAsync(It.IsAny<int>())).ReturnsAsync(new JObject());

            var controller = new ConferenceController(mockConferenceService.Object);
            int id = 100;
            //Act  

            var data = await controller.GetAsync(id);

            //Assert  
            Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(data);
        }

        [Fact]
        public async Task SessionAsyncShouldReturnBadRequest()
        {
            //Arrange
            var mockConferenceService = new Mock<IConferenceService>();
 
            mockConferenceService.Setup(service => service.GetSessionByIdAsync(It.IsAny<int>())).ReturnsAsync(new JObject());

            var controller = new ConferenceController(mockConferenceService.Object);
            int id = -1;

            //Act  
            var data = await controller.GetAsync(id);

            //Assert  
            Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestResult>(data);
        }
    }
}
