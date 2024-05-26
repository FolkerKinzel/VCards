```

BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4412/22H2/2022Update)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.300
  [Host]             : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET 8.0           : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET Framework 4.8 : .NET Framework 4.8.1 (4.8.9241.0), X64 RyuJIT VectorSize=256


```
| Method       | Job                | Runtime            | Mean       | Error     | StdDev     | Median     | Gen0     | Gen1    | Gen2    | Allocated |
|------------- |------------------- |------------------- |-----------:|----------:|-----------:|-----------:|---------:|--------:|--------:|----------:|
| Parse21      | .NET 8.0           | .NET 8.0           |  17.234 μs | 0.1140 μs |  0.1010 μs |  17.231 μs |   3.2349 |       - |       - |  13.22 KB |
| Parse21Photo | .NET 8.0           | .NET 8.0           | 286.598 μs | 2.6237 μs |  2.4542 μs | 286.615 μs | 104.0039 | 77.1484 | 52.2461 | 480.44 KB |
| Parse30      | .NET 8.0           | .NET 8.0           |   7.357 μs | 0.0445 μs |  0.0371 μs |   7.347 μs |   2.4719 |       - |       - |   10.1 KB |
| Parse30Photo | .NET 8.0           | .NET 8.0           | 246.309 μs | 1.6265 μs |  1.5214 μs | 246.119 μs | 104.4922 | 77.6367 | 52.2461 | 474.84 KB |
| Parse40      | .NET 8.0           | .NET 8.0           |   8.658 μs | 0.0273 μs |  0.0228 μs |   8.658 μs |   2.6703 |       - |       - |  10.95 KB |
| Parse40Photo | .NET 8.0           | .NET 8.0           | 251.943 μs | 2.7789 μs |  2.5994 μs | 252.918 μs | 104.4922 | 78.1250 | 52.2461 | 475.75 KB |
| Parse21      | .NET Framework 4.8 | .NET Framework 4.8 |  34.962 μs | 0.2663 μs |  0.2079 μs |  34.985 μs |   3.3569 |       - |       - |  13.99 KB |
| Parse21Photo | .NET Framework 4.8 | .NET Framework 4.8 | 956.985 μs | 7.0485 μs |  5.8858 μs | 955.416 μs | 117.1875 | 74.2188 | 44.9219 | 465.68 KB |
| Parse30      | .NET Framework 4.8 | .NET Framework 4.8 |  21.365 μs | 0.0898 μs |  0.0840 μs |  21.360 μs |   2.6855 |       - |       - |  11.03 KB |
| Parse30Photo | .NET Framework 4.8 | .NET Framework 4.8 | 900.224 μs | 1.9433 μs |  1.7227 μs | 900.570 μs | 120.1172 | 88.8672 | 44.9219 | 460.75 KB |
| Parse40      | .NET Framework 4.8 | .NET Framework 4.8 |  37.599 μs | 3.6591 μs | 10.7889 μs |  41.174 μs |   2.8076 |       - |       - |  11.72 KB |
| Parse40Photo | .NET Framework 4.8 | .NET Framework 4.8 | 974.716 μs | 6.5333 μs |  6.1113 μs | 974.594 μs | 115.2344 | 83.9844 | 44.9219 | 461.47 KB |
