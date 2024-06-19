using ACDCs.App.Desktop;
using ACDCs.Interfaces;
using Moq;


namespace ACDCs.Tests
{
    public class DekstopTest
    {
        [Fact]
        public async void Desktop_Can_Start()
        {
            ITaskbarView taskbar = Mock.Of<ITaskbarView>();
            IWindowService windowService = Mock.Of<IWindowService>();
            DesktopView desktopView = new DesktopView(taskbar, windowService);
            await desktopView.StartDesktop();
        }

        [Fact]
        public void Taskbar_Can_Start()
        {
            IThemeService themeService = Mock.Of<IThemeService>();
            IStartButtonView startButtonView = Mock.Of<IStartButtonView>();
            IStartMenuView startMenuView = Mock.Of<IStartMenuView>();
            IWindowBar windowBar = Mock.Of<IWindowBar>();
            TaskbarView taskbarView = new TaskbarView(themeService, startButtonView, startMenuView, windowBar);
            Assert.NotNull(taskbarView);
        }

        [Fact]
        public async void StartButton_Should_Open_StartMenu()
        {
            IThemeService themeService = Mock.Of<IThemeService>();
            IStartMenuView startMenu = Mock.Of<IStartMenuView>();
            StartButtonView startButtonView = new StartButtonView(themeService);
            await startButtonView.SetStartMenu(startMenu);
           
        }

    }
}