using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using System.Reflection;
using System.Diagnostics.Contracts;


namespace AllanGray.Technology
{
	public sealed class RuntimeContext : IDisposable
	{
		//this must be a singleton
		private static volatile RuntimeContext _instance;
		private static object syncRoot = new Object ();

		private static object _container = null;

		private RuntimeContext ()
		{

		}

		private static void RegisterTypesInAssembly (Assembly a, ContainerBuilder b)
		{
			b.RegisterAssemblyTypes (a);                
		}

		private static string GetConfig (string key)
		{
			string result = System.Configuration.ConfigurationManager.AppSettings [key];
			if (string.IsNullOrWhiteSpace (result))
				throw new Exception (string.Format ("Configure key '{0}' in config file.", key));
			return result;
		}

		public static void Configure (string configurationKey)
		{
			ContainerBuilder _builder = new ContainerBuilder ();
			string[] assembliesNames = GetConfig (configurationKey).Split (';');
			foreach (string a in assembliesNames) {
				//an earlier version of this method looked for classes that were annotated by a specific Attribute but I wanted to
				//cut down on the cognitive load all this stuff imposes.
				_builder.RegisterAssemblyTypes (Assembly.LoadFrom (string.Format ("{0}.dll", a.Trim ()))).AsImplementedInterfaces ();                
			}
			_container = _builder.Build ();
		}
		public static void Configure(params Assembly[] assemblies)
		{
			ContainerBuilder _builder = new ContainerBuilder();
			_builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces();
			_container = _builder.Build();
	}
		public static T Resolve<T> ()
		{
			Contract.Requires ((_container as IContainer).IsRegistered<T> (), "Type not found in Resolver");
			return (_container as IContainer).Resolve<T> ();
		}

		public static T Resolve<T> (params Parameter[] parameters)
		{
			Contract.Requires ((_container as IContainer).IsRegistered<T> (), "Type not found in Resolver");
			return (_container as IContainer).Resolve<T> (parameters);
		}

		public static T Resolve<T> (IEnumerable<Parameter> parameters)
		{
			Contract.Requires ((_container as IContainer).IsRegistered<T> (), "Type not found in Resolver");
			return (_container as IContainer).Resolve<T> (parameters);
		}

		public static bool IsRegistered<T> ()
		{
			return (_container as IContainer).IsRegistered<T> ();
		}

		private static RuntimeContext Instance {
			get {
				if (_instance == null) {
					lock (syncRoot) {
						if (_instance == null) {
							_instance = new RuntimeContext ();
						}
					}
				}
				return _instance;
			}
		}

		#region IDisposable Members

		public void Dispose ()
		{
			if (_container != null) {
				(_container as IContainer).Dispose ();
				_container = null;
			}

		}

		#endregion
	}
}
