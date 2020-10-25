using System.Windows;
using FileRenamer.Services;
using FileRenamer.Services.Impl;
using Ninject;

namespace FileRenamer.Views
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();

            Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            _container = new StandardKernel();

            _container.Bind<IFileService>().To<FileService>();
            _container.Bind<IRenameFile>().To<FileRenameService>();
            _container.Bind<IExtractInformation>().To<TextExtractorCBAStatement>();
        }

        private void ComposeObjects()
        {
            Current.MainWindow = this._container.Get<MainWindow>();
            Current.MainWindow.Title = "Jordan's Pornhub Premium";
        }

    }
}