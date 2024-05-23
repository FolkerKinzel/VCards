```

BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4412/22H2/2022Update)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.300
  [Host]             : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET 6.0           : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2
  .NET 8.0           : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET Framework 4.8 : .NET Framework 4.8.1 (4.8.9241.0), X64 RyuJIT VectorSize=256


```
| Method       | Job                | Runtime            | Mean       | Error     | StdDev    | Median     | Gen0     | Gen1     | Gen2     | Allocated  |
|------------- |------------------- |------------------- |-----------:|----------:|----------:|-----------:|---------:|---------:|---------:|-----------:|
| Write21      | .NET 6.0           | .NET 6.0           |         NA |        NA |        NA |         NA |       NA |       NA |       NA |         NA |
| WritePhoto21 | .NET 6.0           | .NET 6.0           |         NA |        NA |        NA |         NA |       NA |       NA |       NA |         NA |
| Write30      | .NET 6.0           | .NET 6.0           |  11.471 μs | 0.1323 μs | 0.1237 μs |  11.486 μs |   3.8452 |        - |        - |   15.75 KB |
| WritePhoto30 | .NET 6.0           | .NET 6.0           | 358.821 μs | 2.6561 μs | 2.4846 μs | 358.289 μs | 136.2305 | 136.2305 | 136.2305 |  734.42 KB |
| Write40      | .NET 6.0           | .NET 6.0           |  14.779 μs | 0.0605 μs | 0.0566 μs |  14.784 μs |   4.2725 |        - |        - |   17.47 KB |
| WritePhoto40 | .NET 6.0           | .NET 6.0           | 432.782 μs | 2.4521 μs | 2.0476 μs | 432.648 μs | 234.8633 | 234.8633 | 234.8633 | 1051.28 KB |
| Write21      | .NET 8.0           | .NET 8.0           |   9.983 μs | 0.0508 μs | 0.0475 μs |   9.980 μs |   4.0894 |        - |        - |   16.73 KB |
| WritePhoto21 | .NET 8.0           | .NET 8.0           | 301.212 μs | 4.0035 μs | 3.7449 μs | 301.860 μs | 142.5781 | 142.5781 | 142.5781 |  770.84 KB |
| Write30      | .NET 8.0           | .NET 8.0           |   8.213 μs | 0.0461 μs | 0.0431 μs |   8.207 μs |   3.8452 |        - |        - |   15.75 KB |
| WritePhoto30 | .NET 8.0           | .NET 8.0           | 265.140 μs | 2.9198 μs | 2.7311 μs | 265.313 μs | 136.2305 | 136.2305 | 136.2305 |  734.42 KB |
| Write40      | .NET 8.0           | .NET 8.0           |  10.432 μs | 0.0716 μs | 0.0670 μs |  10.423 μs |   4.2725 |        - |        - |   17.47 KB |
| WritePhoto40 | .NET 8.0           | .NET 8.0           | 338.531 μs | 5.3599 μs | 4.4757 μs | 336.948 μs | 234.8633 | 234.8633 | 234.8633 | 1051.28 KB |
| Write21      | .NET Framework 4.8 | .NET Framework 4.8 |  38.771 μs | 3.1159 μs | 9.1873 μs |  41.376 μs |   5.4321 |        - |        - |   22.49 KB |
| WritePhoto21 | .NET Framework 4.8 | .NET Framework 4.8 | 509.471 μs | 5.8854 μs | 5.5052 μs | 509.884 μs | 142.5781 | 142.5781 | 142.5781 |  776.63 KB |
| Write30      | .NET Framework 4.8 | .NET Framework 4.8 |  21.044 μs | 0.0700 μs | 0.0620 μs |  21.043 μs |   5.2490 |        - |        - |    21.6 KB |
| WritePhoto30 | .NET Framework 4.8 | .NET Framework 4.8 | 585.546 μs | 6.3413 μs | 5.6214 μs | 585.250 μs | 135.7422 | 135.7422 | 135.7422 |  740.57 KB |
| Write40      | .NET Framework 4.8 | .NET Framework 4.8 |  26.615 μs | 0.0616 μs | 0.0546 μs |  26.618 μs |   5.7678 |        - |        - |   23.67 KB |
| WritePhoto40 | .NET Framework 4.8 | .NET Framework 4.8 | 660.990 μs | 2.8760 μs | 2.6902 μs | 660.510 μs | 234.3750 | 234.3750 | 234.3750 | 1058.05 KB |

Benchmarks with issues:
  WriteBench.Write21: .NET 6.0(Runtime=.NET 6.0)
  WriteBench.WritePhoto21: .NET 6.0(Runtime=.NET 6.0)
