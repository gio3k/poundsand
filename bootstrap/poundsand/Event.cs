using System.Reflection;
using Poundsand.Internal;

namespace Poundsand;

public class Event : ContextSeparatedObject<Event>, IAssemblyTypeObject
{
	public static string AssemblyName => "Sandbox.Event";
	public static string TypeName => "Sandbox.Event";
	
	/// <summary>Register an object to start receiving events</summary>
	public void Register( object obj ) => Type.GetMethod( "Register" ).Invoke( null, new[] { obj } );

	/// <summary>Unregister an object, stop receiving events</summary>
	public void Unregister( object obj ) => Type.GetMethod( "Unregister" ).Invoke( null, new[] { obj } );

	public void RegisterAssembly( Assembly o )
	{
		var manager =
			Type.GetField( "eventManager", BindingFlags.Static | BindingFlags.NonPublic )!
				.GetValue( null );
		manager?.GetType().GetMethod( "RegisterAssembly", BindingFlags.Instance | BindingFlags.NonPublic )!
			.Invoke( manager, new[] { (object?)null, o } );
	}

	public static void FullRegisterAssembly( Assembly o )
	{
		Server?.RegisterAssembly( o );
		Menu?.RegisterAssembly( o );
		Client?.RegisterAssembly( o );
		Default?.RegisterAssembly( o );
	}
}
