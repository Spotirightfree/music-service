using music_service.Controllers;

namespace music_service.test
{
    public class UnitTest1
    {
        [Fact]
        public void WeatherForecastTest()
        {
            WeatherForecastController wc = new WeatherForecastController(null);
            var results = wc.Get();
            Assert.NotNull(results);
        }
    }
}