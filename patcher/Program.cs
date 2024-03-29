﻿using CommandLine;

namespace Patcher;

public static class Program
{
	private static void PostParse( Options opts )
	{
		if ( opts.Verbose )
			Static.ShowInfo = true;

		Patchers.CodeAnalysisPatcher patcher = new() { Path = opts.Path };
		if ( !patcher.Patch() ) Static.Err( "Failed to patch assembly." );
	}

	private static void Main( string[] args )
	{
		Parser.Default.ParseArguments<Options>( args )
			.WithParsed( PostParse );
	}

	private class Options
	{
		[Option( 'v', "verbose", Default = false )]
		public bool Verbose { get; set; }

		[Option( 'p', "path", Default = false, Required = true )]
		public string Path { get; set; }
	}
}
