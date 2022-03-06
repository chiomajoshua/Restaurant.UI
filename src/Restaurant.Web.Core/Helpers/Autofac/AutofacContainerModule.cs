using Autofac;

namespace Restaurant.Web.Core.Helpers.Autofac
{
    public class AutofacContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IAutoDependencyCore).Assembly)
             .AssignableTo<IAutoDependencyCore>()
             .As<IAutoDependencyCore>()
             .AsImplementedInterfaces().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}