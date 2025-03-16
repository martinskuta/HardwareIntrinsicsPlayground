Just wanted to compare some built in hashing algorithms in .net for speed. Mainly looking at speed. Main intention is to
choose a hashing algo for comparing files for differences. Separate analysis on collisions is needed, this is pure speed
comparison of implementations that are provided by .NET network. CRC and xxHash family come from System.IO.Hashing which
is a separate package provided from Microsoft.

Here are results:
Device: Surface Pro 11
BenchmarkDotNet v0.14.0, Windows 11 (10.0.26120.3380)
Snapdragon(R) X 12-core X1E80100 @ 3.40 GHz, 3417 Mhz, 12 Core(s), 12 Logical Processor(s)
.NET SDK 9.0.200
[Host]     : .NET 9.0.2 (9.0.225.6610), Arm64 RyuJIT AdvSIMD
DefaultJob : .NET 9.0.2 (9.0.225.6610), Arm64 RyuJIT AdvSIMD


| Method    | Mean        | Error     | StdDev    | Median      | Allocated |
|---------- |------------:|----------:|----------:|------------:|----------:|
| Crc32     |    40.62 us |  0.033 us |  0.028 us |    40.61 us |      72 B |
| Crc64     |    39.85 us |  0.792 us |  1.820 us |    40.56 us |      72 B |
| Md5       | 2,244.08 us | 36.833 us | 40.940 us | 2,230.24 us |     112 B |
| Sha1      | 1,645.64 us |  1.537 us |  1.284 us | 1,645.68 us |     129 B |
| Sha256    |   429.37 us |  0.479 us |  0.470 us |   429.24 us |     168 B |
| Sha512    | 2,352.35 us |  7.737 us |  7.598 us | 2,350.64 us |     288 B |
| XxHash3   |    37.77 us |  0.782 us |  1.450 us |    38.42 us |      80 B |
| XxHash32  |   124.43 us |  4.029 us | 11.879 us |   115.75 us |      72 B |
| XxHash64  |   108.59 us |  2.152 us |  3.155 us |   107.63 us |      80 B |
| XxHash128 |    38.34 us |  0.696 us |  0.651 us |    38.50 us |     112 B |