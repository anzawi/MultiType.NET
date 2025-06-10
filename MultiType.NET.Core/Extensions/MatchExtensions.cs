namespace MultiType.NET.Core.Extensions;

using Unions;

/// <summary>
/// Provides extension methods for matching and handling instances of union types.
/// </summary>
public static class MatchExtensions
{
    /// <summary>
    /// Attempts to match the provided union instance to a specific type and retrieves its value if it matches.
    /// </summary>
    /// <typeparam name="T">The type to match against the union's contained value.</typeparam>
    /// <param name="union">The union instance to be checked and potentially matched.</param>
    /// <param name="value">
    /// The output parameter that will hold the matched value of type <typeparamref name="T"/> if the match is successful.
    /// If the match fails, this parameter will remain as its default value.
    /// </param>
    /// <returns>
    /// A boolean value indicating whether the match was successful. Returns true if the union's contained value
    /// matches the specified type <typeparamref name="T"/>, and false otherwise.
    /// </returns>
    public static bool TryMatch<T>(this IUnion union, out T? value)
    {
        value = default;
        if (union.TypeIndex == 0)
        {
            return false;
        }

        if (union.Is<T>())
        {
            value = union.As<T>();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Executes the specified action if the type of the current union instance matches the specified type.
    /// </summary>
    /// <typeparam name="T">The type to match against the union instance.</typeparam>
    /// <param name="union">The union instance being evaluated.</param>
    /// <param name="action">The action to execute if the type matches.</param>
    /// <returns>True if the type matches and the action is executed, otherwise false.</returns>
    public static bool If<T>(this IUnion union, Action<T> action)
    {
        if (union.TypeIndex == 0)
        {
            return false;
        }

        if (union.Is<T>())
        {
            action(union.As<T>());
            return true;
        }

        return false;
    }

    /// Evaluates the current `Union` instance using the appropriate case function and returns a result of type `TResult`.
    /// <param name="union">
    /// The `Union` instance containing two possible types.
    /// </param>
    /// <param name="case1">
    /// The function to execute if the `Union` instance contains a value of type `T1`.
    /// </param>
    /// <param name="case2">
    /// The function to execute if the `Union` instance contains a value of type `T2`.
    /// </param>
    /// <returns>
    /// The result of the appropriate case function applied to the value of the `Union`.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the `Union` is not initialized.
    /// </exception>
    public static TResult Match<T1, T2, TResult>(
        this Union<T1, T2> union,
        Func<T1?, TResult> case1,
        Func<T2?, TResult> case2)
    {
        return union.TypeIndex switch
        {
            1 => case1(union.Is<T1>() ? union.As<T1>() : default),
            2 => case2(union.Is<T2>() ? union.As<T2>() : default),
            _ => throw new InvalidOperationException("Union is not initialized."),
        };
    }

    /// Matches the provided union instance with one of the specified actions based on its contained type.
    /// The correct action is executed based on the type of the value stored within the union.
    /// <typeparam name="T1">The type of the first possible value in the union.</typeparam>
    /// <typeparam name="T2">The type of the second possible value in the union.</typeparam>
    /// <param name="union">The union instance to be matched.</param>
    /// <param name="case1">The action to execute if the union contains a value of type <typeparamref name="T1"/>.</param>
    /// <param name="case2">The action to execute if the union contains a value of type <typeparamref name="T2"/>.</param>
    /// <exception cref="InvalidOperationException">Thrown if the union is not properly initialized.</exception>
    public static void Match<T1, T2>(
        this Union<T1, T2> union,
        Action<T1?> case1,
        Action<T2?> case2)
    {
        switch (union.TypeIndex)
        {
            case 1: case1(union.Is<T1>() ? union.As<T1>() : default); break;
            case 2: case2(union.Is<T2>() ? union.As<T2>() : default); break;
            default: throw new InvalidOperationException("Union is not initialized.");
        }
    }

    /// <summary>
    /// Attempts to match a union containing two possible types against provided case functions and transforms it to a result.
    /// This is a "soft" matching operation where case handlers are optional.
    /// </summary>
    /// <typeparam name="T1">Type of first possible value in the union</typeparam>
    /// <typeparam name="T2">Type of second possible value in the union</typeparam>
    /// <typeparam name="TResult">The type of the result after transformation</typeparam>
    /// <param name="union">The union instance to match against</param>
    /// <param name="case1">Optional function to handle and transform the first type. If null, this case is skipped</param>
    /// <param name="case2">Optional function to handle and transform the second type. If null, this case is skipped</param>
    /// <returns>
    /// The transformed result if a matching non-null case function was found and executed;
    /// otherwise returns default value of <typeparamref name="TResult"/>.
    /// </returns>
    public static TResult? TryMatch<T1, T2, TResult>(
        this Union<T1, T2> union,
        Func<T1?, TResult>? case1 = null,
        Func<T2?, TResult>? case2 = null)
    {
        return union.TypeIndex switch
        {
            1 when case1 != null => case1(union.Is<T1>() ? union.As<T1>() : default),
            2 when case2 != null => case2(union.Is<T2>() ? union.As<T2>() : default),
            _ => default,
        };
    }

    /// <summary>
    /// Tries to invoke the provided actions based on the type of the union instance.
    /// Executes the action corresponding to the contained type within the union if type matching occurs.
    /// </summary>
    /// <typeparam name="T1">The first possible type of the union's contained value.</typeparam>
    /// <typeparam name="T2">The second possible type of the union's contained value.</typeparam>
    /// <param name="union">The union instance whose contained value is to be matched and processed.</param>
    /// <param name="case1">
    /// The action to execute if the union's contained value matches the first type <typeparamref name="T1"/>.
    /// If no match occurs, the action will not be invoked.
    /// </param>
    /// <param name="case2">
    /// The action to execute if the union's contained value matches the second type <typeparamref name="T2"/>.
    /// If no match occurs, the action will not be invoked.
    /// </param>
    public static void TryMatch<T1, T2>(
        this Union<T1, T2> union,
        Action<T1?>? case1 = null,
        Action<T2?>? case2 = null)
    {
        switch (union.TypeIndex)
        {
            case 1 when case1 != null:
                case1(union.Is<T1>() ? union.As<T1>() : default);
                break;
            case 2 when case2 != null:
                case2(union.Is<T2>() ? union.As<T2>() : default);
                break;
        }
    }

    /// <summary>
    /// Matches the current union instance to one of the specified cases and returns a value
    /// determined by the corresponding case function.
    /// </summary>
    /// <typeparam name="T1">The first possible type in the union.</typeparam>
    /// <typeparam name="T2">The second possible type in the union.</typeparam>
    /// <typeparam name="T3">The third possible type in the union.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the case functions.</typeparam>
    /// <param name="union">The union instance containing a value of one of the specified types.</param>
    /// <param name="case1">The function to execute if the union contains a value of type <typeparamref name="T1"/>.</param>
    /// <param name="case2">The function to execute if the union contains a value of type <typeparamref name="T2"/>.</param>
    /// <param name="case3">The function to execute if the union contains a value of type <typeparamref name="T3"/>.</param>
    /// <returns>
    /// A value of type <typeparamref name="TResult"/> produced by the case function corresponding to the union's contained value.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the union is not initialized or if the contained value does not match any of the cases.
    /// </exception>
    public static TResult Match<T1, T2, T3, TResult>(
        this Union<T1, T2, T3> union,
        Func<T1?, TResult> case1,
        Func<T2?, TResult> case2,
        Func<T3?, TResult> case3)
    {
        return union.TypeIndex switch
        {
            1 => case1(union.Is<T1>() ? union.As<T1>() : default),
            2 => case2(union.Is<T2>() ? union.As<T2>() : default),
            3 => case3(union.Is<T3>() ? union.As<T3>() : default),
            _ => throw new InvalidOperationException("Union is not initialized."),
        };
    }

    /// <summary>
    /// Matches the value contained within the union to one of three possible types
    /// and executes a corresponding action for the matched type.
    /// </summary>
    /// <typeparam name="T1">The first possible type of the union.</typeparam>
    /// <typeparam name="T2">The second possible type of the union.</typeparam>
    /// <typeparam name="T3">The third possible type of the union.</typeparam>
    /// <param name="union">The union containing a value of one of the three possible types.</param>
    /// <param name="case1">An action to execute if the value in the union matches the first type.</param>
    /// <param name="case2">An action to execute if the value in the union matches the second type.</param>
    /// <param name="case3">An action to execute if the value in the union matches the third type.</param>
    /// <exception cref="InvalidOperationException">Thrown if the union is not initialized.</exception>
    public static void Match<T1, T2, T3>(
        this Union<T1, T2, T3> union,
        Action<T1?> case1,
        Action<T2?> case2,
        Action<T3?> case3)
    {
        switch (union.TypeIndex)
        {
            case 1: case1(union.Is<T1>() ? union.As<T1>() : default); break;
            case 2: case2(union.Is<T2>() ? union.As<T2>() : default); break;
            case 3: case3(union.Is<T3>() ? union.As<T3>() : default); break;
            default: throw new InvalidOperationException("Union is not initialized.");
        }
    }

    /// <summary>
    /// Attempts to match a union containing two possible types against provided case functions and transforms it to a result.
    /// This is a "soft" matching operation where case handlers are optional.
    /// </summary>
    /// <typeparam name="T1">Type of first possible value in the union</typeparam>
    /// <typeparam name="T2">Type of second possible value in the union</typeparam>
    /// <typeparam name="T3">Type of third possible value (Note: This appears to be unused as union only contains T1 and T2)</typeparam>
    /// <typeparam name="TResult">The type of the result after transformation</typeparam>
    /// <param name="union">The union instance to match against</param>
    /// <param name="case1">Optional function to handle and transform the first type. If null, this case is skipped</param>
    /// <param name="case2">Optional function to handle and transform the second type. If null, this case is skipped</param>
    /// <param name="case3">Optional function to handle and transform the third type. Note: This parameter appears to be unused as union only contains T1 and T2</param>
    /// <returns>
    /// The transformed result if a matching non-null case function was found and executed;
    /// otherwise returns default value of <typeparamref name="TResult"/>.
    /// </returns>
    public static TResult? TryMatch<T1, T2, T3, TResult>(
        this Union<T1, T2> union,
        Func<T1?, TResult>? case1 = null,
        Func<T2?, TResult>? case2 = null,
        Func<T3?, TResult>? case3 = null)
    {
        return union.TypeIndex switch
        {
            1 when case1 != null => case1(union.Is<T1>() ? union.As<T1>() : default),
            2 when case2 != null => case2(union.Is<T2>() ? union.As<T2>() : default),
            3 when case3 != null => case3(union.Is<T3>() ? union.As<T3>() : default),
            _ => default,
        };
    }

    /// <summary>
    /// Attempts to match a union and execute corresponding actions for the matched type.
    /// This is a "soft" matching operation where handlers are optional, allowing selective handling of cases.
    /// </summary>
    /// <typeparam name="T1">Type of first possible value in the union</typeparam>
    /// <typeparam name="T2">Type of second possible value in the union</typeparam>
    /// <typeparam name="T3">Type of third possible value (Note: This appears to be unused as union only contains T1 and T2)</typeparam>
    /// <param name="union">The union instance to match against</param>
    /// <param name="case1">Optional action to execute if the union contains a value of type T1. If null, this case is skipped</param>
    /// <param name="case2">Optional action to execute if the union contains a value of type T2. If null, this case is skipped</param>
    /// <param name="case3">Optional action to execute for T3 type (Note: This parameter appears to be unused as union only contains T1 and T2)</param>
       public static void TryMatch<T1, T2, T3>(
        this Union<T1, T2> union,
        Action<T1?>? case1 = null,
        Action<T2?>? case2 = null,
        Action<T3?>? case3 = null)
    {
        switch (union.TypeIndex)
        {
            case 1 when case1 != null:
                case1(union.Is<T1>() ? union.As<T1>() : default);
                break;
            case 2 when case2 != null:
                case2(union.Is<T2>() ? union.As<T2>() : default);
                break;
            case 2 when case3 != null:
                case3(union.Is<T3>() ? union.As<T3>() : default);
                break;
        }
    }

    /// Matches a union of four possible types and executes one of the provided functions based on the type of the contained value.
    /// <typeparam name="T1">The type of the first possible value in the union.</typeparam>
    /// <typeparam name="T2">The type of the second possible value in the union.</typeparam>
    /// <typeparam name="T3">The type of the third possible value in the union.</typeparam>
    /// <typeparam name="T4">The type of the fourth possible value in the union.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the matching function.</typeparam>
    /// <param name="union">The union containing a value of one of the possible types.</param>
    /// <param name="case1">A function to handle the case where the value is of type <typeparamref name="T1"/>.</param>
    /// <param name="case2">A function to handle the case where the value is of type <typeparamref name="T2"/>.</param>
    /// <param name="case3">A function to handle the case where the value is of type <typeparamref name="T3"/>.</param>
    /// <param name="case4">A function to handle the case where the value is of type <typeparamref name="T4"/>.</param>
    /// <returns>The result of invoking the matching function for the type of the contained value in the union. Throws an exception if the union is uninitialized.</returns>
    public static TResult Match<T1, T2, T3, T4, TResult>(
        this Union<T1, T2, T3, T4> union,
        Func<T1?, TResult> case1,
        Func<T2?, TResult> case2,
        Func<T3?, TResult> case3,
        Func<T4?, TResult> case4)
    {
        return union.TypeIndex switch
        {
            1 => case1(union.Is<T1>() ? union.As<T1>() : default),
            2 => case2(union.Is<T2>() ? union.As<T2>() : default),
            3 => case3(union.Is<T3>() ? union.As<T3>() : default),
            4 => case4(union.Is<T4>() ? union.As<T4>() : default),
            _ => throw new InvalidOperationException("Union is not initialized."),
        };
    }

    /// Matches the current instance of the union against one of the four possible cases
    /// and executes the corresponding action for the matched case.
    /// <typeparam name="T1">The type for the first case.</typeparam>
    /// <typeparam name="T2">The type for the second case.</typeparam>
    /// <typeparam name="T3">The type for the third case.</typeparam>
    /// <typeparam name="T4">The type for the fourth case.</typeparam>
    /// <param name="union">The union instance to match.</param>
    /// <param name="case1">The action to execute if the first case is matched.</param>
    /// <param name="case2">The action to execute if the second case is matched.</param>
    /// <param name="case3">The action to execute if the third case is matched.</param>
    /// <param name="case4">The action to execute if the fourth case is matched.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the union is not initialized.
    /// </exception>
    public static void Match<T1, T2, T3, T4>(
        this Union<T1, T2, T3, T4> union,
        Action<T1?> case1,
        Action<T2?> case2,
        Action<T3?> case3,
        Action<T4?> case4)
    {
        switch (union.TypeIndex)
        {
            case 1: case1(union.Is<T1>() ? union.As<T1>() : default); break;
            case 2: case2(union.Is<T2>() ? union.As<T2>() : default); break;
            case 3: case3(union.Is<T3>() ? union.As<T3>() : default); break;
            case 4: case4(union.Is<T4>() ? union.As<T4>() : default); break;
            default: throw new InvalidOperationException("Union is not initialized.");
        }
    }

    /// <summary>
    /// Attempts to match the union's value against provided case functions and transforms it to a result.
    /// This is a "soft" matching operation where unmatched cases are allowed (can be null).
    /// </summary>
    /// <typeparam name="T1">Type of first possible value in the union</typeparam>
    /// <typeparam name="T2">Type of second possible value in the union</typeparam>
    /// <typeparam name="T3">Type of third possible value in the union</typeparam>
    /// <typeparam name="T4">Type of fourth possible value in the union</typeparam>
    /// <typeparam name="TResult">The type of the result after transformation</typeparam>
    /// <param name="union">The union instance to match against</param>
    /// <param name="case1">Optional function to handle and transform the first type. If null, this case is skipped</param>
    /// <param name="case2">Optional function to handle and transform the second type. If null, this case is skipped</param>
    /// <param name="case3">Optional function to handle and transform the third type. If null, this case is skipped</param>
    /// <param name="case4">Optional function to handle and transform the fourth type. If null, this case is skipped</param>
    /// <returns>
    /// The transformed result if a matching non-null case function was found and executed;
    /// otherwise returns default value of <typeparamref name="TResult"/>.
    /// </returns>
    /// <remarks>
    /// Unlike the regular Match method, TryMatch allows for partial matching where not all cases need to be handled.
    /// If the union's value corresponds to a case where the handler is null, the method returns default(TResult).
    /// This is useful when you only care about specific cases and want to ignore others.
    /// </remarks>
    public static TResult? TryMatch<T1, T2, T3, T4, TResult>(
        this Union<T1, T2, T3, T4> union,
        Func<T1?, TResult>? case1 = null,
        Func<T2?, TResult>? case2 = null,
        Func<T3?, TResult>? case3 = null,
        Func<T4?, TResult>? case4 = null)
    {
        return union.TypeIndex switch
        {
            1 when case1 != null => case1(union.Is<T1>() ? union.As<T1>() : default),
            2 when case2 != null => case2(union.Is<T2>() ? union.As<T2>() : default),
            3 when case3 != null => case3(union.Is<T3>() ? union.As<T3>() : default),
            4 when case4 != null => case4(union.Is<T4>() ? union.As<T4>() : default),
            _ => default,
        };
    }

    /// Attempts to match the current union instance with a specific type and, if successful, executes the corresponding action.
    /// <typeparam name="T1">The first type in the union.</typeparam>
    /// <typeparam name="T2">The second type in the union.</typeparam>
    /// <typeparam name="T3">The third type in the union.</typeparam>
    /// <typeparam name="T4">The fourth type in the union.</typeparam>
    /// <param name="union">The union instance containing values of types T1, T2, T3, or T4.</param>
    /// <param name="case1">An action to execute if the union contains a value of type T1.</param>
    /// <param name="case2">An action to execute if the union contains a value of type T2.</param>
    /// <param name="case3">An action to execute if the union contains a value of type T3.</param>
    /// <param name="case4">An action to execute if the union contains a value of type T4.</param>
    public static void TryMatch<T1, T2, T3, T4>(
        this Union<T1, T2, T3, T4> union,
        Action<T1?>? case1 = null,
        Action<T2?>? case2 = null,
        Action<T3?>? case3 = null,
        Action<T4?>? case4 = null)
    {
        switch (union.TypeIndex)
        {
            case 1 when case1 != null:
                case1(union.Is<T1>() ? union.As<T1>() : default);
                break;
            case 2 when case2 != null:
                case2(union.Is<T2>() ? union.As<T2>() : default);
                break;
            case 3 when case3 != null:
                case3(union.Is<T3>() ? union.As<T3>() : default);
                break;
            case 4 when case4 != null:
                case4(union.Is<T4>() ? union.As<T4>() : default);
                break;
        }
    }

    /// Matches the value contained in a union of five possible types to one of the provided cases
    /// and returns a result based on the matched case.
    /// <typeparam name="T1">The type of the first possible case.</typeparam>
    /// <typeparam name="T2">The type of the second possible case.</typeparam>
    /// <typeparam name="T3">The type of the third possible case.</typeparam>
    /// <typeparam name="T4">The type of the fourth possible case.</typeparam>
    /// <typeparam name="T5">The type of the fifth possible case.</typeparam>
    /// <typeparam name="TResult">The return type of the matching function.</typeparam>
    /// <param name="union">The union object containing a value of one of the five possible types.</param>
    /// <param name="case1">The function to handle the first type of the union.</param>
    /// <param name="case2">The function to handle the second type of the union.</param>
    /// <param name="case3">The function to handle the third type of the union.</param>
    /// <param name="case4">The function to handle the fourth type of the union.</param>
    /// <param name="case5">The function to handle the fifth type of the union.</param>
    /// <returns>The result of the function corresponding to the matched case.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the union is not initialized.</exception>
    public static TResult Match<T1, T2, T3, T4, T5, TResult>(
        this Union<T1, T2, T3, T4, T5> union,
        Func<T1?, TResult> case1,
        Func<T2?, TResult> case2,
        Func<T3?, TResult> case3,
        Func<T4?, TResult> case4,
        Func<T5?, TResult> case5)
    {
        return union.TypeIndex switch
        {
            1 => case1(union.Is<T1>() ? union.As<T1>() : default),
            2 => case2(union.Is<T2>() ? union.As<T2>() : default),
            3 => case3(union.Is<T3>() ? union.As<T3>() : default),
            4 => case4(union.Is<T4>() ? union.As<T4>() : default),
            5 => case5(union.Is<T5>() ? union.As<T5>() : default),
            _ => throw new InvalidOperationException("Union is not initialized."),
        };
    }

    /// <summary>
    /// Executes one of the provided actions based on the runtime type of the value contained in the union.
    /// </summary>
    /// <typeparam name="T1">The type of the first possible case.</typeparam>
    /// <typeparam name="T2">The type of the second possible case.</typeparam>
    /// <typeparam name="T3">The type of the third possible case.</typeparam>
    /// <typeparam name="T4">The type of the fourth possible case.</typeparam>
    /// <typeparam name="T5">The type of the fifth possible case.</typeparam>
    /// <param name="union">The union containing the value to match.</param>
    /// <param name="case1">The action to execute if the value is of type <typeparamref name="T1"/>.</param>
    /// <param name="case2">The action to execute if the value is of type <typeparamref name="T2"/>.</param>
    /// <param name="case3">The action to execute if the value is of type <typeparamref name="T3"/>.</param>
    /// <param name="case4">The action to execute if the value is of type <typeparamref name="T4"/>.</param>
    /// <param name="case5">The action to execute if the value is of type <typeparamref name="T5"/>.</param>
    /// <exception cref="InvalidOperationException">Thrown if the union is not properly initialized.</exception>
    public static void Match<T1, T2, T3, T4, T5>(
        this Union<T1, T2, T3, T4, T5> union,
        Action<T1?> case1,
        Action<T2?> case2,
        Action<T3?> case3,
        Action<T4?> case4,
        Action<T5?> case5)
    {
        switch (union.TypeIndex)
        {
            case 1: case1(union.Is<T1>() ? union.As<T1>() : default); break;
            case 2: case2(union.Is<T2>() ? union.As<T2>() : default); break;
            case 3: case3(union.Is<T3>() ? union.As<T3>() : default); break;
            case 4: case4(union.Is<T4>() ? union.As<T4>() : default); break;
            case 5: case5(union.Is<T5>() ? union.As<T5>() : default); break;
            default: throw new InvalidOperationException("Union is not initialized.");
        }
    }


    /// <summary>
    /// Attempts to match the value of the union instance to one of the possible case types, and executes the corresponding function, if provided.
    /// </summary>
    /// <typeparam name="T1">The first possible type for the union's value.</typeparam>
    /// <typeparam name="T2">The second possible type for the union's value.</typeparam>
    /// <typeparam name="T3">The third possible type for the union's value.</typeparam>
    /// <typeparam name="T4">The fourth possible type for the union's value.</typeparam>
    /// <typeparam name="T5">The fifth possible type for the union's value.</typeparam>
    /// <typeparam name="TResult">The result type of the function to be returned.</typeparam>
    /// <param name="union">The union instance containing the value to attempt matching.</param>
    /// <param name="case1">
    /// An optional function to execute if the union's value is of type <typeparamref name="T1"/>.
    /// </param>
    /// <param name="case2">
    /// An optional function to execute if the union's value is of type <typeparamref name="T2"/>.
    /// </param>
    /// <param name="case3">
    /// An optional function to execute if the union's value is of type <typeparamref name="T3"/>.
    /// </param>
    /// <param name="case4">
    /// An optional function to execute if the union's value is of type <typeparamref name="T4"/>.
    /// </param>
    /// <param name="case5">
    /// An optional function to execute if the union's value is of type <typeparamref name="T5"/>.
    /// </param>
    /// <returns>
    /// The result produced by the executed function if a match is found. If no match is found or no function is provided for the matching type, returns the default value of <typeparamref name="TResult"/>.
    /// </returns>
    public static TResult? TryMatch<T1, T2, T3, T4, T5, TResult>(
        this Union<T1, T2, T3, T4, T5> union,
        Func<T1?, TResult>? case1 = null,
        Func<T2?, TResult>? case2 = null,
        Func<T3?, TResult>? case3 = null,
        Func<T4?, TResult>? case4 = null,
        Func<T5?, TResult>? case5 = null)
    {
        return union.TypeIndex switch
        {
            1 when case1 != null => case1(union.Is<T1>() ? union.As<T1>() : default),
            2 when case2 != null => case2(union.Is<T2>() ? union.As<T2>() : default),
            3 when case3 != null => case3(union.Is<T3>() ? union.As<T3>() : default),
            4 when case4 != null => case4(union.Is<T4>() ? union.As<T4>() : default),
            5 when case5 != null => case5(union.Is<T5>() ? union.As<T5>() : default),
            _ => default,
        };
    }


    /// <summary>
    /// Attempts to match the union's value to one of five possible types and invokes the corresponding action
    /// if the match is successful.
    /// </summary>
    /// <typeparam name="T1">The first possible type.</typeparam>
    /// <typeparam name="T2">The second possible type.</typeparam>
    /// <typeparam name="T3">The third possible type.</typeparam>
    /// <typeparam name="T4">The fourth possible type.</typeparam>
    /// <typeparam name="T5">The fifth possible type.</typeparam>
    /// <param name="union">The union instance to be matched.</param>
    /// <param name="case1">
    /// The action to invoke if the union's value matches the first type <typeparamref name="T1"/>.
    /// This parameter is optional and can be null.
    /// </param>
    /// <param name="case2">
    /// The action to invoke if the union's value matches the second type <typeparamref name="T2"/>.
    /// This parameter is optional and can be null.
    /// </param>
    /// <param name="case3">
    /// The action to invoke if the union's value matches the third type <typeparamref name="T3"/>.
    /// This parameter is optional and can be null.
    /// </param>
    /// <param name="case4">
    /// The action to invoke if the union's value matches the fourth type <typeparamref name="T4"/>.
    /// This parameter is optional and can be null.
    /// </param>
    /// <param name="case5">
    /// The action to invoke if the union's value matches the fifth type <typeparamref name="T5"/>.
    /// This parameter is optional and can be null.
    /// </param>
    public static void TryMatch<T1, T2, T3, T4, T5>(
        this Union<T1, T2, T3, T4, T5> union,
        Action<T1?>? case1 = null,
        Action<T2?>? case2 = null,
        Action<T3?>? case3 = null,
        Action<T4?>? case4 = null,
        Action<T5?>? case5 = null)
    {
        switch (union.TypeIndex)
        {
            case 1 when case1 != null:
                case1(union.Is<T1>() ? union.As<T1>() : default);
                break;
            case 2 when case2 != null:
                case2(union.Is<T2>() ? union.As<T2>() : default);
                break;
            case 3 when case3 != null:
                case3(union.Is<T3>() ? union.As<T3>() : default);
                break;
            case 4 when case4 != null:
                case4(union.Is<T4>() ? union.As<T4>() : default);
                break;
            case 5 when case5 != null:
                case5(union.Is<T5>() ? union.As<T5>() : default);
                break;
        }
    }


    /// <summary>
    /// Matches the value contained in a six-case union to one of the specified cases
    /// and returns the result from the corresponding case function.
    /// </summary>
    /// <typeparam name="T1">The type of the first possible value in the union.</typeparam>
    /// <typeparam name="T2">The type of the second possible value in the union.</typeparam>
    /// <typeparam name="T3">The type of the third possible value in the union.</typeparam>
    /// <typeparam name="T4">The type of the fourth possible value in the union.</typeparam>
    /// <typeparam name="T5">The type of the fifth possible value in the union.</typeparam>
    /// <typeparam name="T6">The type of the sixth possible value in the union.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the case functions.</typeparam>
    /// <param name="union">The union instance to be matched against one of the cases.</param>
    /// <param name="case1">The function to be invoked if the value matches the first type <typeparamref name="T1"/>.</param>
    /// <param name="case2">The function to be invoked if the value matches the second type <typeparamref name="T2"/>.</param>
    /// <param name="case3">The function to be invoked if the value matches the third type <typeparamref name="T3"/>.</param>
    /// <param name="case4">The function to be invoked if the value matches the fourth type <typeparamref name="T4"/>.</param>
    /// <param name="case5">The function to be invoked if the value matches the fifth type <typeparamref name="T5"/>.</param>
    /// <param name="case6">The function to be invoked if the value matches the sixth type <typeparamref name="T6"/>.</param>
    /// <returns>The result of invoking the function for the matched type.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the union is not properly initialized.</exception>
    public static TResult Match<T1, T2, T3, T4, T5, T6, TResult>(
        this Union<T1, T2, T3, T4, T5, T6> union,
        Func<T1?, TResult> case1,
        Func<T2?, TResult> case2,
        Func<T3?, TResult> case3,
        Func<T4?, TResult> case4,
        Func<T5?, TResult> case5,
        Func<T6?, TResult> case6)
    {
        return union.TypeIndex switch
        {
            1 => case1(union.Is<T1>() ? union.As<T1>() : default),
            2 => case2(union.Is<T2>() ? union.As<T2>() : default),
            3 => case3(union.Is<T3>() ? union.As<T3>() : default),
            4 => case4(union.Is<T4>() ? union.As<T4>() : default),
            5 => case5(union.Is<T5>() ? union.As<T5>() : default),
            6 => case6(union.Is<T6>() ? union.As<T6>() : default),
            _ => throw new InvalidOperationException("Union is not initialized."),
        };
    }

    /// Matches an instance of a union with one of six possible types and executes the corresponding action
    /// based on the type contained within the union.
    /// <typeparam name="T1">The first possible type of the union.</typeparam>
    /// <typeparam name="T2">The second possible type of the union.</typeparam>
    /// <typeparam name="T3">The third possible type of the union.</typeparam>
    /// <typeparam name="T4">The fourth possible type of the union.</typeparam>
    /// <typeparam name="T5">The fifth possible type of the union.</typeparam>
    /// <typeparam name="T6">The sixth possible type of the union.</typeparam>
    /// <param name="union">The union instance containing one of six possible types.</param>
    /// <param name="case1">The action to execute if the union contains a value of type T1.</param>
    /// <param name="case2">The action to execute if the union contains a value of type T2.</param>
    /// <param name="case3">The action to execute if the union contains a value of type T3.</param>
    /// <param name="case4">The action to execute if the union contains a value of type T4.</param>
    /// <param name="case5">The action to execute if the union contains a value of type T5.</param>
    /// <param name="case6">The action to execute if the union contains a value of type T6.</param>
    /// <exception cref="InvalidOperationException">Thrown when the union is not properly initialized.</exception>
    public static void Match<T1, T2, T3, T4, T5, T6>(
        this Union<T1, T2, T3, T4, T5, T6> union,
        Action<T1?> case1,
        Action<T2?> case2,
        Action<T3?> case3,
        Action<T4?> case4,
        Action<T5?> case5,
        Action<T6?> case6)
    {
        switch (union.TypeIndex)
        {
            case 1: case1(union.Is<T1>() ? union.As<T1>() : default); break;
            case 2: case2(union.Is<T2>() ? union.As<T2>() : default); break;
            case 3: case3(union.Is<T3>() ? union.As<T3>() : default); break;
            case 4: case4(union.Is<T4>() ? union.As<T4>() : default); break;
            case 5: case5(union.Is<T5>() ? union.As<T5>() : default); break;
            case 6: case6(union.Is<T6>() ? union.As<T6>() : default); break;
            default: throw new InvalidOperationException("Union is not initialized.");
        }
    }


    /// <summary>
    /// Attempts to match a union containing six possible types against provided case functions and transforms it to a result.
    /// This is a "soft" matching operation where case handlers are optional.
    /// </summary>
    /// <typeparam name="T1">Type of first possible value in the union</typeparam>
    /// <typeparam name="T2">Type of second possible value in the union</typeparam>
    /// <typeparam name="T3">Type of third possible value in the union</typeparam>
    /// <typeparam name="T4">Type of fourth possible value in the union</typeparam>
    /// <typeparam name="T5">Type of fifth possible value in the union</typeparam>
    /// <typeparam name="T6">Type of sixth possible value in the union</typeparam>
    /// <typeparam name="TResult">The type of the result after transformation</typeparam>
    /// <param name="union">The union instance to match against</param>
    /// <param name="case1">Optional function to handle and transform the first type. If null, this case is skipped</param>
    /// <param name="case2">Optional function to handle and transform the second type. If null, this case is skipped</param>
    /// <param name="case3">Optional function to handle and transform the third type. If null, this case is skipped</param>
    /// <param name="case4">Optional function to handle and transform the fourth type. If null, this case is skipped</param>
    /// <param name="case5">Optional function to handle and transform the fifth type. If null, this case is skipped</param>
    /// <param name="case6">Optional function to handle and transform the sixth type. If null, this case is skipped</param>
    /// <returns>
    /// The transformed result if a matching non-null case function was found and executed;
    /// otherwise returns default value of <typeparamref name="TResult"/>.
    /// </returns>
    public static TResult? TryMatch<T1, T2, T3, T4, T5, T6, TResult>(
        this Union<T1, T2, T3, T4, T5, T6> union,
        Func<T1?, TResult>? case1 = null,
        Func<T2?, TResult>? case2 = null,
        Func<T3?, TResult>? case3 = null,
        Func<T4?, TResult>? case4 = null,
        Func<T5?, TResult>? case5 = null,
        Func<T6?, TResult>? case6 = null)
    {
        return union.TypeIndex switch
        {
            1 when case1 != null => case1(union.Is<T1>() ? union.As<T1>() : default),
            2 when case2 != null => case2(union.Is<T2>() ? union.As<T2>() : default),
            3 when case3 != null => case3(union.Is<T3>() ? union.As<T3>() : default),
            4 when case4 != null => case4(union.Is<T4>() ? union.As<T4>() : default),
            5 when case5 != null => case5(union.Is<T5>() ? union.As<T5>() : default),
            6 when case6 != null => case6(union.Is<T6>() ? union.As<T6>() : default),
            _ => default,
        };
    }


    /// <summary>
    /// Attempts to match the value contained within a union instance against a specified type
    /// and invokes the corresponding action for the matched type if provided.
    /// </summary>
    /// <typeparam name="T1">The first type that the union instance may contain.</typeparam>
    /// <typeparam name="T2">The second type that the union instance may contain.</typeparam>
    /// <typeparam name="T3">The third type that the union instance may contain.</typeparam>
    /// <typeparam name="T4">The fourth type that the union instance may contain.</typeparam>
    /// <typeparam name="T5">The fifth type that the union instance may contain.</typeparam>
    /// <typeparam name="T6">The sixth type that the union instance may contain.</typeparam>
    /// <param name="union">The union instance whose value is to be matched and processed.</param>
    /// <param name="case1">An optional action to invoke if the union value matches the first type <typeparamref name="T1"/>.</param>
    /// <param name="case2">An optional action to invoke if the union value matches the second type <typeparamref name="T2"/>.</param>
    /// <param name="case3">An optional action to invoke if the union value matches the third type <typeparamref name="T3"/>.</param>
    /// <param name="case4">An optional action to invoke if the union value matches the fourth type <typeparamref name="T4"/>.</param>
    /// <param name="case5">An optional action to invoke if the union value matches the fifth type <typeparamref name="T5"/>.</param>
    /// <param name="case6">An optional action to invoke if the union value matches the sixth type <typeparamref name="T6"/>.</param>
    public static void TryMatch<T1, T2, T3, T4, T5, T6>(
        this Union<T1, T2, T3, T4, T5, T6> union,
        Action<T1?>? case1 = null,
        Action<T2?>? case2 = null,
        Action<T3?>? case3 = null,
        Action<T4?>? case4 = null,
        Action<T5?>? case5 = null,
        Action<T6?>? case6 = null)
    {
        switch (union.TypeIndex)
        {
            case 1 when case1 != null:
                case1(union.Is<T1>() ? union.As<T1>() : default);
                break;
            case 2 when case2 != null:
                case2(union.Is<T2>() ? union.As<T2>() : default);
                break;
            case 3 when case3 != null:
                case3(union.Is<T3>() ? union.As<T3>() : default);
                break;
            case 4 when case4 != null:
                case4(union.Is<T4>() ? union.As<T4>() : default);
                break;
            case 5 when case5 != null:
                case5(union.Is<T5>() ? union.As<T5>() : default);
                break;
            case 6 when case6 != null:
                case6(union.Is<T6>() ? union.As<T6>() : default);
                break;
        }
    }

    /// Matches a union with seven potential types to a result by executing the appropriate function
    /// for the contained value based on its type.
    /// <typeparam name="T1">The type of the first possible value.</typeparam>
    /// <typeparam name="T2">The type of the second possible value.</typeparam>
    /// <typeparam name="T3">The type of the third possible value.</typeparam>
    /// <typeparam name="T4">The type of the fourth possible value.</typeparam>
    /// <typeparam name="T5">The type of the fifth possible value.</typeparam>
    /// <typeparam name="T6">The type of the sixth possible value.</typeparam>
    /// <typeparam name="T7">The type of the seventh possible value.</typeparam>
    /// <typeparam name="TResult">The result type of the matching function.</typeparam>
    /// <param name="union">The union instance to match.</param>
    /// <param name="case1">The function to execute if the union contains a value of type <typeparamref name="T1"/>.</param>
    /// <param name="case2">The function to execute if the union contains a value of type <typeparamref name="T2"/>.</param>
    /// <param name="case3">The function to execute if the union contains a value of type <typeparamref name="T3"/>.</param>
    /// <param name="case4">The function to execute if the union contains a value of type <typeparamref name="T4"/>.</param>
    /// <param name="case5">The function to execute if the union contains a value of type <typeparamref name="T5"/>.</param>
    /// <param name="case6">The function to execute if the union contains a value of type <typeparamref name="T6"/>.</param>
    /// <param name="case7">The function to execute if the union contains a value of type <typeparamref name="T7"/>.</param>
    /// <returns>The result of the function corresponding to the type of the union's contained value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the union is not initialized.</exception>
    public static TResult Match<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Union<T1, T2, T3, T4, T5, T6, T7> union,
        Func<T1?, TResult> case1,
        Func<T2?, TResult> case2,
        Func<T3?, TResult> case3,
        Func<T4?, TResult> case4,
        Func<T5?, TResult> case5,
        Func<T6?, TResult> case6,
        Func<T7?, TResult> case7)
    {
        return union.TypeIndex switch
        {
            1 => case1(union.Is<T1>() ? union.As<T1>() : default),
            2 => case2(union.Is<T2>() ? union.As<T2>() : default),
            3 => case3(union.Is<T3>() ? union.As<T3>() : default),
            4 => case4(union.Is<T4>() ? union.As<T4>() : default),
            5 => case5(union.Is<T5>() ? union.As<T5>() : default),
            6 => case6(union.Is<T6>() ? union.As<T6>() : default),
            7 => case7(union.Is<T7>() ? union.As<T7>() : default),
            _ => throw new InvalidOperationException("Union is not initialized."),
        };
    }

    /// Matches the value of the union to one of the specified cases and executes the respective action.
    /// Throws an InvalidOperationException if the union is not initialized.
    /// <typeparam name="T1">The type of the first value in the union.</typeparam>
    /// <typeparam name="T2">The type of the second value in the union.</typeparam>
    /// <typeparam name="T3">The type of the third value in the union.</typeparam>
    /// <typeparam name="T4">The type of the fourth value in the union.</typeparam>
    /// <typeparam name="T5">The type of the fifth value in the union.</typeparam>
    /// <typeparam name="T6">The type of the sixth value in the union.</typeparam>
    /// <typeparam name="T7">The type of the seventh value in the union.</typeparam>
    /// <param name="union">The union instance to be matched.</param>
    /// <param name="case1">The action to be executed for the first type in the union.</param>
    /// <param name="case2">The action to be executed for the second type in the union.</param>
    /// <param name="case3">The action to be executed for the third type in the union.</param>
    /// <param name="case4">The action to be executed for the fourth type in the union.</param>
    /// <param name="case5">The action to be executed for the fifth type in the union.</param>
    /// <param name="case6">The action to be executed for the sixth type in the union.</param>
    /// <param name="case7">The action to be executed for the seventh type in the union.</param>
    public static void Match<T1, T2, T3, T4, T5, T6, T7>(
        this Union<T1, T2, T3, T4, T5, T6, T7> union,
        Action<T1?> case1,
        Action<T2?> case2,
        Action<T3?> case3,
        Action<T4?> case4,
        Action<T5?> case5,
        Action<T6?> case6,
        Action<T7?> case7)
    {
        switch (union.TypeIndex)
        {
            case 1: case1(union.Is<T1>() ? union.As<T1>() : default); break;
            case 2: case2(union.Is<T2>() ? union.As<T2>() : default); break;
            case 3: case3(union.Is<T3>() ? union.As<T3>() : default); break;
            case 4: case4(union.Is<T4>() ? union.As<T4>() : default); break;
            case 5: case5(union.Is<T5>() ? union.As<T5>() : default); break;
            case 6: case6(union.Is<T6>() ? union.As<T6>() : default); break;
            case 7: case7(union.Is<T7>() ? union.As<T7>() : default); break;
            default: throw new InvalidOperationException("Union is not initialized.");
        }
    }

    /// <summary>
    /// Attempts to match a union containing seven possible types against provided case functions and transforms it to a result.
    /// This is a "soft" matching operation where case handlers are optional.
    /// </summary>
    /// <typeparam name="T1">Type of first possible value in the union</typeparam>
    /// <typeparam name="T2">Type of second possible value in the union</typeparam>
    /// <typeparam name="T3">Type of third possible value in the union</typeparam>
    /// <typeparam name="T4">Type of fourth possible value in the union</typeparam>
    /// <typeparam name="T5">Type of fifth possible value in the union</typeparam>
    /// <typeparam name="T6">Type of sixth possible value in the union</typeparam>
    /// <typeparam name="T7">Type of seventh possible value in the union</typeparam>
    /// <typeparam name="TResult">The type of the result after transformation</typeparam>
    /// <param name="union">The union instance to match against</param>
    /// <param name="case1">Optional function to handle and transform the first type. If null, this case is skipped</param>
    /// <param name="case2">Optional function to handle and transform the second type. If null, this case is skipped</param>
    /// <param name="case3">Optional function to handle and transform the third type. If null, this case is skipped</param>
    /// <param name="case4">Optional function to handle and transform the fourth type. If null, this case is skipped</param>
    /// <param name="case5">Optional function to handle and transform the fifth type. If null, this case is skipped</param>
    /// <param name="case6">Optional function to handle and transform the sixth type. If null, this case is skipped</param>
    /// <param name="case7">Optional function to handle and transform the seventh type. If null, this case is skipped</param>
    /// <returns>
    /// The transformed result if a matching non-null case function was found and executed;
    /// otherwise returns default value of <typeparamref name="TResult"/>.
    /// </returns>
    public static TResult? TryMatch<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Union<T1, T2, T3, T4, T5, T6, T7> union,
        Func<T1?, TResult>? case1 = null,
        Func<T2?, TResult>? case2 = null,
        Func<T3?, TResult>? case3 = null,
        Func<T4?, TResult>? case4 = null,
        Func<T5?, TResult>? case5 = null,
        Func<T6?, TResult>? case6 = null,
        Func<T7?, TResult>? case7 = null)
    {
        return union.TypeIndex switch
        {
            1 when case1 != null => case1(union.Is<T1>() ? union.As<T1>() : default),
            2 when case2 != null => case2(union.Is<T2>() ? union.As<T2>() : default),
            3 when case3 != null => case3(union.Is<T3>() ? union.As<T3>() : default),
            4 when case4 != null => case4(union.Is<T4>() ? union.As<T4>() : default),
            5 when case5 != null => case5(union.Is<T5>() ? union.As<T5>() : default),
            6 when case6 != null => case6(union.Is<T6>() ? union.As<T6>() : default),
            7 when case7 != null => case7(union.Is<T7>() ? union.As<T7>() : default),
            _ => default,
        };
    }

