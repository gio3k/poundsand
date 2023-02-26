using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Patcher.Patchers;

public static class Util
{
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
