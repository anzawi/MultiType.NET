# 📦 MultiType.NET

MultiType.NET is a high-performance, strongly-typed **discriminated union** library for .NET. It provides type-safe modeling of multi-type values with zero-allocation structs, full pattern matching, JSON integration, and source generator support.

## ✨ Features

### ✅ Type-Safe Discriminated Unions

- Supports `Union<T1, T2>` up to `Union<T1, ..., T8>`
- Immutable `readonly struct` implementation
- Full nullable reference type support
- Safe type access: `Is<T>()`, `As<T>()`
- Pattern matching APIs: `Match`, `TryMatch`, `If`

### 🔁 JSON Integration

- Native `System.Text.Json` support with discriminator format
- `UnionJsonConverterFactory` for simple registration
- Safe deserialization based on type index

```json
{
"type": 1,
"value": "hello"
}
```

### 🧠 Pattern Matching

- `Match`: exhaustive and safe
- `TryMatch`: nullable or `out` pattern
- `If`: lightweight conditional

### ⚙️ Source Generator

- `[GenerateUnion(...)]` creates complete union struct
- Auto-generates pattern matching and JSON converter

---

## 🚀 Getting Started

### 🔧 Creating a Union

```csharp
var value = new Union<string, int>("hello");
```

### 🔍 Pattern Matching

```csharp
var result = value.Match(
s => $"It's a string: {s}",
i => $"It's a number: {i}"
);
```

### 🔐 TryMatch Pattern

```csharp
if (value.TryMatch(out int number))
Console.WriteLine($"Number: {number}");
```

### 💬 Conditional If

```csharp
value.If((string s) => Console.WriteLine($"Hello: {s}"));
```

---

## 🧩 JSON Support

### 🔌 Register the Converter

```csharp
var options = new JsonSerializerOptions();
options.Converters.Add(new UnionJsonConverterFactory());
```

### 📦 JSON Format

```json
{
"type": 2,
"value": 42
}
```

> Type index starts at `1` for the first generic parameter.

---

## 🧱 Interface: IUnion

| Property     | Description                                   |
|--------------|-----------------------------------------------|
| `TypeIndex`  | Index of the active type (1-based)            |
| `Value`      | The raw object value                          |
| `Type`       | The runtime type of the active value          |
| `Is<T>()`    | Whether the current value is of type `T`      |
| `As<T>()`    | Casts the value to `T`, throws on mismatch    |

---

## 🧬 Source Generation

```csharp
[GenerateUnion(typeof(string), typeof(int), typeof(bool))]
public partial struct MyUnion { }
```

### 🛠️ Project Setup

To enable source generation in a consumer project:

```xml
<ItemGroup>
<ProjectReference Include="..\MultiType.NET.Generators\MultiType.NET.Generators.csproj"
OutputItemType="Analyzer"
ReferenceOutputAssembly="false" />
</ItemGroup>
```

---

## 🧭 Best Practices

1. Use the fewest type parameters possible.
2. Prefer `Match` or `TryMatch` over type casting.
3. Register the JSON converter once per `JsonSerializerOptions`.
4. Use `[GenerateUnion]` for reusable custom unions.
5. Handle all branches in `Match` for safety.

---

## ⚠️ Limitations

- Max 8 types per union
- Value types are boxed internally
- No runtime inheritance or union polymorphism

---

## 🧪 Performance Notes

| Feature         | Efficiency                  |
|------------------|-----------------------------|
| Structs          | Zero-allocation             |
| Matching         | Allocation-free             |
| JSON Serialization | Boxes value types for encoding |

---

## 📦 Requirements

- .NET Standard 2.0+
- .NET 6 / .NET 8 supported
- C# 8.0+ required for nullable types
- Uses only `System.Text.Json` (no Newtonsoft required)

---

## 🤝 Contributing

### 🙌 How to Contribute

- Report bugs or request features via GitHub issues
- Submit pull requests with improvements or new unions
- Add examples or improve documentation

### 📌 Contribution Ideas

- Support `Union<T1,...,T16>`
- Add optional Newtonsoft.Json integration
- Add `UnionSwitch` syntax sugar
- Improve generator to support default values, attributes
- Add Roslyn analyzer to recommend union usage

### 🔧 Local Dev Setup

```xml
<ProjectReference Include="MultiType.NET.Generators.csproj"
OutputItemType="Analyzer"
ReferenceOutputAssembly="false" />
```

---

## 🌟 Show Your Support

If you find this project useful, star it and share it with others!
