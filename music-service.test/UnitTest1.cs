using music_service.Controllers;

namespace music_service.test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            WeatherForecastController weather = new WeatherForecastController(null);
            var result = weather.Get();
            Assert.IsNotNull(result); 
        }
    }
}