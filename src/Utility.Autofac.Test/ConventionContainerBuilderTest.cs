using System;
using System.Linq;
using Autofac;
using NUnit.Framework;
using Autofac.Core.Resolving;

namespace Utility.Autofac.Test
{
  /// <summary>
  /// Note: Run these unit tests separately.
  /// 
  /// Some depend on a fresh AppDomain to pass.
  /// </summary>
  public class ConventionContainerBuilderTest
  {
    [Test]
    public void ContainerLoadsLoadedAutofacModule()
    {
      var container = ConventionContainerBuilder.BuilderFromModules().Build();

      Assert.That(container, Is.Not.Null);

      var result = container.ResolveNamed<string>("LoadedAutofacModule");

      Assert.That(result, Is.Not.Null);
      Assert.That(result, Is.EqualTo("LoadedAutofacModule"));
    }

    [Test]
    public void ContainerLoadsUnloadedAutofacModule()
    {
      Assert.That(AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().Name == "Utility.Autofac.Test.UnloadedModule"), Is.False);

      var container = ConventionContainerBuilder.BuilderFromModules().Build();

      Assert.That(AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().Name == "Utility.Autofac.Test.UnloadedModule"), Is.True);
      Assert.That(container, Is.Not.Null);

      var result = container.ResolveNamed<string>("UnloadedAutofacModule");

      Assert.That(result, Is.Not.Null);
      Assert.That(result, Is.EqualTo("UnloadedAutofacModule"));
    }
  }
}