    /// <summary>
    /// Attempts to match the provided union instance to one of its contained types and execute an associated action
    /// if the match is successful.
    /// </summary>
    /// <typeparam name="T1">The first possible type contained in the union.</typeparam>
    /// <typeparam name="T2">The second possible type contained in the union.</typeparam>
    /// <typeparam name="T3">The third possible type contained in the union.</typeparam>
    /// <typeparam name="T4">The fourth possible type contained in the union.</typeparam>
    /// <typeparam name="T5">The fifth possible type contained in the union.</typeparam>
    /// <typeparam name="T6">The sixth possible type contained in the union.</typeparam>
    /// <typeparam name="T7">The seventh possible type contained in the union.</typeparam>
    /// <param name="union">The union instance containing a value of one of the specified types.</param>
    /// <param name="case1">An optional action to be executed if the union contains a value of type <typeparamref name="T1"/>.</param>
    /// <param name="case2">An optional action to be executed if the union contains a value of type <typeparamref name="T2"/>.</param>
    /// <param name="case3">An optional action to be executed if the union contains a value of type <typeparamref name="T3"/>.</param>
    /// <param name="case4">An optional action to be executed if the union contains a value of type <typeparamref name="T4"/>.</param>
    /// <param name="case5">An optional action to be executed if the union contains a value of type <typeparamref name="T5"/>.</param>
    /// <param name="case6">An optional action to be executed if the union contains a value of type <typeparamref name="T6"/>.</param>
    /// <param name="case7">An optional action to be executed if the union contains a value of type <typeparamref name="T7"/>.</param>
    public static void TryMatch<T1, T2, T3, T4, T5, T6, T7>(
        this Union<T1, T2, T3, T4, T5, T6, T7> union,
        Action<T1?>? case1 = null,
        Action<T2?>? case2 = null,
        Action<T3?>? case3 = null,
        Action<T4?>? case4 = null,
        Action<T5?>? case5 = null,
        Action<T6?>? case6 = null,
        Action<T7?>? case7 = null)
    {
        switch (union.TypeIndex)
        {
            case 1 when case1 != null:
                case1(union.Is<T1>() ? union.As<T1>() : default);
                break;
            case 2 when case2 != null:
                case2(union.Is<T2>() ? union.As<T2>() : default);
                break;
            case 3 when case3 != null:
                case3(union.Is<T3>() ? union.As<T3>() : default);
                break;
            case 4 when case4 != null:
                case4(union.Is<T4>() ? union.As<T4>() : default);
                break;
            case 5 when case5 != null:
                case5(union.Is<T5>() ? union.As<T5>() : default);
                break;
            case 6 when case6 != null:
                case6(union.Is<T6>() ? union.As<T6>() : default);
                break;
            case 7 when case7 != null:
                case7(union.Is<T7>() ? union.As<T7>() : default);
                break;
        }
    }

