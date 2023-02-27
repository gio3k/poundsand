using System.Reflection;
using Poundsand.Internal;
using Sandbox;

namespace Poundsand;

public class Game : ContextSeparatedObject<Game>, IAssemblyTypeObject
{
	public static string AssemblyName => "Sandbox.Game";
	public static string TypeName => "Sandbox.Game";

	public bool InGame =>
		(bool)Type.GetProperty( "InGame", BindingFlags.Public | BindingFlags.Static ).GetValue( null );

	public ulong ServerSteamId =>
		(ulong)Type.GetProperty( "ServerSteamId", BindingFlags.Public | BindingFlags.Static ).GetValue( null );

	public ServerInformation ServerInfo =>
		(ServerInformation)Type.GetProperty( "Server", BindingFlags.Public | BindingFlags.Static ).GetValue( null );

	public IReadOnlyCollection<IClient> Clients =>
		(IReadOnlyCollection<IClient>)Type.GetProperty( "Clients", BindingFlags.Public | BindingFlags.Static )
			.GetValue( null );
}
