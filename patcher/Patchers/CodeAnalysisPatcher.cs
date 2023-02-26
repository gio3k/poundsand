using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Patcher.Patchers;

public class CodeAnalysisPatcher : IPatcher
{
	public string Path { get; }
	public string OutputPath { get; }

	public CodeAnalysisPatcher( string path )
	{
		Path = path;
		OutputPath = path;
	}

	public CodeAnalysisPatcher( string path, string outputPath )
	{
		Path = path;
		OutputPath = outputPath;
	}

	public bool Patch()
	{
		var assembly = AssemblyDefinition.ReadAssembly( Path,
			new ReaderParameters { ReadWrite = true, ReadingMode = ReadingMode.Immediate, InMemory = true } );
		if ( assembly == null )
		{
			Static.Err( $"Couldn't read assembly @ {Path}" );
			return false;
		}

		// Create our static constructor
		var cctor = new MethodDefinition( ".cctor",
			MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.SpecialName |
			MethodAttributes.RTSpecialName, assembly.MainModule.TypeSystem.Void );

		// Get the type we want to patch (SyntaxNode)
		var type = assembly.MainModule.GetType( "Microsoft.CodeAnalysis.SyntaxNode" );
		if ( type == null )
		{
			Static.Err( $"Couldn't find type Microsoft.CodeAnalysis.SyntaxNode" );
			return false;
		}

		// Add our static constructor to the type
		type.Methods.Add( cctor );

		// Make sure the type doesn't have BeforeFieldInit set
		type.Attributes &= ~TypeAttributes.BeforeFieldInit;

		// Create IL for the constructor
		var processor = cctor.Body.GetILProcessor();
		var patch = Util.CreateLoadInstructions( ref processor, assembly.MainModule );

		foreach ( var instruction in patch ) processor.Append( instruction );
		processor.Append( processor.Create( OpCodes.Ret ) );

		// Write new assembly
		assembly.Write( OutputPath );

		return true;
	}
}