    /// <summary>
    /// Matches the current value of the union against the provided cases
    /// and returns the result of the matched case.
    /// </summary>
    /// <typeparam name="T1">The type of the first possible value in the union.</typeparam>
    /// <typeparam name="T2">The type of the second possible value in the union.</typeparam>
    /// <typeparam name="T3">The type of the third possible value in the union.</typeparam>
    /// <typeparam name="T4">The type of the fourth possible value in the union.</typeparam>
    /// <typeparam name="T5">The type of the fifth possible value in the union.</typeparam>
    /// <typeparam name="T6">The type of the sixth possible value in the union.</typeparam>
    /// <typeparam name="T7">The type of the seventh possible value in the union.</typeparam>
    /// <typeparam name="T8">The type of the eighth possible value in the union.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the matching case.</typeparam>
    /// <param name="union">The union to match against the provided cases.</param>
    /// <param name="case1">The function to be invoked if the first type matches.</param>
    /// <param name="case2">The function to be invoked if the second type matches.</param>
    /// <param name="case3">The function to be invoked if the third type matches.</param>
    /// <param name="case4">The function to be invoked if the fourth type matches.</param>
    /// <param name="case5">The function to be invoked if the fifth type matches.</param>
    /// <param name="case6">The function to be invoked if the sixth type matches.</param>
    /// <param name="case7">The function to be invoked if the seventh type matches.</param>
    /// <param name="case8">The function to be invoked if the eighth type matches.</param>
    /// <returns>The result of the function associated with the matched case.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the union is not initialized.</exception>
    public static TResult Match<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        this Union<T1, T2, T3, T4, T5, T6, T7, T8> union,
        Func<T1?, TResult> case1,
        Func<T2?, TResult> case2,
        Func<T3?, TResult> case3,
        Func<T4?, TResult> case4,
        Func<T5?, TResult> case5,
        Func<T6?, TResult> case6,
        Func<T7?, TResult> case7,
        Func<T8?, TResult> case8)
    {
        return union.TypeIndex switch
        {
            1 => case1(union.Is<T1>() ? union.As<T1>() : default),
            2 => case2(union.Is<T2>() ? union.As<T2>() : default),
            3 => case3(union.Is<T3>() ? union.As<T3>() : default),
            4 => case4(union.Is<T4>() ? union.As<T4>() : default),
            5 => case5(union.Is<T5>() ? union.As<T5>() : default),
            6 => case6(union.Is<T6>() ? union.As<T6>() : default),
            7 => case7(union.Is<T7>() ? union.As<T7>() : default),
            8 => case8(union.Is<T8>() ? union.As<T8>() : default),
            _ => throw new InvalidOperationException("Union is not initialized."),
        };
    }

    /// Evaluates the value stored in a union of up to eight possible types and executes the action corresponding to the actual type of the union's value.
    /// Throws an InvalidOperationException if the union is not initialized.
    /// <param name="union">The union containing a value of one of the specified types.</param>
    /// <param name="case1">The action to execute if the value matches type <typeparamref name="T1"/>.</param>
    /// <param name="case2">The action to execute if the value matches type <typeparamref name="T2"/>.</param>
    /// <param name="case3">The action to execute if the value matches type <typeparamref name="T3"/>.</param>
    /// <param name="case4">The action to execute if the value matches type <typeparamref name="T4"/>.</param>
    /// <param name="case5">The action to execute if the value matches type <typeparamref name="T5"/>.</param>
    /// <param name="case6">The action to execute if the value matches type <typeparamref name="T6"/>.</param>
    /// <param name="case7">The action to execute if the value matches type <typeparamref name="T7"/>.</param>
    /// <param name="case8">The action to execute if the value matches type <typeparamref name="T8"/>.</param>
    public static void Match<T1, T2, T3, T4, T5, T6, T7, T8>(
        this Union<T1, T2, T3, T4, T5, T6, T7, T8> union,
        Action<T1?> case1,
        Action<T2?> case2,
        Action<T3?> case3,
        Action<T4?> case4,
        Action<T5?> case5,
        Action<T6?> case6,
        Action<T7?> case7,
        Action<T8?> case8)
    {
        switch (union.TypeIndex)
        {
            case 1: case1(union.Is<T1>() ? union.As<T1>() : default); break;
            case 2: case2(union.Is<T2>() ? union.As<T2>() : default); break;
            case 3: case3(union.Is<T3>() ? union.As<T3>() : default); break;
            case 4: case4(union.Is<T4>() ? union.As<T4>() : default); break;
            case 5: case5(union.Is<T5>() ? union.As<T5>() : default); break;
            case 6: case6(union.Is<T6>() ? union.As<T6>() : default); break;
            case 7: case7(union.Is<T7>() ? union.As<T7>() : default); break;
            case 8: case8(union.Is<T8>() ? union.As<T8>() : default); break;
            default: throw new InvalidOperationException("Union is not initialized.");
        }
    }

    /// <summary>
    /// Attempts to match a union containing eight possible types against provided case functions and transforms it to a result.
    /// This is a "soft" matching operation where case handlers are optional.
    /// </summary>
    /// <typeparam name="T1">Type of first possible value in the union</typeparam>
    /// <typeparam name="T2">Type of second possible value in the union</typeparam>
    /// <typeparam name="T3">Type of third possible value in the union</typeparam>
    /// <typeparam name="T4">Type of fourth possible value in the union</typeparam>
    /// <typeparam name="T5">Type of fifth possible value in the union</typeparam>
    /// <typeparam name="T6">Type of sixth possible value in the union</typeparam>
    /// <typeparam name="T7">Type of seventh possible value in the union</typeparam>
    /// <typeparam name="T8">Type of eighth possible value in the union</typeparam>
    /// <typeparam name="TResult">The type of the result after transformation</typeparam>
    /// <param name="union">The union instance to match against</param>
    /// <param name="case1">Optional function to handle and transform the first type. If null, this case is skipped</param>
    /// <param name="case2">Optional function to handle and transform the second type. If null, this case is skipped</param>
    /// <param name="case3">Optional function to handle and transform the third type. If null, this case is skipped</param>
    /// <param name="case4">Optional function to handle and transform the fourth type. If null, this case is skipped</param>
    /// <param name="case5">Optional function to handle and transform the fifth type. If null, this case is skipped</param>
    /// <param name="case6">Optional function to handle and transform the sixth type. If null, this case is skipped</param>
    /// <param name="case7">Optional function to handle and transform the seventh type. If null, this case is skipped</param>
    /// <param name="case8">Optional function to handle and transform the eighth type. If null, this case is skipped</param>
    /// <returns>
    /// The transformed result if a matching non-null case function was found and executed;
    /// otherwise returns default value of <typeparamref name="TResult"/>.
    /// </returns>
    public static TResult? TryMatch<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        this Union<T1, T2, T3, T4, T5, T6, T7, T8> union,
        Func<T1?, TResult>? case1 = null,
        Func<T2?, TResult>? case2 = null,
        Func<T3?, TResult>? case3 = null,
        Func<T4?, TResult>? case4 = null,
        Func<T5?, TResult>? case5 = null,
        Func<T6?, TResult>? case6 = null,
        Func<T7?, TResult>? case7 = null,
        Func<T8?, TResult>? case8 = null)
    {
        return union.TypeIndex switch
        {
            1 when case1 != null => case1(union.Is<T1>() ? union.As<T1>() : default),
            2 when case2 != null => case2(union.Is<T2>() ? union.As<T2>() : default),
            3 when case3 != null => case3(union.Is<T3>() ? union.As<T3>() : default),
            4 when case4 != null => case4(union.Is<T4>() ? union.As<T4>() : default),
            5 when case5 != null => case5(union.Is<T5>() ? union.As<T5>() : default),
            6 when case6 != null => case6(union.Is<T6>() ? union.As<T6>() : default),
            7 when case7 != null => case7(union.Is<T7>() ? union.As<T7>() : default),
            8 when case8 != null => case8(union.Is<T8>() ? union.As<T8>() : default),
            _ => default,
        };
    }

    /// <summary>
    /// Attempts to match the union's contained value against one of its potential types and executes the corresponding action for each type if provided.
    /// </summary>
    /// <typeparam name="T1">The first possible type contained in the union.</typeparam>
    /// <typeparam name="T2">The second possible type contained in the union.</typeparam>
    /// <typeparam name="T3">The third possible type contained in the union.</typeparam>
    /// <typeparam name="T4">The fourth possible type contained in the union.</typeparam>
    /// <typeparam name="T5">The fifth possible type contained in the union.</typeparam>
    /// <typeparam name="T6">The sixth possible type contained in the union.</typeparam>
    /// <typeparam name="T7">The seventh possible type contained in the union.</typeparam>
    /// <typeparam name="T8">The eighth possible type contained in the union.</typeparam>
    /// <param name="union">
    /// The union instance containing a value that needs to be matched against one of the provided type cases.
    /// </param>
    /// <param name="case1">The action to execute if the contained value matches <typeparamref name="T1"/>.</param>
    /// <param name="case2">The action to execute if the contained value matches <typeparamref name="T2"/>.</param>
    /// <param name="case3">The action to execute if the contained value matches <typeparamref name="T3"/>.</param>
    /// <param name="case4">The action to execute if the contained value matches <typeparamref name="T4"/>.</param>
    /// <param name="case5">The action to execute if the contained value matches <typeparamref name="T5"/>.</param>
    /// <param name="case6">The action to execute if the contained value matches <typeparamref name="T6"/>.</param>
    /// <param name="case7">The action to execute if the contained value matches <typeparamref name="T7"/>.</param>
    /// <param name="case8">The action to execute if the contained value matches <typeparamref name="T8"/>.</param>
    public static void TryMatch<T1, T2, T3, T4, T5, T6, T7, T8>(
        this Union<T1, T2, T3, T4, T5, T6, T7, T8> union,
        Action<T1?>? case1 = null,
        Action<T2?>? case2 = null,
        Action<T3?>? case3 = null,
        Action<T4?>? case4 = null,
        Action<T5?>? case5 = null,
        Action<T6?>? case6 = null,
        Action<T7?>? case7 = null,
        Action<T8?>? case8 = null)
    {
        switch (union.TypeIndex)
        {
            case 1 when case1 != null:
                case1(union.Is<T1>() ? union.As<T1>() : default);
                break;
            case 2 when case2 != null:
                case2(union.Is<T2>() ? union.As<T2>() : default);
                break;
            case 3 when case3 != null:
                case3(union.Is<T3>() ? union.As<T3>() : default);
                break;
            case 4 when case4 != null:
                case4(union.Is<T4>() ? union.As<T4>() : default);
                break;
            case 5 when case5 != null:
                case5(union.Is<T5>() ? union.As<T5>() : default);
                break;
            case 6 when case6 != null:
                case6(union.Is<T6>() ? union.As<T6>() : default);
                break;
            case 7 when case7 != null:
                case7(union.Is<T7>() ? union.As<T7>() : default);
                break;
            case 8 when case8 != null:
                case8(union.Is<T8>() ? union.As<T8>() : default);
                break;
        }
    }
}