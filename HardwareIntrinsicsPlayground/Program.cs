// See https://aka.ms/new-console-template for more information


using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Running;
using HardwareIntrinsicsPlayground;
using HardwareIntrinsicsPlayground.Hashing;
using HardwareIntrinsicsPlayground.Sum;

SanityCheck();

BenchmarkRunner.Run<SumBenchmarks>();
BenchmarkRunner.Run<HashingBenchmark>();
return;

static void SanityCheck()
{
    //Sanity check
    var sumBenchmarks = new SumBenchmarks { Size = 10_000 };
    sumBenchmarks.Setup();

    var expected = sumBenchmarks.SumLinq();
    if (expected != sumBenchmarks.SumForeach())
        throw new Exception("Go fix the code");
    if (expected != sumBenchmarks.SumUnrolledSafe())
        throw new Exception("Go fix the code");
    if (expected != sumBenchmarks.SumUnrolledUnsafe())
        throw new Exception("Go fix the code");
    if (expected != sumBenchmarks.SumVectorT())
        throw new Exception("Go fix the code");
    if (Sse2.IsSupported)
        if (expected != sumBenchmarks.SumVectorizedSse2())
            throw new Exception("Go fix the code");
    if (AdvSimd.IsSupported)
        if (expected != sumBenchmarks.SumVectorizedArmAdv())
            throw new Exception("Go fix the code");
}