using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;

namespace HardwareIntrinsicsPlayground.Hashing;

[MemoryDiagnoser]
public class HashingBenchmark
{
    private byte[] _data = null!;

    [GlobalSetup]
    public void Setup()
    {
        _data = new byte[1024 * 1024];
        new Random().NextBytes(_data);
    }

    [Benchmark]
    public string Crc32()
    {
        return Convert.ToBase64String(System.IO.Hashing.Crc32.Hash(_data));
    }
    
    [Benchmark]
    public string Crc64()
    {
        return Convert.ToBase64String(System.IO.Hashing.Crc32.Hash(_data));
    }

    [Benchmark]
    public string Md5()
    {
        return Convert.ToBase64String(MD5.HashData(_data));
    }
    
    [Benchmark]
    public string Sha1()
    {
        return Convert.ToBase64String(SHA1.HashData(_data));
    }
    
    [Benchmark]
    public string Sha256()
    {
        return Convert.ToBase64String(SHA256.HashData(_data));
    }
    
    [Benchmark]
    public string Sha512()
    {
        return Convert.ToBase64String(SHA512.HashData(_data));
    }
    
    [Benchmark]
    public string XxHash3()
    {
        return Convert.ToBase64String(System.IO.Hashing.XxHash3.Hash(_data));
    }
    
    [Benchmark]
    public string XxHash32()
    {
        return Convert.ToBase64String(System.IO.Hashing.XxHash32.Hash(_data));
    }

    [Benchmark]
    public string XxHash64()
    {
        return Convert.ToBase64String(System.IO.Hashing.XxHash64.Hash(_data));
    }

    [Benchmark]
    public string XxHash128()
    {
        return Convert.ToBase64String(System.IO.Hashing.XxHash128.Hash(_data));
    }
}