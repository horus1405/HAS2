using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using HAS2.Core.Localization;

namespace HAS2
{
    public class IocConfig
    {
        //https://code.google.com/p/autofac/wiki/MvcIntegration

        public static IContainer CurrentContainer { get; protected set; }

        public static void RegisterDependencies()
        {
            //getting current assembly for scannning purposes
            var appAssembly = Assembly.GetExecutingAssembly();

            //creating the container builder
            var builder = new ContainerBuilder();

            //registering mvc controllers
            builder.RegisterControllers(appAssembly);

            //registering types
            builder.RegisterType<LocalizationManager>().As<ILocalizationManager>();
            builder.RegisterType<JsTranslationsScriptManager>().As<IJsTranslationsScriptManager>();
            builder.RegisterType<JsResourceScriptBase>().As<IJsResourceScriptManager>();
            builder.RegisterType<JsConfigurationScriptManager>().As<IJsConfigurationScriptManager>();
            
            //injecting http abstractions (optional)
            builder.RegisterModule(new AutofacWebTypesModule());

            //dependencies into view pages (optional)
            builder.RegisterSource(new ViewRegistrationSource());

            //resolving the container
            var container = builder.Build();
            CurrentContainer = container;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}