```

BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4412/22H2/2022Update)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.300
  [Host]             : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET 8.0           : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET Framework 4.8 : .NET Framework 4.8.1 (4.8.9241.0), X64 RyuJIT VectorSize=256


```
| Method       | Job                | Runtime            | Mean         | Error      | StdDev     | Gen0     | Gen1     | Gen2    | Allocated |
|------------- |------------------- |------------------- |-------------:|-----------:|-----------:|---------:|---------:|--------:|----------:|
| Parse21      | .NET 8.0           | .NET 8.0           |    17.021 μs |  0.3348 μs |  0.6205 μs |   3.6316 |        - |       - |  14.89 KB |
| Parse21Photo | .NET 8.0           | .NET 8.0           |   294.427 μs |  2.6452 μs |  2.3449 μs | 155.2734 | 117.1875 | 86.4258 | 585.61 KB |
| Parse30      | .NET 8.0           | .NET 8.0           |     6.886 μs |  0.0402 μs |  0.0357 μs |   2.7847 |        - |       - |  11.38 KB |
| Parse30Photo | .NET 8.0           | .NET 8.0           |   278.071 μs |  2.4864 μs |  2.2041 μs | 198.2422 | 113.2813 | 87.4023 | 755.77 KB |
| Parse40      | .NET 8.0           | .NET 8.0           |     8.652 μs |  0.1688 μs |  0.1658 μs |   3.0365 |        - |       - |  12.45 KB |
| Parse40Photo | .NET 8.0           | .NET 8.0           |   285.475 μs |  4.7562 μs |  4.2163 μs | 199.2188 | 112.3047 | 88.3789 | 756.97 KB |
| Parse21      | .NET Framework 4.8 | .NET Framework 4.8 |    34.662 μs |  0.1480 μs |  0.1312 μs |   4.0894 |        - |       - |  16.76 KB |
| Parse21Photo | .NET Framework 4.8 | .NET Framework 4.8 | 1,077.511 μs |  5.5093 μs |  5.1534 μs | 185.5469 | 154.2969 | 89.8438 | 750.09 KB |
| Parse30      | .NET Framework 4.8 | .NET Framework 4.8 |    20.981 μs |  0.3671 μs |  0.3065 μs |   3.1433 |        - |       - |   12.9 KB |
| Parse30Photo | .NET Framework 4.8 | .NET Framework 4.8 | 1,075.397 μs |  8.5427 μs |  7.9908 μs | 224.6094 | 169.9219 | 89.8438 | 920.38 KB |
| Parse40      | .NET Framework 4.8 | .NET Framework 4.8 |    22.913 μs |  0.1243 μs |  0.1163 μs |   3.4180 |        - |       - |  14.01 KB |
| Parse40Photo | .NET Framework 4.8 | .NET Framework 4.8 | 1,665.573 μs | 28.2562 μs | 31.4067 μs | 218.7500 | 175.7813 | 89.8438 | 921.36 KB |
