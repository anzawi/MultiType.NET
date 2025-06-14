namespace MultiType.NET.SourceGenerator;

using System.Text;
using Emitters.JsonConverterEmitters;

internal class AnyJsonConverterBuilder(int arity, string anyNamespace, string jsonConverterNamespace)
{
    private readonly StringBuilder _sb = new();

    public AnyJsonConverterBuilder AddHeaders()
    {
        HeadersEmitter.EmitHeaders(this._sb, arity, anyNamespace, jsonConverterNamespace);;
        return this;
    }

    public AnyJsonConverterBuilder AddReadMethod()
    {
        ReadMethodEmitter.EmitReadMethod(this._sb, arity);
        return this;
    }

    public AnyJsonConverterBuilder AddWriteMethod()
    {
        WriteMethodEmitter.EmitReadMethod(this._sb, arity);
        return this;
    }

    public string? Build()
    {
        this._sb.AppendLine("    }");
        return this._sb.ToString();
    }
}