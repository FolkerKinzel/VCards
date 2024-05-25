```

BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4412/22H2/2022Update)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.300
  [Host]             : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET 8.0           : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  .NET Framework 4.8 : .NET Framework 4.8.1 (4.8.9241.0), X64 RyuJIT VectorSize=256


```
| Method       | Job                | Runtime            | Mean       | Error     | StdDev    | Median     | Gen0     | Gen1     | Gen2     | Allocated |
|------------- |------------------- |------------------- |-----------:|----------:|----------:|-----------:|---------:|---------:|---------:|----------:|
| Parse21      | .NET 8.0           | .NET 8.0           |  16.868 μs | 0.3350 μs | 0.6918 μs |  17.161 μs |   3.4485 |        - |        - |  14.12 KB |
| Parse21Photo | .NET 8.0           | .NET 8.0           | 323.762 μs | 1.5898 μs | 1.4871 μs | 323.897 μs | 104.9805 | 104.9805 | 104.9805 | 650.02 KB |
| Parse30      | .NET 8.0           | .NET 8.0           |   7.556 μs | 0.1054 μs | 0.0880 μs |   7.538 μs |   2.6245 |        - |        - |  10.77 KB |
| Parse30Photo | .NET 8.0           | .NET 8.0           | 283.783 μs | 2.3988 μs | 2.2439 μs | 283.729 μs | 104.9805 | 104.9805 | 104.9805 | 642.17 KB |
| Parse40      | .NET 8.0           | .NET 8.0           |   8.443 μs | 0.0471 μs | 0.0393 μs |   8.444 μs |   2.8229 |        - |        - |  11.53 KB |
| Parse40Photo | .NET 8.0           | .NET 8.0           | 287.048 μs | 2.3703 μs | 1.9793 μs | 286.910 μs | 104.9805 | 104.9805 | 104.9805 | 643.24 KB |
| Parse21      | .NET Framework 4.8 | .NET Framework 4.8 |  33.660 μs | 0.1002 μs | 0.0888 μs |  33.650 μs |   3.4790 |        - |        - |  14.42 KB |
| Parse21Photo | .NET Framework 4.8 | .NET Framework 4.8 | 966.000 μs | 3.6500 μs | 3.0479 μs | 966.053 μs | 166.9922 | 129.8828 |  90.8203 | 618.26 KB |
| Parse30      | .NET Framework 4.8 | .NET Framework 4.8 |  21.411 μs | 0.1127 μs | 0.1055 μs |  21.445 μs |   2.7161 |        - |        - |  11.16 KB |
| Parse30Photo | .NET Framework 4.8 | .NET Framework 4.8 | 928.752 μs | 3.2743 μs | 3.0628 μs | 927.281 μs | 166.0156 | 136.7188 |  90.8203 | 610.83 KB |
| Parse40      | .NET Framework 4.8 | .NET Framework 4.8 |  29.901 μs | 2.7165 μs | 8.0096 μs |  24.278 μs |   2.9907 |        - |        - |  12.33 KB |
| Parse40Photo | .NET Framework 4.8 | .NET Framework 4.8 | 980.929 μs | 4.2572 μs | 3.9822 μs | 979.729 μs | 164.0625 | 134.7656 |  89.8438 |  612.2 KB |
