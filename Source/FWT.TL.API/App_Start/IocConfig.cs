using Auth.FWT.Data;
using Autofac;
using Autofac.Integration.WebApi;
using FWT.TL.API.Providers;
using FWT.TL.Core.Data;
using FWT.TL.Core.Providers;
using FWT.TL.Core.Services.Telegram;
using FWT.TL.Infrastructure.Telegram;
using NodaTime;
using System.Net.Http;
using System.Reflection;
using System.Web;
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

            builder.Register<IUserProvider>(b =>
            {
                HttpRequestMessage httpRequestMessage = HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                return new UserProvider(httpRequestMessage);
            }).InstancePerRequest();

            builder.Register(b =>
            {
                var manager = b.Resolve<IUserSessionManager>();

                var userProvider = b.Resolve<IUserProvider>();
                if (userProvider.IsAuthenticated)
                {
                    return manager.Get(userProvider.CurrentUserId.ToString());
                }

                return manager.GetAnonymous();
            }).InstancePerRequest();

            builder.Register<IUserSessionManager>(b =>
            {
                return AppUserSessionManager.Instance.UserSessionManager;
            }).SingleInstance();

            builder.RegisterType(typeof(IUnitOfWork)).As(typeof(IUnitOfWork)).InstancePerDependency();

            builder.Register<IEntitiesContext>(b =>
            {
                var context = new AppContext("name=AppContext");
                return context;
            }).InstancePerDependency();

            builder.Register<IClock>(b =>
            {
                return SystemClock.Instance;
            }).SingleInstance();

            _container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(_container);
            return _container;
        }
    }
}