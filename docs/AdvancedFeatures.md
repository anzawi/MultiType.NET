# 📚 Deep Dive: MultiType.NET Features

MultiType.NET offers a powerful and expressive API for working with union types in .NET. This guide dives into the full method set provided by `Any<T1, ..., Tn>`, including all matching, mapping, selecting, switching, introspection, value access, and JSON features.

---

## ✅ Type Matching

### `Match<TResult>()`

Calls the delegate matching the current type and returns a result.

```csharp
var result = value.Match(
    i => $"int: {i}",
    s => $"string: {s}"
);
```

### `Match(Action<T1>, Action<T2>, ...)`

Same as `Match`, but void-returning:

```csharp
value.Match(
    i => Console.WriteLine(i),
    s => Console.WriteLine(s)
);
```

### `TryMatch<TResult>()`

Optional handlers for partial matching; returns default if unmatched.

```csharp
var result = value.TryMatch(
    case1: i => i.ToString()
);
```

---

## 🔁 Mapping Methods

### `Map<TResult>()`

Transforms the current value to a new value using provided delegates.

```csharp
var result = value.Map(
    i => i * 2,
    s => s.Length
);
```

### `MapSafe<TResult>()`

Same as `Map`, but skips null values.

```csharp
var result = value.MapSafe(
    i => i + 1,
    s => s?.ToUpper()
);
```

### `MapAsync<TResult>()`

Async variant of `Map`.

```csharp
var result = await value.MapAsync(
    i => Task.FromResult(i * 10),
    s => Task.FromResult(s.ToLower())
);
```

### `MapOrDefault<TResult>()`

Maps the current value or returns a fallback result.

```csharp
var result = value.MapOrDefault(
    i => i * 2,
    s => s.Length,
    () => -1
);
```

### `MapWithContext<TContext, TResult>()`

Provides external context to the mapping function.

```csharp
var result = value.MapWithContext("prefix", (v, ctx) => ctx + v);
```

### `MapAny<T1, ..., Tn>()`

Maps and returns a new `Any` with new types.

```csharp
var result = value.MapAny(
    i => i.ToString(),
    s => s.Length
); // Any<string, int>
```

### `MapValue<TSource, TResult>()`

Maps only if current value is `TSource`.

```csharp
var result = value.MapValue<int, string>(i => $"Number: {i}");
```

### `MapWhere<TResult>()`

Maps if a predicate is satisfied.

```csharp
var result = value.MapWhere(
    predicate: v => v is int i && i > 0,
    mapper: v => ((int)v!) * 10
);
```

---

## 🔍 Selecting Methods

### `Select<TResult>()`

Projects the value into another form (like `Map`, for readability).

```csharp
var output = value.Select(
    i => i.ToString(),
    s => s.ToUpper()
);
```

### `TrySelect<TResult>()`

Safe version, returns null if unmatched.

```csharp
var output = value.TrySelect(
    i => $"Val: {i}"
);
```

### `SelectAsync<TResult>()`

Async version of `Select()`.

```csharp
var output = await value.SelectAsync(
    i => Task.FromResult(i.ToString()),
    s => Task.FromResult(s.ToUpper())
);
```

### `SelectAsyncOrDefault<TResult>()`

Async version with default fallback.

```csharp
var output = await value.SelectAsyncOrDefault(
    i => Task.FromResult(i.ToString()),
    fallback: () => Task.FromResult("none")
);
```

### `SelectWithContext<TContext, TResult>()`

Provides context alongside the value.

```csharp
var result = value.SelectWithContext(
    context: 10,
    (i, ctx) => i + ctx,
    (s, ctx) => s + ctx
);
```

### `SelectWhere<TResult>()`

Performs conditional selection.

```csharp
var result = value.SelectWhere(
    filter: v => v is string,
    selector: v => ((string)v!).ToUpper()
);
```

### `SelectOrDefault<TResult>()`

Returns a selected value or fallback.

```csharp
var result = value.SelectOrDefault(
    i => i * 2,
    s => s.Length,
    fallback: () => -1
);
```

---

## 🔄 Switch Variants

### `Switch(Action<T1>, Action<T2>, ...)`

Void alternative to `Match`.

```csharp
value.Switch(
    i => Console.WriteLine(i),
    s => Console.WriteLine(s)
);
```

### `SwitchAsync(Func<T1, Task>, Func<T2, Task>, ...)`

Async version.

```csharp
await value.SwitchAsync(
    i => Task.Run(() => Console.WriteLine(i)),
    s => Task.Run(() => Console.WriteLine(s))
);
```

### `SwitchOrDefault(...)`

Fallback handler if no match.

```csharp
value.SwitchOrDefault(
    i => Console.WriteLine(i),
    s => Console.WriteLine(s),
    fallback: () => Console.WriteLine("None")
);
```

---

## 📦 Value Access

### `GetTn()` / `TryGetTn()`

Access the value if it matches the Tn type.

```csharp
if (value.TryGetT2(out var str)) { Console.WriteLine(str); }
```

### `TryGetTn(out value, out remainder)`

Extracts value and the remaining union.

```csharp
if (value.TryGetT1(out var i, out var rem)) { ... }
```

### `Deconstruct(...)`

Deconstructs union for pattern matching.

```csharp
var (isInt, isStr) = value;
```

---

## ⚙️ Construction

### `From(object)`

Creates a value from dynamic object (throws if invalid).

```csharp
var result = Any<int, string>.From("hello");
```

### `TryFrom(object, out Any<T...>)`

Safe construction.

```csharp
if (Any<int, string>.TryFrom(input, out var value)) { ... }
```

### `FromTn(Tn value)`

Directly create the union value with the correct type.

```csharp
var val = Any<int, string>.FromT2("abc");
```

### Implicit / Explicit Operators

```csharp
Any<int, string> a = 123;
string s = (string)(Any<string, bool>)"hello";
```

---

## 🧠 Introspection

### `Is<T>()`

Checks if current value is of type `T`.

### `As<T>()`

Returns the current value if of type `T`, otherwise null.

### `ActualType`

Returns `System.Type` of the current inner value.

### `AllowedTypes`

Returns array of allowed types for this union.

### `IsNull`

Returns true if both internal references are null.

### `ToString()`

Pretty representation of the current value.

---

## 📤 JSON Support

### 🔧 Global Configuration

```csharp
options.Converters.Add(new AnyJsonConverterFactory());
```

### 📌 Attribute-Based

```csharp
[JsonConverter(typeof(AnyJsonConverter<int, string>))]
public Any<int, string> Field;
```

---

## ✅ Summary

MultiType.NET offers one of the most complete and expressive APIs for handling polymorphic values in .NET. With extensive pattern matching, transformation, and introspection capabilities, it supports use cases ranging from API modeling to domain-driven design and dynamic UI layers.

Next: ➡️ [AnyGenerator](./AnyGenerator.md)
