# 🔧 CLI Generator — `MultiType.NET.SourceGenerator`

The `MultiType.NET.SourceGenerator` is a powerful CLI tool that lets you generate union types beyond the built-in `Any<T1..T16>` limit, including:

* ✅ `Any<T1..Tn>` how many types do you want?
* ✅ Matching JSON converters per arity
* ✅ A factory-based JSON converter that supports them all

---

## 🚀 Installation

Install the tool globally:

```bash
dotnet tool install --global MultiType.NET.SourceGenerator
```

Or update it if already installed:

```bash
dotnet tool update --global MultiType.NET.SourceGenerator
```

---

## 🧪 Usage

From your solution root:

```bash
anygen --maxArity=50
```

This will:

* Generate `Any17`, `Any18`, ..., `Any50`
* Generate corresponding JSON converters for each type
* Compatible with universal `AnyJsonConverterFactory`

### Options

| Flag           | Description                                           |
| -------------- | ----------------------------------------------------- |
| `--maxArity=n` | Max number of generic types to generate (default: 16) |

---

## 📁 Output Structure

The generated files will be placed directly into your project:

```
MultiType.NET.Core/
├── Anys/Generated/
│   ├── Any17.g.cs
│   ├── ...
│   └── Any50.g.cs
├── AnySerializations/Generated/
│   ├── Any17JsonConverter.g.cs
│   ├── ...
│   └── AnyJsonConverterFactory.g.cs
```

> 📌 Make sure you are installed `MultiType.NET` package.

---

## 🧠 How It Works

Internally, the CLI uses:

* `AnyEmitter.EmitAnyTypes()` to build type-safe structs
* Roslyn APIs to format and normalize code
* Auto-commented headers to signal generated code

---

## 🔄 Regenerating

You can re-run the generator at any time. Existing `.g.cs` files will be overwritten.

To automate this, add a build script:

```bash
multitypegen --maxArity=50 --silent
```

---

## ⚠️ Notes

* The CLI must be run manually (it is not a source generator by itself).
* It works alongside `MultiType.NET` in the same solution.
* Do not manually edit `.g.cs` files.

---

## ✅ Why Use This?

| Benefit          | Explanation                            |
| ---------------- | -------------------------------------- |
| Unbounded Unions | Supports 17+ types in `Any<T1..Tn>`    |
| JSON Ready       | Each union has a matching converter    |
| API Friendly     | Compatible with ASP.NET & Minimal APIs |
| Fast & Light     | Outputs optimized `.g.cs` source code  |

---

## 🔗 Next Step

Go back to [Deep Dive Features](./AdvancedFeatures.md) or start using [AnyGenerator](./AnyGenerator.md).
