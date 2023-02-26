namespace Patcher;

public static class Static
{
	public const string BootstrapAssemblyPath = "poundsand\\Bootstrap.dll";

	public static bool ShowInfo = false;

	public static void Info( FormattableString txt )
	{
		if ( !ShowInfo ) return;
		Console.WriteLine( $"[INFO] {txt}" );
	}

	public static void Warn( FormattableString txt ) => Console.WriteLine( $"[WARN] {txt}" );
	public static void Err( FormattableString txt ) => Console.WriteLine( $"[ERR] {txt}" );

	public static void Info( string txt )
	{
		if ( !ShowInfo ) return;
		Console.WriteLine( $"[INFO] {txt}" );
	}

	public static void Warn( string txt ) => Console.WriteLine( $"[WARN] {txt}" );
	public static void Err( string txt ) => Console.WriteLine( $"[ERR] {txt}" );
}
