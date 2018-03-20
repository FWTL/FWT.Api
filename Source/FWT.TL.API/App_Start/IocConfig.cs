using Auth.FWT.Data;
using Autofac;
using Autofac.Integration.WebApi;
using FWT.TL.API.Providers;
using FWT.TL.Core;
using FWT.TL.Core.Data;
using FWT.TL.Core.Providers;
using FWT.TL.Core.Services.Telegram;
using FWT.TL.Infrastructure.Telegram;
using NodaTime;
using OpenTl.ClientApi;
using StackExchange.Redis;
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

            builder.Register<IUserSessionManager>(b =>
            {
                return AppUserSessionManager.Instance.UserSessionManager;
            }).SingleInstance();

            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerRequest();
            builder.RegisterType(typeof(SqlSessionStore)).As(typeof(ISessionStore)).InstancePerRequest();

            builder.Register<IEntitiesContext>(b =>
            {
                var context = new AppContext("name=AppContext");
                return context;
            }).InstancePerDependency();

            builder.Register<IClock>(b =>
            {
                return SystemClock.Instance;
            }).SingleInstance();

            builder.Register(b =>
            {
                return ConnectionMultiplexer.Connect(ConfigKeys.RedisConnectionString);
            }).SingleInstance();

            builder.Register(b =>
            {
                var redis = b.Resolve<ConnectionMultiplexer>();
                return redis.GetDatabase();
            }).InstancePerRequest();

            _container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(_container);
            return _container;
        }
    }
}