```

BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4412/22H2/2022Update)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.300
  [Host]             : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET 8.0           : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET Framework 4.8 : .NET Framework 4.8.1 (4.8.9241.0), X64 RyuJIT VectorSize=256


```
| Method       | Job                | Runtime            | Mean         | Error      | StdDev      | Median     | Gen0     | Gen1    | Gen2    | Allocated |
|------------- |------------------- |------------------- |-------------:|-----------:|------------:|-----------:|---------:|--------:|--------:|----------:|
| Parse21      | .NET 8.0           | .NET 8.0           |    16.654 μs |  0.0616 μs |   0.0577 μs |  16.658 μs |   3.2349 |       - |       - |  13.22 KB |
| Parse21Photo | .NET 8.0           | .NET 8.0           |   283.100 μs |  2.2567 μs |   2.1109 μs | 282.894 μs | 104.0039 | 77.1484 | 52.2461 | 480.57 KB |
| Parse30      | .NET 8.0           | .NET 8.0           |     7.327 μs |  0.0518 μs |   0.0433 μs |   7.314 μs |   2.4719 |       - |       - |   10.1 KB |
| Parse30Photo | .NET 8.0           | .NET 8.0           |   241.881 μs |  0.8680 μs |   0.7695 μs | 241.800 μs | 104.9805 | 77.6367 | 52.2461 | 475.37 KB |
| Parse40      | .NET 8.0           | .NET 8.0           |     8.477 μs |  0.0437 μs |   0.0387 μs |   8.470 μs |   2.6703 |       - |       - |  10.95 KB |
| Parse40Photo | .NET 8.0           | .NET 8.0           |   246.848 μs |  1.7382 μs |   1.6259 μs | 247.044 μs | 104.4922 | 78.1250 | 52.2461 | 476.07 KB |
| Parse21      | .NET Framework 4.8 | .NET Framework 4.8 |    33.741 μs |  0.1878 μs |   0.1665 μs |  33.756 μs |   3.3569 |       - |       - |  13.99 KB |
| Parse21Photo | .NET Framework 4.8 | .NET Framework 4.8 |   944.416 μs |  3.8655 μs |   3.6158 μs | 943.516 μs | 117.1875 | 76.1719 | 44.9219 | 465.67 KB |
| Parse30      | .NET Framework 4.8 | .NET Framework 4.8 |    20.873 μs |  0.1687 μs |   0.1578 μs |  20.880 μs |   2.6855 |       - |       - |  11.03 KB |
| Parse30Photo | .NET Framework 4.8 | .NET Framework 4.8 |   882.459 μs |  3.1666 μs |   2.8071 μs | 882.498 μs | 120.1172 | 88.8672 | 44.9219 | 460.75 KB |
| Parse40      | .NET Framework 4.8 | .NET Framework 4.8 |    23.614 μs |  0.1300 μs |   0.1152 μs |  23.635 μs |   2.9297 |       - |       - |  12.04 KB |
| Parse40Photo | .NET Framework 4.8 | .NET Framework 4.8 | 1,142.072 μs | 92.6596 μs | 273.2088 μs | 985.783 μs | 114.2578 | 83.0078 | 44.9219 | 461.78 KB |
