# 🚦 MultiType.NET

**Type-safe, zero-allocation discriminated unions for modern .NET**  
Bring TypeScript-style union types to C# — safer than `object`, cleaner than `if`, and built for real-world APIs and models.
```csharp
Any<int, string> result = "hello";
```

![NuGet](https://img.shields.io/nuget/v/MultiType.NET.Core)
![Downloads](https://img.shields.io/nuget/dt/MultiType.NET.Core)
![License](https://img.shields.io/github/license/mohammadan/MultiType.NET)
![Build](https://img.shields.io/github/actions/workflow/status/mohammadan/MultiType.NET/ci.yml)

---

## 🧠 What Is a Union Type?

A **union type** lets a value be one of multiple specified types — like this in TypeScript:

```
let result = number | string | null

// C# with MultiType.NET
Any<int, string> result = "hello";
```

MultiType.NET brings that idea to C# with full support for:

- ✅ Type-safe access (`Is<T>()`, `As<T>()`, `Match`)
- ✅ JSON (de)serialization
- ✅ Zero boxing
- ✅ ASP.NET API compatibility
- ✅ Source generation for no-boilerplate code

---

## ✨ Features Overview

| Category       | Features |
|----------------|----------|
| ✅ Matching     | `Match`, `TryMatch`, `Switch`, `SwitchAsync`, `SwitchOrDefault` |
| 🔁 Mapping      | `Map`, `MapAsync`, `MapSafe`, `MapValue`, `MapAny`, `MapWithContext`, `MapOrDefault`, `MapWhere` |
| 🔍 Selection    | `Select`, `TrySelect`, `SelectAsync`, `SelectAsyncOrDefault`, `SelectWithContext`, `SelectWhere` |
| 📦 Extraction   | `GetTn`, `TryGetTn`, `GetTn(out remainder)`, `Deconstruct(...)` |
| 🧠 Introspection| `ActualType`, `AllowedTypes`, `IsNull`, `ToString()` |
| ⚙️ Construction | `From`, `TryFrom`, `FromTn`, implicit/explicit operators |
| 📤 Serialization| Native `System.Text.Json` support (global or attribute-based) |
| 🧑‍💻 API-Ready   | Works with Controllers & Minimal APIs |
| 🧩 Generator     | Auto-generates union types via `[GenerateAny]` or `Any<T1..Tn>` |

---

## 🚀 Get Started

### 📦 Install Core Library

```
dotnet add package MultiType.NET.Core
```
This gives you:

- `Any<T1..T16>` prebuilt types
- Core features like `Match`, `Map`, `Switch`, `TryMatch`, ...etc
- JSON support via converter factory

### 🔧 Add Optional Source Generators

| Package | Description |
|--------|-------------|
| `MultiType.NET.Generator` | Adds `[GenerateAny(typeof(...))]` support |
| `MultiType.NET.SourceGenerator` | Enables `Any<T1..Tn>` (over 16 types), JSON support, API integration |


```
dotnet add package MultiType.NET.Generator
```
This allow you to generate a custom types with `[GenerateAny]`, for more details [MultiType.NET.Generator attribute](link-here).
```
[GenerateAny(typeof(string), typeof(MyType))]
public partial struct MyCustomType{}
```
This will generate `MyCustomType` with all MultiType APIs.

### Need to generate `Any<T17, ..., Tn>`?

Install the official CLI generator:

```
dotnet tool install --global MultiType.NET.SourceGenerator
```
Then run:

```
multitypegen --maxArity=50
```

for more details and documentation [MultiType.NET.SourceGenerator CLI](link-here)
---

## 💡 Learn by Example

```
Any<int, string, DateTime> result = "hello";

string output = result.Match(
    i => $"Int: {i}",
    s => $"String: {s}",
    d => $"Date: {d:yyyy-MM-dd}"
);
```

```
if (result.TryGetT1(out var i, out var remainder))
    Console.WriteLine($"Was int: {i}");
else
    Console.WriteLine($"Not an int: {remainder}");
```

```
var summary = result.Select(
    i => $"# {i}",
    s => s.ToUpperInvariant(),
    d => d.ToShortTimeString()
);
```

---

## 🧱 Creating Any Values

MultiType.NET offers multiple ways to construct union values.

### ✅ `From(...)` — dynamic dispatch

```
object raw = 123;
var value = Any<int, string, DateTime>.From(raw);
```

> 💡 Throws if the value is not one of the allowed types.

### ✅ `TryFrom(...)` — safe version

```
if (Any<int, string>.TryFrom(someValue, out var result))
{
    // Use result
}
```

### ✅ `FromTn(...)` — type-specific creation

```
var a = Any<int, string, bool>.FromT1(42);
var b = Any<int, string, bool>.FromT2("hello");
```

> 💡 These are especially useful for code generation, dynamic input handling, or overload clarity.

### ✅ Implicit Operators

```
Any<int, string> v1 = 5;
Any<int, string> v2 = "done";
```

---

## 📦 JSON Serialization

MultiType.NET works seamlessly with `System.Text.Json`.

### ✅ Global registration

```
builder.Services.Configure<JsonOptions>(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new AnyJsonConverterFactory());
});
```

### ✅ Per-type registration

```
[JsonConverter(typeof(AnyJsonConverterFactory))]
public readonly partial struct MyUnionType;
```

### 🧪 Example

```
var options = new JsonSerializerOptions { WriteIndented = true };
string json = JsonSerializer.Serialize(Any<int, string>.From(123), options);
```

> 💡 JSON output includes both the value and the represented type.

---

## 🧩 Custom Types with `[GenerateAny]`
> ⚠️ Requires `MultiType.NET.Generator` installed.

```
[GenerateAny(typeof(int), typeof(string), typeof(Guid))]
public partial struct ResponsePayload;
```

```
ResponsePayload payload = Guid.NewGuid();

payload.Match(
    i => Console.WriteLine($"Int: {i}"),
    s => Console.WriteLine($"Str: {s}"),
    g => Console.WriteLine($"Guid: {g}")
);
```

** More advanced Type**
```
[GenerateAny(typeof(Success), typeof(Warning), typeof(Error), typeof(Info))]
public partial struct StatusType
{
    public static StatusType From(string value) => value.ToLowerInvariant() switch
    {
        "success" => new Success(),
        "warning" => new Warning(),
        "error" => new Error(),
        "info" => new Info(),
        _ => throw new ArgumentException("Invalid status", nameof(value))
    };

    public bool IsSuccess => this.Is<Success>();
    public bool IsWarning => this.Is<Warning>();
    public bool IsError => this.Is<Error>();
    public bool IsInfo => this.Is<Info>();

    public readonly struct Success { }
    public readonly struct Warning { }
    public readonly struct Error { }
    public readonly struct Info { }
}
```

---

## ⚙️ How It Works (Behind the Scenes)

- `Any<T1, ..., Tn>` is a readonly struct with internal value/ref handling.
- Tracks active type via `TypeIndex`
- Source generators:
    - Generate custom union types with all logic
    - Auto-wire JSON support
    - Add full method surface (Match, Map, Select, etc.)

---

## 🧪 Real-World Use Cases

| Use Case | Example |
|----------|---------|
| ✅ API Results | `Any<SuccessDto, ErrorDto>` |
| 🧑‍⚖️ Workflow Results | `Any<Approved, Rejected, Escalated>` |
| 🧠 State Modeling | `Any<Draft, Submitted, Published>` |
| 🧾 Flexible Inputs | `Any<string, int, bool>` |
| 🔄 Retry Backoff | `Any<RetryLater, Fail, Success>` |

---

## 📁 Project Structure

| Project                        | Description |
|-------------------------------|-------------|
| `MultiType.NET.Core`          | Runtime types and logic |
| `MultiType.NET.Generator`     | `[GenerateAny]` source generation |
| `MultiType.NET.SourceGenerator` | JSON + `Any<T1..Tn>` generation |

---

## 📘 Documentation

| Resource | Link |
|----------|------|
| 🔍 Advanced Features | [docs/AdvancedFeatures.md](docs/AdvancedFeatures.md) |
| 🧠 Generator Guide | [docs/SourceGenerators.md](docs/SourceGenerators.md) |
| 💡 Integration Tips | [docs/Integration.md](docs/Integration.md) |


---

## 🙌 Contributing

Contributions, issues, and PRs are welcome!  
See [`CONTRIBUTING.md`](CONTRIBUTING.md) to get started.

---

## 📄 License

MIT — [LICENSE](LICENSE)

---

> Because type safety shouldn't be optional.
