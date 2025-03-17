using System.Security.Cryptography;
using Microsoft.Extensions.ObjectPool;

namespace HardwareIntrinsicsPlayground.Hashing;

public class IncrementalHashPolicy : PooledObjectPolicy<IncrementalHash>
{
    public override IncrementalHash Create()
    {
        return IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
    }

    public override bool Return(IncrementalHash hashInstance)
    {
        Span<byte> throwaway = stackalloc byte[SHA256.HashSizeInBytes];
        hashInstance.GetHashAndReset(throwaway);
        return true;
    }
}