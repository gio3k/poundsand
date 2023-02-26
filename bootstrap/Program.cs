using System.Reflection;
using Bootstrap;

Sandbox.Internal.GlobalSystemNamespace.Log.Info(
	"pounds& bootstrap loaded! (https://github.com/lotuspar/poundsand)" );
Sandbox.Internal.GlobalSystemNamespace.Log.Info( $"- cwd {Directory.GetCurrentDirectory()}" );
Sandbox.Internal.GlobalSystemNamespace.Log.Info( $"- current {Assembly.GetExecutingAssembly()}" );

LoadAssemblies();

static bool ShouldSkipAssembly( string path )
{
	if ( Assembly.GetExecutingAssembly().Location == Path.GetFullPath( path ) )
	{
		return true;
	}

	return false;
}

static Assembly ResolveAssemblyHandler( object? sender, ResolveEventArgs args )
{
	// Check if we already have the assembly loaded
	var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault( v => v.FullName == args.Name );
	if ( assembly != null ) return assembly;

	// Try to find the assembly by short name / file name
	var shortName = args.Name.Split( ',' )[0];
	var path = Path.Combine( Static.GetFullAssemblyDirectoryPath(), $"{shortName}.dll" );

	if ( ShouldSkipAssembly( path ) ) return null;

	// Check for assembly
	if ( !Path.Exists( path ) ) return null;

	// Load assembly
	try
	{
		return Assembly.LoadFrom( path );
	}
	catch ( Exception e )
	{
		Sandbox.Internal.GlobalSystemNamespace.Log.Info( $"error while loading dependency {path}" );
		Sandbox.Internal.GlobalSystemNamespace.Log.Error( e );
		return null;
	}
}

static void LoadAssemblies()
{
	AppDomain.CurrentDomain.AssemblyResolve += ResolveAssemblyHandler;

	foreach ( var file in Directory.GetFiles( Static.GetFullAssemblyDirectoryPath(), "*.dll" ) )
	{
		// Make sure we aren't loading the current assembly
		if ( ShouldSkipAssembly( file ) )
		{
			Sandbox.Internal.GlobalSystemNamespace.Log.Info( $"skipping assembly {file}" );
			continue;
		}

		Sandbox.Internal.GlobalSystemNamespace.Log.Info( $"loading assembly {file}" );
		try
		{
			AppDomain.CurrentDomain.ExecuteAssembly( file );
		}
		catch ( MissingMethodException )
		{
			// ignored
		}
		catch ( Exception e )
		{
			Sandbox.Internal.GlobalSystemNamespace.Log.Info( $"error while loading assembly {file}" );
			Sandbox.Internal.GlobalSystemNamespace.Log.Error( e );
		}
	}
}
