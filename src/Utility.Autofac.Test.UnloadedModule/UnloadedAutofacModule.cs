using Autofac;

namespace Utility.Autofac.Test.UnloadedModule
{
  public class UnloadedAutofacModule : global::Autofac.Module
  {
    protected override void Load(global::Autofac.ContainerBuilder builder)
    {
      base.Load(builder);

      builder.RegisterInstance("UnloadedAutofacModule").Named<string>("UnloadedAutofacModule");
    }
  }
}