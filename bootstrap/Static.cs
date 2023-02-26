namespace Bootstrap;

public static class Static
{
	public const string AssemblyDirectoryPath = "poundsand\\";

	public static string GetFullAssemblyDirectoryPath() =>
		Path.Combine( Directory.GetCurrentDirectory(), AssemblyDirectoryPath );
}
