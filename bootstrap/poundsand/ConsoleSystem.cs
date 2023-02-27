using System.Reflection;
using Poundsand.Internal;

namespace Poundsand;

public class ConsoleSystem : ContextSeparatedObject<ConsoleSystem>, IAssemblyTypeObject
{
	public static string AssemblyName => "Sandbox.Game";
	public static string TypeName => "Sandbox.ConsoleSystem";

	private object? _collectionInstance;

	public override void Initialize()
	{
		base.Initialize();

		_collectionInstance = Type.GetField( "Collection", BindingFlags.NonPublic | BindingFlags.Static )
			?.GetValue( null );
	}

	public void AddAssemblyToCollection( Assembly assembly ) =>
		_collectionInstance.GetType().GetMethod( "AddAssembly", BindingFlags.NonPublic | BindingFlags.Instance,
				new[] { typeof(Assembly) } )
			.Invoke( _collectionInstance, new object?[] { assembly } );
}
