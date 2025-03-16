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


| Method    | Mean        | Error    | StdDev    | Median      | Allocated |
|---------- |------------:|---------:|----------:|------------:|----------:|
| Crc32     |    40.61 us | 0.028 us |  0.026 us |    40.61 us |      72 B |
| Crc64     |    36.15 us | 1.316 us |  3.881 us |    34.12 us |      72 B |
| Md5       | 1,904.35 us | 1.417 us |  1.183 us | 1,904.64 us |     113 B |
| Sha1      | 1,403.53 us | 1.102 us |  0.920 us | 1,403.53 us |     128 B |
| Sha256    |   366.74 us | 0.558 us |  0.522 us |   366.52 us |     168 B |
| Sha512    | 1,884.12 us | 3.705 us |  3.094 us | 1,883.20 us |     289 B |
| XxHash3   |    30.74 us | 0.033 us |  0.031 us |    30.73 us |      80 B |
| XxHash32  |   115.59 us | 0.074 us |  0.061 us |   115.61 us |      72 B |
| XxHash64  |   138.09 us | 4.395 us | 12.889 us |   134.82 us |      80 B |
| XxHash128 |    38.42 us | 0.297 us |  0.264 us |    38.49 us |     112 B |