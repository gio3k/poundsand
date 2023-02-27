using System.Reflection;
using System.Runtime.Loader;

namespace Poundsand.Internal;

/// <summary>
/// Helper class for finding specific AssemblyLoadContexts
/// </summary>
public static class Contexts
{
	private static AssemblyLoadContext? GetServerContext()
	{
		return AssemblyLoadContext.All
			.Where( assemblyLoadContext => assemblyLoadContext != AssemblyLoadContext.Default )
			.FirstOrDefault( assemblyLoadContext =>
				assemblyLoadContext.Assemblies.Any( v => v.GetName().Name == "Sandbox.Server" ) );
	}

	private static AssemblyLoadContext? GetClientContext()
	{
		return AssemblyLoadContext.All
			.Where( assemblyLoadContext => assemblyLoadContext != AssemblyLoadContext.Default )
			.FirstOrDefault( assemblyLoadContext =>
				assemblyLoadContext.Assemblies.Any( v => v.GetName().Name == "Sandbox.Client" ) );
	}

	private static AssemblyLoadContext? GetMenuContext()
	{
		return AssemblyLoadContext.All
			.Where( assemblyLoadContext => assemblyLoadContext != AssemblyLoadContext.Default )
			.FirstOrDefault( assemblyLoadContext =>
				assemblyLoadContext.Assemblies.Any( v => v.GetName().Name == "Sandbox.Menu" ) );
	}

	private static AssemblyLoadContext? _serverContext;
	private static AssemblyLoadContext? _clientContext;
	private static AssemblyLoadContext? _menuContext;

	public static AssemblyLoadContext? Server
	{
		get
		{
			_serverContext ??= GetServerContext();
			return _serverContext;
		}
	}

	public static AssemblyLoadContext? Client
	{
		get
		{
			_clientContext ??= GetClientContext();
			return _clientContext;
		}
	}

	public static AssemblyLoadContext? Menu
	{
		get
		{
			_menuContext ??= GetMenuContext();
			return _menuContext;
		}
	}

	public static AssemblyLoadContext Default => AssemblyLoadContext.Default;

	public static Assembly? FindAssembly( this AssemblyLoadContext context, string name ) =>
		context.Assemblies.SingleOrDefault( v => v.GetName().Name == name );
}
