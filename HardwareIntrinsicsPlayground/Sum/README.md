Benchmark of various methods that sum numbers from an array (Using CollectionMarshals we could use list too). Wanted to compare naive foreach, LINQ Sum() and see if I can beat them using the new hardware intrinsics API and SIMD.

Turns out LINQ Sum is pretty fast, but was able to beat it using SIMD vector arithmetics.

Here are results:
Device: Surface Pro 11
BenchmarkDotNet v0.14.0, Windows 11 (10.0.26120.3380)
Snapdragon(R) X 12-core X1E80100 @ 3.40 GHz, 3417 Mhz, 12 Core(s), 12 Logical Processor(s)
.NET SDK 9.0.200
[Host]     : .NET 9.0.2 (9.0.225.6610), Arm64 RyuJIT AdvSIMD
DefaultJob : .NET 9.0.2 (9.0.225.6610), Arm64 RyuJIT AdvSIMD


| Method              | Size  | Mean         | Error      | StdDev      | Median       | Allocated |
|-------------------- |------ |-------------:|-----------:|------------:|-------------:|----------:|
| SumForeach          | 100   |    35.028 ns |  0.2163 ns |   0.1689 ns |    34.987 ns |         - |
| SumLinq             | 100   |    11.604 ns |  0.3564 ns |   1.0509 ns |    11.674 ns |         - |
| SumUnrolledSafe     | 100   |    25.017 ns |  0.0160 ns |   0.0142 ns |    25.020 ns |         - |
| SumUnrolledUnsafe   | 100   |    16.143 ns |  0.6169 ns |   1.8189 ns |    14.798 ns |         - |
| SumVectorT          | 100   |    15.514 ns |  0.7266 ns |   2.1425 ns |    16.405 ns |         - |
| SumVectorizedSse2   | 100   |           NA |         NA |          NA |           NA |        NA |
| SumVectorizedArmAdv | 100   |     6.977 ns |  0.3108 ns |   0.9165 ns |     6.622 ns |         - |
| SumForeach          | 1000  |   313.828 ns |  0.6958 ns |   1.1815 ns |   313.592 ns |         - |
| SumLinq             | 1000  |   180.025 ns |  0.2210 ns |   0.1959 ns |   180.035 ns |         - |
| SumUnrolledSafe     | 1000  |   266.949 ns |  9.4606 ns |  27.8947 ns |   248.129 ns |         - |
| SumUnrolledUnsafe   | 1000  |   240.019 ns |  0.3489 ns |   0.3263 ns |   240.041 ns |         - |
| SumVectorT          | 1000  |   160.335 ns |  5.7922 ns |  17.0785 ns |   175.100 ns |         - |
| SumVectorizedSse2   | 1000  |           NA |         NA |          NA |           NA |        NA |
| SumVectorizedArmAdv | 1000  |    87.257 ns |  0.1415 ns |   0.1323 ns |    87.208 ns |         - |
| SumForeach          | 10000 | 3,171.586 ns |  2.7531 ns |   2.2990 ns | 3,172.172 ns |         - |
| SumLinq             | 10000 | 1,798.816 ns |  0.9824 ns |   0.8203 ns | 1,799.032 ns |         - |
| SumUnrolledSafe     | 10000 | 2,539.590 ns |  4.6106 ns |   3.8501 ns | 2,539.126 ns |         - |
| SumUnrolledUnsafe   | 10000 | 2,520.971 ns |  2.2738 ns |   1.8987 ns | 2,521.076 ns |         - |
| SumVectorT          | 10000 | 1,534.463 ns | 54.8917 ns | 161.8495 ns | 1,676.922 ns |         - |
| SumVectorizedSse2   | 10000 |           NA |         NA |          NA |           NA |        NA |
| SumVectorizedArmAdv | 10000 | 1,209.328 ns |  0.9282 ns |   0.8683 ns | 1,209.470 ns |         - |