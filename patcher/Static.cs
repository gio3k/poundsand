namespace Patcher;

public static class Static
{
	/// <summary>
	/// Path to bootstrap assembly (relative to game root dir)
	/// </summary>
	public const string BootstrapAssemblyPath = "poundsand\\bootstrap.dll";

	/// <summary>
	/// Whether or not info text should be printed
	/// </summary>
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
