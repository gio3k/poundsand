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

static void LoadAssemblies()
{
	foreach ( var file in Directory.GetFiles(
		         Path.Combine( Directory.GetCurrentDirectory(), Static.AssemblyDirectoryPath ), "*.dll" ) )
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
		catch ( Exception e )
		{
			Sandbox.Internal.GlobalSystemNamespace.Log.Info( $"error while loading assembly {file}" );
			Sandbox.Internal.GlobalSystemNamespace.Log.Error( e );
		}
	}
}
