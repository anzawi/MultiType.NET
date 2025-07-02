# 🧩 AnyGenerator — Custom Union Type Generator

The `MultiType.NET.Generator` package enables you to define your own custom union types using a simple `[GenerateAny]` attribute. It removes boilerplate and auto-generates rich union logic for your own domain-specific types.

---

## ⚙️ What It Does

* Generates a struct based on your type definition.
* Implements all core APIs: `Match`, `TryMatch`, `Map`, `Switch`, `From`, `TryFrom`, and more.
* Adds optional `System.Text.Json` support for serialization.
* Integrates seamlessly with Controllers, Minimal APIs, and DTOs.

---

## 🚀 How to Use

### 1️⃣ Install the Generator Package

```bash
dotnet add package MultiType.NET.Generator
```

### 2️⃣ Annotate Your Partial Struct

```csharp
[GenerateAny(typeof(int), typeof(string))]
public partial struct IntOrString;
```

### 3️⃣ Use It Like Any\<T1, T2>

```csharp
IntOrString value = "hello";

var result = value.Match(
    i => $"Int: {i}",
    s => $"String: {s}"
);
```

---

## ✨ Advanced Example

```csharp
[GenerateAny(typeof(Success), typeof(Warning), typeof(Error), typeof(Info))]
public partial struct StatusType
{
    public static StatusType From(string value) => value.ToLowerInvariant() switch
    {
        "success" => new Success(),
        "warning" => new Warning(),
        "error" => new Error(),
        "info" => new Info(),
        _ => throw new ArgumentException("Invalid status")
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

## 📤 JSON Support

The generated type is automatically decorated to support `System.Text.Json`.

### Global Config

```csharp
builder.Services.Configure<JsonOptions>(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new AnyJsonConverterFactory());
});
```

### Per-Type Config

```csharp
[JsonConverter(typeof(AnyJsonConverterFactory))]
public partial struct MyUnionType;
```

---

## ✅ Use Cases

| Use Case        | Example                                         |
| --------------- | ----------------------------------------------- |
| API Payloads    | `IntOrString` for flexible IDs                  |
| Workflow Status | `StatusType` for state modeling                 |
| Dynamic Input   | `StringOrBoolOrInt` from forms or query strings |

---

## 📎 Notes

* Generated files are `.g.cs` and automatically included during compilation.
* Works with source generators (no runtime reflection).
* Types must be `readonly struct` or `record struct`.

---

## 📘 Next Step

➡️ Proceed to [CLI Generator](./CLIGenerator.md) for generating `Any<T1..T50>` and custom JSON converters.
