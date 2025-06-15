using System;
using MultiType.NET.SourceGenerator;

int maxArity = 16;

foreach (var arg in args)
{
    if (arg.StartsWith("--maxArity=") &&
        int.TryParse(arg["--maxArity=".Length..], out var parsed))
    {
        maxArity = parsed;
    }
    else if (arg is "--help" or "-h")
    {
        Console.WriteLine("""
                          📦 MultiType.NET Generator CLI
                          ------------------------------

                          Options:
                            --maxArity=N   Maximum arity to generate (must be > 16)
                            --help / -h    Show this help message
                          """);
        return;
    }
}

if (maxArity < 17)
{
    Console.WriteLine($"Cannot generate Any<{maxArity}>. Must be > 16.");
    Console.WriteLine("By default MultiType.NET Generator support types up to Any<16>.");

}

Console.WriteLine($"MultiType.NET Generator - Generating up to Any<{maxArity}>");

AnyEmitter.EmitAnyTypes(maxArity);

Console.WriteLine("Generation complete.");