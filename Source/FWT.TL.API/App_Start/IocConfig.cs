using Autofac;
using Autofac.Integration.WebApi;
using FWT.TL.Core;
using FWT.TL.Core.Providers;
using FWT.TL.Core.Services.Telegram;
using FWT.TL.Infrastructure;
using FWT.TL.Infrastructure.Telegram;
using OpenTl.ClientApi;
using System.Reflection;
using System.Web.Http;

namespace FWT.TL.API
{
    public class IocConfig
    {
        private static IContainer _container;

        public static IContainer RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.Register<IUserSessionManager>(b =>
            {
                return AppUserSessionManager.Instance.UserSessionManager;
            }).SingleInstance();

            builder.Register(b =>
            {
                var manager = b.Resolve<IUserSessionManager>();
                var currentUserId = b.Resolve<IUserProvider>()?.CurrentUserId;
                return manager.Get(currentUserId.ToString());
            }).InstancePerRequest();

            _container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(_container);
            return _container;
        }
    }
}
