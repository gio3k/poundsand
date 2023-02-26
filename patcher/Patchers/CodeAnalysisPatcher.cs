using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Patcher.Patchers;

public class CodeAnalysisPatcher : IPatcher
{
	/// <inheritdoc cref="IPatcher.Patch"/>
	public bool Patch()
	{
		if ( Path == null )
		{
			Static.Err( "Path was unset - patch halted!" );
			return false;
		}

		if ( OutputPath == null )
		{
			Static.Err( "OutputPath was unset - patch halted!" );
			return false;
		}

		Static.Info( $"Reading assembly @ {Path} ..." );
		var assembly = AssemblyDefinition.ReadAssembly( Path,
			new ReaderParameters { ReadWrite = true, ReadingMode = ReadingMode.Immediate, InMemory = true } );
		if ( assembly == null )
		{
			Static.Err( $"Couldn't read assembly @ {Path}" );
			return false;
		}

		// Create our static constructor
		Static.Info( "Generating static constructor definition" );
		var cctor = new MethodDefinition( ".cctor",
			MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.SpecialName |
			MethodAttributes.RTSpecialName, assembly.MainModule.TypeSystem.Void );

		// Get the type we want to patch (SyntaxNode)
		Static.Info( "Looking for Microsoft.CodeAnalysis.SyntaxNode..." );
		var type = assembly.MainModule.GetType( "Microsoft.CodeAnalysis.SyntaxNode" );
		if ( type == null )
		{
			Static.Err( "Couldn't find type Microsoft.CodeAnalysis.SyntaxNode" );
			return false;
		}

		// Add our static constructor to the type
		Static.Info( "Adding static constructor to SyntaxNode..." );
		type.Methods.Add( cctor );

		// Make sure the type doesn't have BeforeFieldInit set
		Static.Info( "Making sure BeforeFieldInit isn't set" );
		type.Attributes &= ~TypeAttributes.BeforeFieldInit;

		// Create IL for the constructor
		Static.Info( "Creating IL for the static constructor" );
		var processor = cctor.Body.GetILProcessor();
		var patch = IPatcher.CreateLoadInstructions( ref processor, assembly.MainModule );

		Static.Info( "Appending generated IL..." );
		foreach ( var instruction in patch ) processor.Append( instruction );
		processor.Append( processor.Create( OpCodes.Ret ) );

		// Write new assembly
		Static.Info( $"Writing assembly @ {OutputPath} ..." );
		assembly.Write( OutputPath );
		return true;
	}

	/// <inheritdoc cref="IPatcher.Path"/>
	public string Path { get; set; }

	/// <inheritdoc cref="IPatcher.OutputPath"/>
	public string OutputPath { get; set; }
}
