using ACDCs.App;
using ACDCs.Interfaces;
using Moq;

namespace ACDCs.Tests
{
    public class StartupTest
    {
        [Fact]
        public void Startup_Should_Launch_App()
        {
            MauiApp app = MauiProgram.CreateMauiApp("temp");
            Assert.NotNull(app);
        }

        [Fact]
        public void Startup_Xaml_Should_Get_Desktop()
        {
            MauiApp app = MauiProgram.CreateMauiApp("temp");
            Startup startup = new Startup();
            Assert.NotNull(startup);
        }

        [Fact]
        public void MainPage_Xaml_Should_Accept_DesktopView()
        {
            IDesktopView desktopView = Mock.Of<IDesktopView>();
            MainPage mainPage = new MainPage(desktopView);
            Assert.NotNull(mainPage);
        }
    }
}