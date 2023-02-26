namespace Patcher;

public interface IPatcher
{
	/// <summary>
	/// Patch assembly
	/// </summary>
	/// <returns>True for success</returns>
	public bool Patch();
}
