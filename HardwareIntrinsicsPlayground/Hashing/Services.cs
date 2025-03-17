using System.Security.Cryptography;
using Microsoft.Extensions.ObjectPool;

namespace HardwareIntrinsicsPlayground.Hashing;

public static class Services
{
    private static readonly ObjectPool<IncrementalHash> IncrementalHashPool =
        new DefaultObjectPool<IncrementalHash>(new IncrementalHashPolicy());
        
    public static byte[] CalculateIncrementalSha256Hash(byte[] data)
    {
        var hasher = IncrementalHashPool.Get();
        hasher.AppendData(data);
        return hasher.GetHashAndReset();
    }
}