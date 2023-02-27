using System.Reflection;
using System.Runtime.Loader;
using Bootstrap;
using Poundsand;

public class Program
{
	public static Program Instance { get; private set; }
	public AssemblyLoadContext? Context { get; private set; }
	public static void Main( string[] args ) => Instance = new Program();

	private Program()
	{
		Log.Info(
			"pounds& bootstrap loaded! (https://github.com/lotuspar/poundsand)" );
		Log.Info( $"- cwd {Directory.GetCurrentDirectory()}" );
		Log.Info( $"- current {Assembly.GetExecutingAssembly()}" );

		GenerateContext();
		LoadAssemblies();

		// Add our ConCmds
		ConsoleSystem.Menu.AddAssemblyToCollection( Assembly.GetExecutingAssembly() );
	}

	[Sandbox.ConCmd.Menu( "poundsand_reload" )]
	private static void Reload()
	{
		Instance.GenerateContext();
		Instance.LoadAssemblies();
	}

	[Sandbox.ConCmd.Menu( "poundsand_unload" )]
	private static void Unload()
	{
		// Unload old load context
		Instance.Context?.Unload();
		Instance.Context = null;
	}

	[Sandbox.ConCmd.Menu( "poundsand_contexts" )]
	private static void Contexts()
	{
		foreach ( var assemblyLoadContext in AssemblyLoadContext.All )
		{
			Log.Info( $"Context {assemblyLoadContext}" );
			foreach ( var assembly in assemblyLoadContext.Assemblies ) Log.Info( $"  Asm {assembly}" );
		}
	}

	/// <summary>
	/// Generate (or regenerate) current AssemblyLoadContext
	/// </summary>
	private void GenerateContext()
	{
		// Unload old load context
		Context?.Unload();
		Context = null;

		// Create our load context
		Log.Info( "creating new AssemblyLoadContext..." );
		Context = new AssemblyLoadContext( "PsLoadContext", true );
		Context.Resolving += ResolveAssembly;
	}

	private static bool ShouldSkipAssembly( string path ) =>
		Assembly.GetExecutingAssembly().Location == Path.GetFullPath( path );

	private Assembly? ResolveAssembly( AssemblyLoadContext context, AssemblyName name )
	{
		// Make sure we haven't already loaded the assembly (in the AppDomain)
		{
			var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault( v => v.FullName == name.Name );
			if ( assembly != null ) return assembly;
		}

		// Try to find the assembly by short name / file name
		var shortName = name.Name.Split( ',' )[0];
		var path = Path.Combine( Static.GetFullAssemblyDirectoryPath(), $"{shortName}.dll" );

		// Check for assembly
		if ( !Path.Exists( path ) ) return null;

		// Load assembly
		try
		{
			return context.LoadFromAssemblyPath( path );
		}
		catch ( Exception e )
		{
			Log.Info( $"failed to load dependency {path}" );
			Log.Error( e );
			return null;
		}
	}

	private void LoadAssemblies()
	{
		Log.Info( "loading assemblies..." );

		foreach ( var file in Directory.GetFiles( Static.GetFullAssemblyDirectoryPath(), "*.dll" ) )
		{
			// Make sure we aren't loading the current assembly
			if ( ShouldSkipAssembly( file ) )
			{
				Log.Info( $"skipping assembly {file}" );
				continue;
			}

			Log.Info( $"loading assembly {file}" );
			try
			{
				var assembly = Context?.LoadFromAssemblyPath( file );
				if ( assembly == null )
				{
					Log.Info( $"failed to load assembly {file}" );
					continue;
				}

				if ( assembly.EntryPoint != null )
				{
					Log.Info( $"executing assembly entry point {file}" );
					assembly.EntryPoint.Invoke( null, new object?[] { Array.Empty<string>() } );
				}
			}
			catch ( MissingMethodException )
			{
				// ignored
			}
			catch ( Exception e )
			{
				Log.Info( $"failed to load / execute assembly {file}" );
				Log.Error( e );
			}
		}
	}
}
