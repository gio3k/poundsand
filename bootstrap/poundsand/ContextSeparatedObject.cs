using System.Runtime.Loader;
using Sandbox.Internal;

namespace Poundsand.Internal;

public abstract class ContextSeparatedObject<T> where T : ContextSeparatedObject<T>, IAssemblyTypeObject, new()
{
	private static T? _default;
	private static T? _menu;
	private static T? _client;
	private static T? _server;

	private static T CreateObjectInstance( AssemblyLoadContext? context )
	{
		if ( context == null )
			throw new Exception( "AssemblyLoadContext was null" );

		var g = typeof(T);
		var assemblyName = (string)g.GetProperty( "AssemblyName" ).GetValue( null );
		var typeName = (string)g.GetProperty( "TypeName" ).GetValue( null );

		var assembly = context.FindAssembly( assemblyName );
		if ( assembly == null )
			throw new Exception( $"AssemblyLoadContext didn't have {assemblyName}" );

		var type = assembly.GetType( typeName );
		if ( type == null )
			throw new Exception( $"Assembly {assemblyName} didn't have {typeName}" );

		var instance = new T { Type = type };
		instance.Initialize();
		return instance;
	}

	public static T? Default
	{
		get
		{
			if ( _default != null )
				return _default;

			_default = CreateObjectInstance( Contexts.Default );

			return _default;
		}
	}

	public static T? Menu
	{
		get
		{
			if ( _menu != null )
				return _menu;

			_menu = CreateObjectInstance( Contexts.Menu );

			if ( _menu == null )
				GlobalSystemNamespace.Log.Warning( "Failed to create event instance for menu" );

			return _menu;
		}
	}

	public static T? Client
	{
		get
		{
			if ( _client != null )
				return _client;

			_client = CreateObjectInstance( Contexts.Client );

			if ( _client == null )
				GlobalSystemNamespace.Log.Warning( "Failed to create event instance for client" );

			return _client;
		}
	}

	public static T? Server
	{
		get
		{
			if ( _server != null )
				return _server;

			_server = CreateObjectInstance( Contexts.Server );

			if ( _server == null )
				GlobalSystemNamespace.Log.Warning( "Failed to create event instance for server" );

			return _server;
		}
	}

	public virtual void Initialize() { }
	public Type Type { get; init; }
}
