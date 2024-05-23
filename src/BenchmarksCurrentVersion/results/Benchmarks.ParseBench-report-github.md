```

BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4412/22H2/2022Update)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.205
  [Host]             : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET 8.0           : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET Framework 4.8 : .NET Framework 4.8.1 (4.8.9241.0), X64 RyuJIT VectorSize=256


```
| Method       | Job                | Runtime            | Mean         | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------------- |------------------- |------------------- |-------------:|----------:|----------:|---------:|---------:|---------:|-----------:|
| Parse21      | .NET 8.0           | .NET 8.0           |    18.211 μs | 0.3556 μs | 0.5430 μs |   6.2561 |        - |        - |   25.57 KB |
| Parse21Photo | .NET 8.0           | .NET 8.0           |   403.203 μs | 4.4015 μs | 3.9018 μs | 142.5781 | 142.5781 | 142.5781 |  929.99 KB |
| Parse30      | .NET 8.0           | .NET 8.0           |     7.905 μs | 0.0641 μs | 0.0536 μs |   4.9286 |        - |        - |   20.17 KB |
| Parse30Photo | .NET 8.0           | .NET 8.0           |   360.181 μs | 5.1241 μs | 4.7931 μs | 142.5781 | 142.5781 | 142.5781 |  860.21 KB |
| Parse40      | .NET 8.0           | .NET 8.0           |     9.114 μs | 0.1522 μs | 0.1349 μs |   5.1270 |        - |        - |   21.02 KB |
| Parse40Photo | .NET 8.0           | .NET 8.0           |   344.718 μs | 5.5596 μs | 5.2005 μs | 142.5781 | 142.5781 | 142.5781 |  861.17 KB |
| Parse21      | .NET Framework 4.8 | .NET Framework 4.8 |    36.766 μs | 0.1650 μs | 0.1544 μs |   6.7139 |   0.0610 |        - |   27.55 KB |
| Parse21Photo | .NET Framework 4.8 | .NET Framework 4.8 | 1,021.116 μs | 7.6985 μs | 7.2011 μs | 189.4531 | 189.4531 | 189.4531 | 1038.49 KB |
| Parse30      | .NET Framework 4.8 | .NET Framework 4.8 |    21.420 μs | 0.1027 μs | 0.0910 μs |   5.3101 |        - |        - |   21.81 KB |
| Parse30Photo | .NET Framework 4.8 | .NET Framework 4.8 |   985.552 μs | 6.2904 μs | 5.8840 μs | 142.5781 | 142.5781 | 142.5781 |  863.46 KB |
| Parse40      | .NET Framework 4.8 | .NET Framework 4.8 |    23.023 μs | 0.1072 μs | 0.1003 μs |   5.5237 |        - |        - |   22.65 KB |
| Parse40Photo | .NET Framework 4.8 | .NET Framework 4.8 | 1,005.552 μs | 2.9796 μs | 2.6413 μs | 189.4531 | 189.4531 | 189.4531 | 1014.28 KB |
