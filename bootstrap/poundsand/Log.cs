namespace Poundsand;

public static class Log
{
	public static void Info( FormattableString txt ) => Sandbox.Internal.GlobalSystemNamespace.Log.Info( txt );
	public static void Info( object txt ) => Sandbox.Internal.GlobalSystemNamespace.Log.Info( txt );

	public static void Warning( FormattableString txt ) => Sandbox.Internal.GlobalSystemNamespace.Log.Warning( txt );
	public static void Warning( object txt ) => Sandbox.Internal.GlobalSystemNamespace.Log.Warning( txt );

	public static void Error( FormattableString txt ) => Sandbox.Internal.GlobalSystemNamespace.Log.Error( txt );
	public static void Error( object txt ) => Sandbox.Internal.GlobalSystemNamespace.Log.Error( txt );
}
