using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Patcher;

public interface IPatcher
{
	/// <summary>
	/// Patch assembly
	/// </summary>
	/// <returns>True for success</returns>
	public bool Patch();

	/// <summary>
	/// Path to assembly
	/// </summary>
	public string Path { get; set; }

	/// <summary>
	/// Path to write new assembly at - can be the same as input path
	/// </summary>
	public string OutputPath { get; set; }

	public static IEnumerable<Instruction> CreateLoadInstructions( ref ILProcessor processor, ModuleDefinition module )
	{
		var currentDomainPropertyMethod =
			module.ImportReference( typeof(AppDomain).GetProperty( "CurrentDomain" )!.GetMethod );
		var executeAssemblyMethod =
			module.ImportReference( typeof(AppDomain).GetMethod( "ExecuteAssembly", new[] { typeof(string) } ) );

		var result = new List<Instruction>
		{
			processor.Create( OpCodes.Nop ),
			processor.Create( OpCodes.Call, currentDomainPropertyMethod ),
			processor.Create( OpCodes.Ldstr, Static.BootstrapAssemblyPath ),
			processor.Create( OpCodes.Call, executeAssemblyMethod ),
			processor.Create( OpCodes.Pop )
		};

		return result;
	}
}
