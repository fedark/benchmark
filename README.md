# About

Contains different .NET benchmarks utilizing BenchmarkDotNet library. There are two separate projects for .NET and .NET Framework 4.7.2.

Currently, the following tests are there:
- `ToLowerInLoopTest` - the difference between modifying a string outside a loop and inside on each iteration
- `FindByIdTest` - find element on each iteration, or preserve a lookup before the loop
