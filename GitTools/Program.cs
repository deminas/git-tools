using DevExpress.XtraBars.FluentDesignSystem;
using DevExpress.XtraEditors;
using GitTools.Configs;
using GitTools.Core.BaseCore;
using GitTools.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace GitTools
{
    /// <summary>
    /// Жизненный цикл
    /// </summary>
    internal enum DiRegisterType
    {
        /// <summary>
        ///  сервис создается каждый раз, когда его запрашивают.
        ///  Этот жизненный цикл лучше всего подходит для легковесных,
        ///  не фиксирующих состояние, сервисов.
        /// </summary>
        Transient,

        /// <summary>
        /// создаются единожды для каждого запроса
        /// </summary>
        Scoped,

        /// <summary>
        /// создается при первом запросе
        /// (или при запуске ConfigureServices, если вы указываете инстанс там),
        /// а затем каждый последующий запрос будет использовать этот же инстанс
        /// </summary>
        Singleton
    }

    /// <summary>
    /// https://stackoverflow.com/questions/70475830/how-to-use-dependency-injection-in-winforms
    /// </summary>
    internal static class Program
    {
        private static readonly IServiceCollection _services = new ServiceCollection();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            using var provider = _services
                .ConfigureServices()
                .BuildServiceProvider();

            var mainForm = provider.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }

        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton(services);
            services.AddSingleton<AppConfig>();

            services.RegisterAllByBaseType<BaseTransientCore>();
            services.RegisterAllByBaseType<BaseScopedCore>(DiRegisterType.Scoped);
            services.RegisterAllByBaseType<BaseSingletonCore>(DiRegisterType.Singleton);

            services.RegisterAllByBaseType<FluentDesignForm>();
            services.RegisterAllByBaseType<XtraForm>();

            return services;
        }

        private static IServiceCollection RegisterAllByBaseType<T>(this IServiceCollection services, DiRegisterType registerType = DiRegisterType.Transient)
        {
            var types = typeof(Program).Assembly
                .GetTypes()
                .Where(t => t.BaseType == typeof(T))
                .Where(t => !t.IsAbstract)
                .ToList();

            switch (registerType)
            {
                case DiRegisterType.Transient:
                    types.ForEach(t => services.AddTransient(t));
                    break;
                case DiRegisterType.Scoped:
                    types.ForEach(t => services.AddScoped(t));
                    break;
                case DiRegisterType.Singleton:
                    types.ForEach(t => services.AddSingleton(t));
                    break;
                default:
                    throw new NotSupportedException($"Dependency injection initialization with RegisterType {registerType} not supported");
            }

            return services;
        }

        private static IServiceCollection RegisterAllByInterfaces<I>(this IServiceCollection services, DiRegisterType registerType)
        {
            var typeName = typeof(I).FullName ?? string.Empty;

            var types = typeof(Program).Assembly
                .GetTypes()
                .Where(t => t.GetInterface(typeName) is not null)
                .Where(t => !t.IsAbstract)
                .ToList();

            switch (registerType)
            {
                case DiRegisterType.Transient:
                    types.ForEach(t => services.AddTransient(typeof(I), t));
                    break;
                case DiRegisterType.Scoped:
                    types.ForEach(t => services.AddScoped(typeof(I), t));
                    break;
                case DiRegisterType.Singleton:
                    types.ForEach(t => services.AddSingleton(typeof(I), t));
                    break;
                default:
                    throw new NotSupportedException($"Dependency injection initialization with RegisterType {registerType} not supported");
            }

            return services;
        }
    }
}