using Xunit;

// Mirrors TNZAPI.NET.Tests/AssemblyInfo.cs — tests swap the SDK's shared static
// HttpRequest.MessageHandler field per-test, which is unsafe under parallel execution.
[assembly: CollectionBehavior(DisableTestParallelization = true)]