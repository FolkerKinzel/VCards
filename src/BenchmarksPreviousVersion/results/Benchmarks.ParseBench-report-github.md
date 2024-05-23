```

BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4412/22H2/2022Update)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.300
  [Host]             : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET 8.0           : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET Framework 4.8 : .NET Framework 4.8.1 (4.8.9241.0), X64 RyuJIT VectorSize=256


```
| Method       | Job                | Runtime            | Mean         | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------------- |------------------- |------------------- |-------------:|----------:|----------:|---------:|---------:|---------:|-----------:|
| Parse21      | .NET 8.0           | .NET 8.0           |    18.231 μs | 0.0831 μs | 0.0777 μs |   6.2561 |        - |        - |   25.57 KB |
| Parse21Photo | .NET 8.0           | .NET 8.0           |   387.111 μs | 4.2931 μs | 4.0158 μs | 142.5781 | 142.5781 | 142.5781 |  929.99 KB |
| Parse30      | .NET 8.0           | .NET 8.0           |     7.961 μs | 0.1281 μs | 0.1136 μs |   4.9286 |        - |        - |   20.17 KB |
| Parse30Photo | .NET 8.0           | .NET 8.0           |   361.243 μs | 4.0389 μs | 3.7780 μs | 142.5781 | 142.5781 | 142.5781 |  860.21 KB |
| Parse40      | .NET 8.0           | .NET 8.0           |    10.209 μs | 0.1104 μs | 0.1033 μs |   5.1270 |        - |        - |   21.02 KB |
| Parse40Photo | .NET 8.0           | .NET 8.0           |   348.733 μs | 6.4601 μs | 5.7267 μs | 142.5781 | 142.5781 | 142.5781 |  861.17 KB |
| Parse21      | .NET Framework 4.8 | .NET Framework 4.8 |    36.586 μs | 0.1851 μs | 0.1732 μs |   6.7139 |        - |        - |   27.55 KB |
| Parse21Photo | .NET Framework 4.8 | .NET Framework 4.8 | 1,029.185 μs | 6.1505 μs | 5.7532 μs | 189.4531 | 189.4531 | 189.4531 | 1038.49 KB |
| Parse30      | .NET Framework 4.8 | .NET Framework 4.8 |    20.632 μs | 0.0921 μs | 0.0862 μs |   5.3101 |        - |        - |   21.81 KB |
| Parse30Photo | .NET Framework 4.8 | .NET Framework 4.8 |   987.109 μs | 7.1278 μs | 6.3186 μs | 142.5781 | 142.5781 | 142.5781 |  863.46 KB |
| Parse40      | .NET Framework 4.8 | .NET Framework 4.8 |    23.457 μs | 0.0663 μs | 0.0588 μs |   5.5237 |        - |        - |   22.65 KB |
| Parse40Photo | .NET Framework 4.8 | .NET Framework 4.8 | 1,027.183 μs | 4.3105 μs | 3.8211 μs | 189.4531 | 189.4531 | 189.4531 | 1014.28 KB |
