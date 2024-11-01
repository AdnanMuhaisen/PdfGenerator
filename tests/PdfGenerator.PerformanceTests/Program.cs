using BenchmarkDotNet.Running;
using PdfGenerator.PerformanceTests.Benchmarks;

_ = BenchmarkRunner.Run<PdfServiceBenchmarks>();