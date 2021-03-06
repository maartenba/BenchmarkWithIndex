# BenchmarkWithIndex

Can we optimize https://twitter.com/buhakmeh/status/1291029712458911752 ? That is the question!

<blockquote class="twitter-tweet"><p lang="en" dir="ltr">Looking at <a href="https://twitter.com/hashtag/Kotlin?src=hash&amp;ref_src=twsrc%5Etfw">#Kotlin</a> and I saw the “withIndex()&quot; method.<br><br>Hey! Why isn&#39;t that in <a href="https://twitter.com/hashtag/csharp?src=hash&amp;ref_src=twsrc%5Etfw">#csharp</a> ?<br><br>Well, I made it work! What do you <a href="https://twitter.com/dotnet?ref_src=twsrc%5Etfw">@dotnet</a> folks think? <a href="https://t.co/ya7R7LQ0hf">pic.twitter.com/ya7R7LQ0hf</a></p>&mdash; Khalid (@buhakmeh) <a href="https://twitter.com/buhakmeh/status/1291029712458911752?ref_src=twsrc%5Etfw">August 5, 2020</a></blockquote>

Rules:
* Create a PR that adds your implementation/benchmark.
* Add documentation comments to your `WithIndex` method, explaining why it may (or may not) be faster/better in terms of memory usage.
* The `WithIndex` method *must* use `this IEnumerable<T> enumerable` as a parameter (so no optimization by using an array, for example - the `MicrosoftHighPerf` being the exception here).

## Current results

```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.388 (2004/?/20H1)
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.302
  [Host]     : .NET Core 3.1.6 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.31603), X64 RyuJIT
  DefaultJob : .NET Core 3.1.6 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.31603), X64 RyuJIT


|                      Method |       Mean |    Error |   StdDev | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------------- |-----------:|---------:|---------:|------:|--------:|-------:|------:|------:|----------:|
|                    Original | 1,215.7 ns | 28.18 ns | 80.41 ns |  1.00 |    0.00 | 0.4845 |     - |     - |    1520 B |
|           WithIndexAsStruct |   902.0 ns | 16.23 ns | 24.78 ns |  0.76 |    0.04 | 0.0381 |     - |     - |     120 B |
|              WithValueTuple |   944.6 ns | 18.50 ns | 30.39 ns |  0.80 |    0.05 | 0.0381 |     - |     - |     120 B |
|    WithValueTupleAndForEach |   882.9 ns | 18.06 ns | 52.40 ns |  0.73 |    0.06 | 0.0324 |     - |     - |     104 B |
| WithValueTupleAndEnumerator |   887.6 ns | 17.38 ns | 32.22 ns |  0.75 |    0.05 | 0.0324 |     - |     - |     104 B |
|   WithCustomArrayEnumerator |   477.4 ns |  9.38 ns |  9.63 ns |  0.41 |    0.02 | 0.0176 |     - |     - |      56 B |
|           MicrosoftHighPerf |   420.0 ns |  5.96 ns |  5.28 ns |  0.36 |    0.01 |      - |     - |     - |         - |
```

## Different versions

### `Original`

The original implementation from https://twitter.com/buhakmeh/status/1291029712458911752.

### `WithIndexAsStruct`

As suggested in https://twitter.com/maartenballiauw/status/1291044097235484674,
the `WithIndex{T}` class has been made a struct so it is not allocated
on the managed heap (and does not incur GC).

### `WithValueTuple`

Improves `WithIndexAsStruct` by returning a `ValueTuple` directly 
(instead of `WithIndex{T}`). Cut out the middle person!
        
### `WithValueTupleAndForEach`

Version of `WithValueTuple`, where instead of using a `.Select()`,
a foreach loop is used instead. Assumption is there are fewer
allocations happening.
        
### `WithValueTupleAndEnumerator`

Version of `WithValueTuple`, where instead of using compiler and
runtime magic, we'll use the enumerator directly.

### `WithCustomArrayEnumerator`

This version returns an enumerable of ValueTuple structs.
However, when the original is an array, we return a custom, lightweight `IndexedArrayEnumerable{T}`.
This bypasses the compiler-generated `IEnumerable` that is slightly heavier.
        
### `MicrosoftHighPerf`

Based on [a tweet by @SergioPedri](https://twitter.com/SergioPedri/status/1291330327881879552), uses the [`Microsoft.Toolkit.HighPerformance` package](https://www.nuget.org/packages/Microsoft.Toolkit.HighPerformance/).
