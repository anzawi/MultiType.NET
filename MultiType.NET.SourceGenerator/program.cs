using System;
using System.IO;
using MultiType.NET.SourceGenerator;

void PrintHeader()
{
    var prevColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Cyan;

    Console.WriteLine("╔════════════════════════════════════════════════════╗");
    Console.WriteLine("║          MultiType.NET Any Source Generator        ║");
    Console.WriteLine("╚════════════════════════════════════════════════════╝");

    Console.ForegroundColor = prevColor;
}

void WriteError(string message, string? hint = null)
{
    var prevColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("❌ " + message);
    if (hint != null)
    {
        Console.WriteLine("    >> " + hint);   
    }
    Console.ForegroundColor = prevColor;
}
void WriteWarning(string message)
{
    var prev = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("⚠️ " + message);
    Console.ForegroundColor = prev;
}

void WriteSuccess(string message)
{
    var prev = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("✅ " + message);
    Console.ForegroundColor = prev;
}

PrintHeader();
var maxArity = 16;
string? projectPath = null;

var isCoreProject = false;

for (var i = 0; i < args.Length; i++)
{
    var arg = args[i];

    switch (arg)
    {
        case "--help":
        case "-h":
            Console.WriteLine("""
                📦 MultiType.NET Generator CLI
                ------------------------------

                Usage:
                  anygen --project <path> --maxArity <N>

                Options:
                  --project <path>     Required: path to the target .csproj file
                  --maxArity <N>       Required: max arity to generate (must be > 16)
                  --help, -h           Show this help message
                """);
            return;

        case "--project":
            if (i + 1 >= args.Length)
            {
                WriteError("Missing value for --project", " Try: anygen --project <path> --maxArity <N>");
                return;
            }

            projectPath = args[++i].Trim('"');
            isCoreProject = projectPath.EndsWith("MultiType.Core.csproj", StringComparison.OrdinalIgnoreCase);
            break;

        case "--maxArity":
            if (i + 1 >= args.Length || !int.TryParse(args[++i], out maxArity))
            {
                WriteError("Missing value for --maxArity", " Try: anygen --project <path> --maxArity <N>");
                return;
            }
            break;

        default:
            WriteWarning("Unknown argument: " + arg);
            break;
    }
}

// Validate project path
if (string.IsNullOrWhiteSpace(projectPath))
{
    WriteError("Missing required argument: --project <path-to-project.csproj>", " Try: anygen --project <path> --maxArity <N>");
    return;
}

if (!isCoreProject && (!File.Exists(projectPath) || !projectPath.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)))
{
    WriteError($"Invalid project path: {projectPath}", "It must be a valid path to a .csproj file.");
    return;
}


if (!isCoreProject && maxArity < 17)
{
    WriteError($"Cannot generate Any<{maxArity}>. It must be greater than 16.", "'MultiType.Core' package Built-in support covers up to 16.");
    return;
}

WriteSuccess($"✅ Project found: {projectPath}");
Console.WriteLine($"🚀 Generating Any<T...> types up to Any<{maxArity}>...");

var projectDir = Path.GetDirectoryName(Path.GetFullPath(projectPath))!;
var outputPath = Path.Combine(projectDir, "Generated");

AnyEmitter.EmitAnyTypes(maxArity, outputPath, isCoreProject);

Console.WriteLine($"✅ Generation complete. Files written to: {outputPath}");
