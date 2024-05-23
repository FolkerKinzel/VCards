```

BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4412/22H2/2022Update)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.300
  [Host]             : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET 6.0           : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2
  .NET 8.0           : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET Framework 4.8 : .NET Framework 4.8.1 (4.8.9241.0), X64 RyuJIT VectorSize=256


```
| Method       | Job                | Runtime            | Mean        | Error      | StdDev       | Gen0     | Gen1     | Gen2     | Allocated  |
|------------- |------------------- |------------------- |------------:|-----------:|-------------:|---------:|---------:|---------:|-----------:|
| Write21      | .NET 8.0           | .NET 8.0           |    12.36 μs |   0.117 μs |     0.109 μs |   6.8359 |        - |        - |   28.05 KB |
| WritePhoto21 | .NET 8.0           | .NET 8.0           | 3,837.90 μs |  13.085 μs |    10.926 μs | 171.8750 | 132.8125 | 132.8125 | 1027.55 KB |
| Write30      | .NET 8.0           | .NET 8.0           |    13.36 μs |   0.080 μs |     0.075 μs |   5.9662 |        - |        - |   24.39 KB |
| WritePhoto30 | .NET 8.0           | .NET 8.0           | 4,069.56 μs |  33.792 μs |    31.609 μs | 132.8125 | 132.8125 | 132.8125 |  969.52 KB |
| Write40      | .NET 8.0           | .NET 8.0           |    17.59 μs |   0.154 μs |     0.129 μs |   8.5449 |        - |        - |   34.98 KB |
| WritePhoto40 | .NET 8.0           | .NET 8.0           | 4,054.31 μs |  21.692 μs |    19.230 μs | 226.5625 | 226.5625 | 226.5625 | 1287.47 KB |
| Write21      | .NET Framework 4.8 | .NET Framework 4.8 |    29.25 μs |   0.248 μs |     0.220 μs |   9.1553 |        - |        - |   37.58 KB |
| WritePhoto21 | .NET Framework 4.8 | .NET Framework 4.8 | 5,669.09 μs | 525.061 μs | 1,548.154 μs | 234.3750 | 234.3750 | 234.3750 | 1333.62 KB |
| Write30      | .NET Framework 4.8 | .NET Framework 4.8 |    29.40 μs |   0.343 μs |     0.304 μs |   8.1177 |   0.0610 |        - |   33.39 KB |
| WritePhoto30 | .NET Framework 4.8 | .NET Framework 4.8 | 4,743.36 μs |  39.838 μs |    37.265 μs | 234.3750 | 234.3750 | 234.3750 | 1275.48 KB |
| Write40      | .NET Framework 4.8 | .NET Framework 4.8 |    37.84 μs |   0.340 μs |     0.284 μs |  10.8643 |   0.0610 |        - |   44.71 KB |
| WritePhoto40 | .NET Framework 4.8 | .NET Framework 4.8 | 4,828.51 μs |  59.585 μs |    52.820 μs | 328.1250 | 328.1250 | 328.1250 | 1595.68 KB |
