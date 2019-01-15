using Autofac;
using Autofac.Extensions.DependencyInjection;
using FWTL.Core.CQRS;
using FWTL.Core.Extensions;
using FWTL.Core.Services.Dapper;
using FWTL.Core.Services.Identity;
using FWTL.Core.Services.Redis;
using FWTL.Core.Services.Sql;
using FWTL.Core.Services.Telegram;
using FWTL.Core.Services.Unique;
using FWTL.Database;
using FWTL.Infrastructure;
using FWTL.Infrastructure.CQRS;
using FWTL.Infrastructure.Dapper;
using FWTL.Infrastructure.EventHub;
using FWTL.Infrastructure.Identity;
using FWTL.Infrastructure.Telegram;
using FWTL.Infrastructure.Unique;
using FWTL.Infrastructure.User;
using FWTL.Infrastructure.Validation;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using System;
using System.Data.SqlClient;

namespace FWTL.Api
{
    public class IocConfig
    {
        public static void OverrideWithLocalCredentials(ContainerBuilder builder)
        {
            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var credentials = new TelegramDatabaseCredentials();
                credentials.BuildLocalConnectionString(
                        configuration["Api:Sql:Url"],
                        configuration["Api:Sql:Catalog"]);

                return credentials;
            }).SingleInstance();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var credentials = new HangfireDatabaseCredentials();
                credentials.BuildLocalConnectionString(
                        configuration["Hangfire:Sql:Url"],
                        configuration["Hangfire:Sql:Catalog"]);

                return credentials;
            }).SingleInstance();
        }

        public static void RegisterCredentials(ContainerBuilder builder)
        {
            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var credentials = new HangfireDatabaseCredentials();
                credentials.BuildConnectionString(
                        configuration["Hangfire:Sql:Url"],
                        configuration["Hangfire:Sql:Port"].To<int>(),
                        configuration["Hangfire:Sql:Catalog"],
                        configuration["Hangfire:Sql:User"],
                        configuration["Hangfire:Sql:Password"]);

                return credentials;
            }).SingleInstance();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var credentails = new IdentityModelCredentials()
                {
                    ClientId = configuration["Auth:Client:Id"],
                    ClientSecret = configuration["Auth:Client:Secret"]
                };

                return credentails;
            }).SingleInstance();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var credentails = new RedisCredentials();
                credentails.BuildConnectionString(
                    configuration["Redis:Name"],
                    configuration["Redis:Password"],
                    configuration["Redis:Port"].To<int>(),
                    isSsl: true,
                    allowAdmin: true);

                return ConnectionMultiplexer.Connect(credentails.ConnectionString);
            }).SingleInstance();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var credentials = new TelegramDatabaseCredentials();
                credentials.BuildConnectionString(
                    configuration["Api:Sql:Url"],
                    configuration["Api:Sql:Port"].To<int>(),
                    configuration["Api:Sql:Catalog"],
                    configuration["Api:Sql:User"],
                    configuration["Api:Sql:Password"]);

                return credentials;
            }).SingleInstance();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var settings = new TelegramSettings()
                {
                    AppHash = configuration["Telegram:Settings:AppHash"],
                    AppId = configuration["Telegram:Settings:AppId"].To<int>(),
                    ServerAddress = configuration["Telegram:Settings:ServerAddress"],
                    ServerPort = configuration["Telegram:Settings:ServerPort"].To<int>(),
                    ServerPublicKey = configuration["Telegram:Settings:ServerPublicKey"]
                };

                return settings;
            }).SingleInstance();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var connectionStringBuilder = new EventHubsConnectionStringBuilder(configuration["EventHub:ConnectionString"])
                {
                    EntityPath = configuration["EventHub:EntityPath"]
                };

                return connectionStringBuilder;
            }).SingleInstance();
        }

        public static IContainer RegisterDependencies(IServiceCollection services, IHostingEnvironment env, IConfiguration rootConfiguration)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var builder = new ContainerBuilder();
            builder.Populate(services);
            RegisterCredentials(builder);

            if (env.IsDevelopment())
            {
                OverrideWithLocalCredentials(builder);
            }

            builder.Register<IDiscoveryCache>(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var cache = new DiscoveryCache(configuration["Auth:Client:Url"]);
                return cache;
            }).SingleInstance();

            builder.Register(b =>
            {
                return rootConfiguration;
            }).SingleInstance();

            builder.Register(b =>
            {
                var redis = b.Resolve<ConnectionMultiplexer>();
                return redis.GetDatabase();
            }).InstancePerLifetimeScope();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var redis = b.Resolve<ConnectionMultiplexer>();
                return redis.GetServer(configuration["Redis:Url"]);
            }).InstancePerLifetimeScope();

            builder.RegisterType<IEventDispatcher>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IEventHandler<>)).InstancePerDependency();

            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(ICommandHandler<>)).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(ICommandHandler<,>)).InstancePerLifetimeScope();

            builder.RegisterType<QueryDispatcher>().As<IQueryDispatcher>().InstancePerRequest().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IQueryHandler<,>)).InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(AppAbstractValidation<>)).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IReadCacheHandler<,>)).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IWriteCacheHandler<,>)).InstancePerLifetimeScope();

            builder.Register<ILogger>(b =>
            {
                var format = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {NewLine}{Message:lj}{NewLine}{Exception}";

                return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: format)
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error, outputTemplate: format)
                .WriteTo.Logger(cl => cl.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Information).WriteTo.File("Logs/queries.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: format))
                .CreateLogger();
            }).SingleInstance();

            builder.Register<IClock>(b =>
            {
                return SystemClock.Instance;
            }).SingleInstance();

            builder.Register<IMemoryCache>(b =>
            {
                return new MemoryCache(new MemoryCacheOptions());
            }).SingleInstance();

            builder.RegisterType<GuidService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<TelegramService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<IdentityModelClient>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<CurrentUserProvider>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<RandomService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EventHubService>().AsImplementedInterfaces().SingleInstance();

            builder.Register<IClock>(b =>
            {
                return SystemClock.Instance;
            }).SingleInstance();

            builder.Register(b =>
            {
                var connectionStringBuilder = b.Resolve<EventHubsConnectionStringBuilder>();
                return EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            }).InstancePerLifetimeScope();

            builder.Register<IDatabaseConnector<TelegramDatabaseCredentials>>(b =>
            {
                var databaseCredentials = b.Resolve<TelegramDatabaseCredentials>();
                return new TelegramStoreDapperConnector<TelegramDatabaseCredentials>(databaseCredentials);
            }).InstancePerLifetimeScope();

            builder.Register<IDatabaseConnector<HangfireDatabaseCredentials>>(b =>
            {
                var databaseCredentials = b.Resolve<HangfireDatabaseCredentials>();
                return new DapperConnector<HangfireDatabaseCredentials>(new SqlConnection(databaseCredentials.ConnectionString));
            }).InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}