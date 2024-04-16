using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AutoMapper;
using Application.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Application.Behaviors;
using FluentValidation;
using MediatR;
using Serilog;
using System.ComponentModel.DataAnnotations;
using Redis.Services;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using StackExchange.Redis.Extensions.Utf8Json;
using Application.Features.Base;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR.NotificationPublishers;

namespace Application
{
    public static class ApplicationRegistration
    {
        /// <summary>
        /// کانفیگ های پایه و عمومی که در همه پروژه ها لازم است
        /// </summary>
        public static void RegisterApplication(
            this WebApplicationBuilder builder,
            IConfiguration configuration)
        {
            var services = builder.Services;
            var assembly = Assembly.GetExecutingAssembly();
            var type = typeof(ApplicationRegistration);

            services.AddCustomAutoMapper(assembly);

            services.AddRedis(configuration);

            //راه اندازی سریلاگ
            builder.RegisterSerilog();

            services.AddFluentValidation(type);

            //autofac DI
            services.AddMediatR(assembly);
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>((hostBuilder, containerBuilder) =>
                {
                    containerBuilder.AddMediatrHandlersWithOpenGeneric(assembly);
                });
        }



        #region افزودن AutoMapper به سرویس ها
        public static void AddCustomAutoMapper(this IServiceCollection services, Assembly assembly)
        {
            services.AddAutoMapper(assembly);
            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                #region مپینگ لایه اپلیکیشن
                cfg.ShouldMapMethod = x => false;
                cfg.AddProfile(new MappingProfile(assembly));

                // مپینگ های کاستوم
                var types = assembly.GetTypes().Where(t => typeof(Profile).IsAssignableFrom(t) && t != typeof(MappingProfile));
                foreach (var type in types)
                {
                    var instance = Activator.CreateInstance(type);
                    cfg.AddProfile((Profile)instance);
                }
                #endregion

            }).CreateMapper());
        }
        #endregion




        #region افزودن mediateR به سرویس

        /// <summary>
        /// افزودن mediateR به سرویس
        /// با DI AutoFac
        /// </summary>
        public static ContainerBuilder AddMediatrHandlersWithOpenGeneric(
            this ContainerBuilder builder,
            Assembly assembly)
        {
            builder.RegisterGeneric(typeof(GetByIdQueryHandler<,>)).AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(DropdownQueryHandler<>)).AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(SearchQueryHandler<>)).AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(ToggleEnableCommandHandler<>)).AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(DeleteCommandHandler<>)).AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(LogNotificationHandler<,>)).AsImplementedInterfaces();

            return builder;
        }

        /// <summary>
        /// افزودن mediateR به سرویس
        /// </summary>
        /// <param name="services"></param>
        public static void AddMediatR(this IServiceCollection services, Assembly assembly)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(assembly);

                // استراتژی پابلیش نوتیفیکیشن mediatR
                //ForeachAwaitPublisher => در حلقه یکی یکی هندلر ها رو اجرا میکنه و خطا در یکی کار رو متوقف میکنه
                // TaskWhenAllPublisher => هندلر ها موازی اجرا شده و خطای یکی روی بقیه اثر نداره
                config.NotificationPublisher = new TaskWhenAllPublisher();
                config.NotificationPublisherType = typeof(TaskWhenAllPublisher);

                config.Lifetime = ServiceLifetime.Transient;
            });

            //پایپ لاین های mediateR
            services.ConfigurePipelines();
        }



        #region پایپ لاین ها - behaviors
        public static void ConfigurePipelines(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
        #endregion
        #endregion



        #region افزودن FluentValidation به سرویس
        /// <summary>
        /// افزودن FluentValidation به سرویس
        /// <para>
        /// services.AddFluentValidation(typeof(ApplicationRegistration))
        /// </para>
        /// </summary>
        /// <param name="type">نوع یک کلاس از لایه اپلیکیشن پروزه ی فراخوانی کننده</param>
        public static void AddFluentValidation(this IServiceCollection services, Type type)
        {
            services.AddValidatorsFromAssemblyContaining(type);
            FluentValidationDisplayNameResolver();
        }


        #region نمایش اتریبیوت DisplayName بجای نام پروپرتی در فروئنت ولیدیشن
        public static void FluentValidationDisplayNameResolver()
        {
            ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
            {
                if (memberInfo != null)
                {
                    var attr = memberInfo.GetCustomAttribute(typeof(DisplayAttribute), false) as DisplayAttribute;
                    if (attr != null)
                        return attr.Name;
                }
                return memberInfo?.Name;
            };
        }
        #endregion
        #endregion



        #region کانفیگ سریلاگ
        public static void RegisterSerilog(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.Console()
            //    .WriteTo.File(new JsonFormatter(),
            //        "logs/important-logs-.json",
            //        rollingInterval: RollingInterval.Day,
            //        restrictedToMinimumLevel: LogEventLevel.Warning)

            //    .WriteTo.File("logs/daily-log-.logs",
            //        rollingInterval: RollingInterval.Day)

            //    .MinimumLevel.Debug()

            //    .CreateLogger();

            builder.Host.UseSerilog();
        }
        #endregion



        #region کانفیگ ردیس
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {

            var RedisConfigurations = new List<RedisConfiguration>();
            var redisConfig = configuration.GetSection("Redis").Get<RedisConfiguration>();
            RedisConfigurations.Add(redisConfig);

            services.AddStackExchangeRedisExtensions<Utf8JsonSerializer>((options) =>
            {
                return RedisConfigurations;
            }).AddStackExchangeRedisExtensions<NewtonsoftSerializer>((options) =>
            {
                return RedisConfigurations;
            });

            services.AddSingleton<IRedisManager, RedisManager>();
        }
        #endregion

    }
}
