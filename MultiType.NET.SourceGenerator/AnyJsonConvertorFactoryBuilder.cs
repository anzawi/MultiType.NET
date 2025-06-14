namespace MultiType.NET.SourceGenerator;

using System.Text;
using Emitters.JsonConverterEmitters.ConvertorFactoryEmitters;

internal class AnyJsonConvertorFactoryBuilder(int arity)
{
    private readonly StringBuilder _sb = new();

    public AnyJsonConvertorFactoryBuilder AddConvertorFactory()
    {
        FactoryEmitter.EmitFactory(this._sb, arity);
        return this;
    }
    
    public string? Build()
    {
        return this._sb.ToString();
    }
}