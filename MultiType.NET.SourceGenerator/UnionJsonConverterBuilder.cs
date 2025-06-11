namespace MultiType.NET.SourceGenerator;

using System.Text;
using Emitters.JsonConverterEmitters;

internal class UnionJsonConverterBuilder(int arity, string unionNamespace, string jsonConverterNamespace)
{
    private readonly StringBuilder _sb = new();

    public UnionJsonConverterBuilder AddHeaders()
    {
        HeadersEmitter.EmitHeaders(this._sb, arity, unionNamespace, jsonConverterNamespace);;
        return this;
    }

    public UnionJsonConverterBuilder AddReadMethod()
    {
        ReadMethodEmitter.EmitReadMethod(this._sb, arity);
        return this;
    }

    public UnionJsonConverterBuilder AddWriteMethod()
    {
        WriteMethodEmitter.EmitReadMethod(this._sb, arity);
        return this;
    }

    public string? Build()
    {
        _sb.AppendLine("    }");
        return _sb.ToString();
    }
}