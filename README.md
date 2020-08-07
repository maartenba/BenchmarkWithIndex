# BenchmarkWithIndex

Can we optimize https://twitter.com/buhakmeh/status/1291029712458911752 ? That is the question!

<blockquote class="twitter-tweet"><p lang="en" dir="ltr">Looking at <a href="https://twitter.com/hashtag/Kotlin?src=hash&amp;ref_src=twsrc%5Etfw">#Kotlin</a> and I saw the “withIndex()&quot; method.<br><br>Hey! Why isn&#39;t that in <a href="https://twitter.com/hashtag/csharp?src=hash&amp;ref_src=twsrc%5Etfw">#csharp</a> ?<br><br>Well, I made it work! What do you <a href="https://twitter.com/dotnet?ref_src=twsrc%5Etfw">@dotnet</a> folks think? <a href="https://t.co/ya7R7LQ0hf">pic.twitter.com/ya7R7LQ0hf</a></p>&mdash; Khalid (@buhakmeh) <a href="https://twitter.com/buhakmeh/status/1291029712458911752?ref_src=twsrc%5Etfw">August 5, 2020</a></blockquote> <script async src="https://platform.twitter.com/widgets.js" charset="utf-8"></script>

Rules:
* Create a PR that adds your implementation/benchmark.
* Add documentation comments to your `WithIndex` method, explaining why it may (or may not) be faster.
* The `WithIndex` method *must* use `this IEnumerable<T> enumerable` as a parameter (so no optimization by using an array, for example).

## Current results

```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.388 (2004/?/20H1)
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.302
  [Host]     : .NET Core 3.1.6 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.31603), X64 RyuJIT
  DefaultJob : .NET Core 3.1.6 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.31603), X64 RyuJIT
```

|                      Method |     Mean |     Error |    StdDev | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|--------:|------:|------:|------:|----------:|
|                    Original | 4.715 ms | 0.1782 ms | 0.5083 ms |  1.00 |    0.00 |     - |     - |     - |   2.77 KB |
|           WithIndexAsStruct | 4.725 ms | 0.1113 ms | 0.3211 ms |  1.01 |    0.12 |     - |     - |     - |   2.09 KB |
|              WithValueTuple | 4.923 ms | 0.1476 ms | 0.4353 ms |  1.06 |    0.17 |     - |     - |     - |   2.09 KB |
|    WithValueTupleAndForLoop | 5.136 ms | 0.1835 ms | 0.5323 ms |  1.10 |    0.16 |     - |     - |     - |   2.07 KB |
| WithValueTupleAndEnumerator | 5.255 ms | 0.1612 ms | 0.4626 ms |  1.13 |    0.15 |     - |     - |     - |   2.07 KB |
|           MicrosoftHighPerf | 5.133 ms | 0.1421 ms | 0.4146 ms |  1.10 |    0.14 |     - |     - |     - |   1.97 KB |

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
        
### `WithValueTupleAndForEach`

Version of `WithValueTuple`, where instead of using compiler and
runtime magic, we'll use the enumerator directly.
        
### `MicrosoftHighPerf`

Based on [a tweet by @SergioPedri](https://twitter.com/SergioPedri/status/1291330327881879552), uses the [`Microsoft.Toolkit.HighPerformance` package](https://www.nuget.org/packages/Microsoft.Toolkit.HighPerformance/).