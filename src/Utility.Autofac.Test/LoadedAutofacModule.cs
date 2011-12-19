using Autofac;

namespace Utility.Autofac.Test
{
  public class LoadedAutofacModule : global::Autofac.Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      base.Load(builder);

      builder.RegisterInstance("LoadedAutofacModule").Named<string>("LoadedAutofacModule");
    }
  }
}