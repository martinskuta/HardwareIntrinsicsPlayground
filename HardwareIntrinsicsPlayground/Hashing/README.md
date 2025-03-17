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


| Method            | Mean        | Error    | StdDev   | Allocated |
|------------------ |------------:|---------:|---------:|----------:|
| Crc32             |    33.02 us | 0.635 us | 0.756 us |      72 B |
| Crc64             |    32.52 us | 0.048 us | 0.045 us |      72 B |
| Md5               | 1,909.38 us | 2.076 us | 1.734 us |     113 B |
| Sha1              | 1,408.64 us | 1.339 us | 1.187 us |     129 B |
| Sha256            |   367.64 us | 0.355 us | 0.296 us |     168 B |
| Sha256Incremental |   367.83 us | 0.325 us | 0.304 us |     304 B |
| Sha512            | 1,888.41 us | 1.592 us | 1.329 us |     288 B |
| XxHash3           |    30.82 us | 0.028 us | 0.024 us |      80 B |
| XxHash32          |   115.86 us | 0.124 us | 0.116 us |      72 B |
| XxHash64          |   107.84 us | 0.095 us | 0.084 us |      80 B |
| XxHash128         |    30.83 us | 0.039 us | 0.034 us |     112 B |
