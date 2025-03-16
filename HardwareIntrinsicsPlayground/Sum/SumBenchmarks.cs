using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;

namespace HardwareIntrinsicsPlayground.Sum;

[MemoryDiagnoser]
public class SumBenchmarks
{
    private int[] _dataToSum = null!;

    [Params(100, 1_000, 10_000)] public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _dataToSum = Enumerable.Range(0, Size).ToArray();
    }

    [Benchmark]
    public int SumForeach()
    {
        var result = 0;

        foreach (var i in _dataToSum) result += i;

        return result;
    }

    [Benchmark]
    public int SumLinq()
    {
        return _dataToSum.Sum();
    }

    [Benchmark]
    public int SumUnrolledSafe()
    {
        var result = 0;

        var i = 0;
        var lastBlockIndex = _dataToSum.Length - _dataToSum.Length % 4;

        while (i < lastBlockIndex)
        {
            result += _dataToSum[i + 0];
            result += _dataToSum[i + 1];
            result += _dataToSum[i + 2];
            result += _dataToSum[i + 3];

            i += 4;
        }

        while (i < _dataToSum.Length)
        {
            result += _dataToSum[i];
            i += 1;
        }

        return result;
    }

    [Benchmark]
    public unsafe int SumUnrolledUnsafe()
    {
        var result = 0;

        var i = 0;
        var lastBlockIndex = _dataToSum.Length - _dataToSum.Length % 4;

        // Pin source so we can elide the bounds checks
        fixed (int* pSource = _dataToSum)
        {
            while (i < lastBlockIndex)
            {
                result += pSource[i + 0];
                result += pSource[i + 1];
                result += pSource[i + 2];
                result += pSource[i + 3];

                i += 4;
            }

            while (i < _dataToSum.Length)
            {
                result += pSource[i];
                i += 1;
            }
        }

        return result;
    }

    [Benchmark]
    public int SumVectorT()
    {
        var source = _dataToSum.AsSpan();
        var result = 0;
        var vresult = Vector<int>.Zero;

        var i = 0;
        var lastBlockIndex = source.Length - source.Length % Vector<int>.Count;

        while (i < lastBlockIndex)
        {
            vresult += new Vector<int>(source[i..]);
            i += Vector<int>.Count;
        }

        for (var n = 0; n < Vector<int>.Count; n++) result += vresult[n];

        while (i < source.Length)
        {
            result += source[i];
            i += 1;
        }

        return result;
    }

    [Benchmark]
    public unsafe int SumVectorizedSse2()
    {
        if (!Sse2.IsSupported) throw new NotSupportedException("Sse2 is not supported on this platform.");

        int result;
        var source = _dataToSum.AsSpan();

        fixed (int* pSource = source)
        {
            var vresult = Vector128<int>.Zero;

            var i = 0;
            var lastBlockIndex = source.Length - source.Length % 4;

            while (i < lastBlockIndex)
            {
                vresult = Sse2.Add(vresult, Sse2.LoadVector128(pSource + i));
                i += 4;
            }

            if (Ssse3.IsSupported)
            {
                vresult = Ssse3.HorizontalAdd(vresult, vresult);
                vresult = Ssse3.HorizontalAdd(vresult, vresult);
            }
            else
            {
                vresult = Sse2.Add(vresult, Sse2.Shuffle(vresult, 0x4E));
                vresult = Sse2.Add(vresult, Sse2.Shuffle(vresult, 0xB1));
            }

            result = vresult.ToScalar();

            while (i < source.Length)
            {
                result += pSource[i];
                i += 1;
            }
        }

        return result;
    }

    [Benchmark]
    public unsafe int SumVectorizedArmAdv()
    {
        if (!AdvSimd.Arm64.IsSupported)
            throw new NotSupportedException("ARMx64 AdvSIMD is not supported on this platform.");

        int result;
        var source = _dataToSum.AsSpan();

        fixed (int* pSource = source)
        {
            var vresult = Vector128<int>.Zero;

            var i = 0;
            var lastBlockIndex = source.Length - source.Length % 4;

            while (i < lastBlockIndex)
            {
                vresult = AdvSimd.Add(vresult, AdvSimd.LoadVector128(pSource + i));
                i += 4;
            }

            vresult = AdvSimd.Arm64.AddPairwise(vresult, vresult);
            vresult = AdvSimd.Arm64.AddPairwise(vresult, vresult);
            
            result = vresult.ToScalar();

            while (i < source.Length)
            {
                result += pSource[i];
                i += 1;
            }
        }

        return result;
    }
}