Benchmark of various methods that sum numbers from an array (Using CollectionMarshals we could use list too). Wanted to compare naive foreach, LINQ Sum() and see if I can beat them using the new hardware intrinsics API and SIMD.

Turns out LINQ Sum is pretty fast, but was able to beat it using SIMD vector arithmetics.

Here are results:
Device: Surface Pro 11
BenchmarkDotNet v0.14.0, Windows 11 (10.0.26120.3380)
Snapdragon(R) X 12-core X1E80100 @ 3.40 GHz, 3417 Mhz, 12 Core(s), 12 Logical Processor(s)
.NET SDK 9.0.200
[Host]     : .NET 9.0.2 (9.0.225.6610), Arm64 RyuJIT AdvSIMD
DefaultJob : .NET 9.0.2 (9.0.225.6610), Arm64 RyuJIT AdvSIMD


| Method              | Size    | Mean           | Error          | StdDev         | Median         | Allocated |
|-------------------- |-------- |---------------:|---------------:|---------------:|---------------:|----------:|
| SumForeach          | 100     |      35.274 ns |      0.3993 ns |      0.3334 ns |      35.126 ns |         - |
| SumLinq             | 100     |       9.801 ns |      0.0802 ns |      0.0750 ns |       9.756 ns |         - |
| SumUnrolledSafe     | 100     |      19.767 ns |      0.0342 ns |      0.0285 ns |      19.762 ns |         - |
| SumUnrolledUnsafe   | 100     |      15.140 ns |      0.0174 ns |      0.0136 ns |      15.140 ns |         - |
| SumVectorT          | 100     |      14.495 ns |      0.5826 ns |      1.7179 ns |      14.364 ns |         - |
| SumVectorizedSse2   | 100     |             NA |             NA |             NA |             NA |        NA |
| SumVectorizedArmAdv | 100     |       6.418 ns |      0.3028 ns |      0.8927 ns |       5.703 ns |         - |
| SumForeach          | 1000    |     315.359 ns |      5.7052 ns |      6.7917 ns |     313.285 ns |         - |
| SumLinq             | 1000    |     180.479 ns |      0.2160 ns |      0.2020 ns |     180.445 ns |         - |
| SumUnrolledSafe     | 1000    |     249.724 ns |      4.8948 ns |      6.0112 ns |     247.922 ns |         - |
| SumUnrolledUnsafe   | 1000    |     240.353 ns |      0.1830 ns |      0.1528 ns |     240.366 ns |         - |
| SumVectorT          | 1000    |     158.574 ns |      5.7668 ns |     17.0034 ns |     168.475 ns |         - |
| SumVectorizedSse2   | 1000    |             NA |             NA |             NA |             NA |        NA |
| SumVectorizedArmAdv | 1000    |      87.661 ns |      0.1135 ns |      0.1006 ns |      87.654 ns |         - |
| SumForeach          | 1000000 | 319,059.966 ns |  5,029.0248 ns |  5,589.7478 ns | 317,750.635 ns |         - |
| SumLinq             | 1000000 |             NA |             NA |             NA |             NA |        NA |
| SumUnrolledSafe     | 1000000 | 283,375.990 ns | 10,637.7592 ns | 31,365.6753 ns | 258,720.923 ns |         - |
| SumUnrolledUnsafe   | 1000000 | 253,784.609 ns |    395.6693 ns |    370.1093 ns | 253,760.791 ns |         - |
| SumVectorT          | 1000000 | 167,883.509 ns |    433.9706 ns |    405.9364 ns | 167,810.913 ns |         - |
| SumVectorizedSse2   | 1000000 |             NA |             NA |             NA |             NA |        NA |
| SumVectorizedArmAdv | 1000000 | 125,675.368 ns |    288.5930 ns |    255.8302 ns | 125,639.233 ns |         - |