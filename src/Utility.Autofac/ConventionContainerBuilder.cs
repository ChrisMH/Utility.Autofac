using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;

namespace Utility.Autofac
{
  /// <summary>
  /// Builds Autofac containers by convention
  /// </summary>
  public static class ConventionContainerBuilder
  {
    /// <summary>
    /// Builds an Autofac container based on discovered modules
    /// 
    /// The convention:
    /// 
    /// Any component requiring Autofac registration of components should
    /// do that registration in a class derived from Autofac.Module.
    /// 
    /// This implementation will scan all assemblies in the application domain's base
    /// directory (including subdirectories), create an instance of and register
    /// any Autofac.IModule - derived concrete class that it finds.
    /// </summary>
    /// <param name="excludeAutofacModules">true it exclude any IModule-derived class
    /// defined within Autofac itself</param>
    /// <returns>A ContainerBuilder containing all discovered module registrations</returns>
    public static ContainerBuilder BuilderFromModules(bool excludeAutofacModules = true)
    {

      var autofacModuleTypes = new List<Type>();

      // Scan for IModule-derived concrete types
      var di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
      var assemblyFileNames = di.GetFiles("*.dll", SearchOption.AllDirectories)
        .Where(fi => !(excludeAutofacModules && Path.GetFileNameWithoutExtension(fi.FullName).StartsWith("Autofac")))
        .Select(fi => fi.FullName);
            
      foreach (var assemblyFileName in assemblyFileNames)
      {
        var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(a => a.CodeBase.Equals(assemblyFileName, StringComparison.CurrentCultureIgnoreCase));
            
        if(assembly == null)
        {
          try
          {
            assembly = Assembly.LoadFile(assemblyFileName);
          }
          catch (BadImageFormatException)
          {
            // Not a .Net assembly
            continue;
          }
        }

        autofacModuleTypes.AddRange(assembly.GetTypes().Where(t => !t.IsAbstract && t.GetInterfaces().Contains(typeof (global::Autofac.Core.IModule))));
      }
      
      // Create and register all discovered module types
      var builder = new ContainerBuilder();
      foreach (var autofacModuleType in autofacModuleTypes)
      {
        var module = (global::Autofac.Module)autofacModuleType.Assembly.CreateInstance(autofacModuleType.FullName);
        builder.RegisterModule(module);
      }

      return builder;
    }
  }
}
