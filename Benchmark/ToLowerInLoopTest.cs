using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmark;

[MemoryDiagnoser]
public class ToLowerInLoopTest
{
	private string[] _strings =
	[
		"sofjsfjas[fj|",
		"sofjsfdasfasfasfasfjas[fj|",
		"sofjsfdsfasfdjas[fj|",
		"sofjsfjas[fj|",
		"sofjsfdsfjsdfdsas[fj|",
		"sofjsfjas[fj|",
		"sofjsdsfdsffjas[fj|"
	];

	private string _substr = "DB";

	[Benchmark]
	public bool ToLowerWithinLoop()
	{
		bool result = false;

		foreach (var s in _strings)
		{
			result = result && s.Contains(_substr.ToLowerInvariant());
		}

		return result;
	}

	[Benchmark]
	public bool ToLowerOutsideLoop()
	{
		bool result = false;

		var substr = _substr.ToLowerInvariant();

		foreach (var s in _strings)
		{
			result = result && s.Contains(substr);
		}

		return result;
	}

	public static void Run()
	{
		BenchmarkRunner.Run<ToLowerInLoopTest>();
	}
}