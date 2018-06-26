using Autofac;
using Prism.Events;
using ReadToday.Contracts;
using ReadToday.DataAccess;
using ReadToday.UI.DataProvider;
using ReadToday.UI.Dialogs;
using ReadToday.UI.View;
using ReadToday.UI.ViewModel;

namespace ReadToday.UI.Startup
{
    class CBootStrapper
    {
        public IContainer BootStrup()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>()
                .As<IEventAggregator>().SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<CMainWindowModel>().AsSelf();

            builder.RegisterType<CUserLoginViewModel>()
                .Keyed<IContentViewModel>(nameof(CUserLoginViewModel));
            builder.RegisterType<CContentViewModel>()
                .Keyed<IContentViewModel>(nameof(CContentViewModel));

            builder.RegisterType<CMessageDialogService>()
                .As<IMessageDialogService>();

            builder.RegisterType<CNavigationViewModel>()
                .As<INavigationViewModel>();

            builder.RegisterType<CBookEditViewModel>()
                .Keyed<IEditViewModel>(nameof(CBookEditViewModel));
            builder.RegisterType<CShelfEditViewModel>()
                .Keyed<IEditViewModel>(nameof(CShelfEditViewModel));

            builder.RegisterType<CUserDataProvider>().AsSelf();

            builder.RegisterType<CLookupDataService>()
                .AsImplementedInterfaces();

            builder.RegisterType<LookupClient>()
                .As<ILookupService>();

            builder.RegisterType<CBookRepository>()
                .As<IBookRepository>();
            builder.RegisterType<CShelfRepository>()
                .As<IShelfRepository>();

            builder.RegisterType<CFileDataService>()
                .As<IDataService>();

            builder.RegisterType<CReadTodayDbContext>().AsSelf();

            return builder.Build();
        }
    }
}
