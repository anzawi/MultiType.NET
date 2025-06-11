namespace MultiType.NET.SourceGenerator;

using System.Text;
using Emitters.UnionEmitters;

internal class UnionTypeBuilder(int arity, string unionNamespace)
{
    private readonly StringBuilder _sb = new();

    public UnionTypeBuilder AddCoreStructure()
    {
        BaseMethodsEmitter.EmitBaseMethods(this._sb, arity, unionNamespace);
        return this;
    }

    public UnionTypeBuilder AddMatchMethods()
    {
        MatchMethodEmitter.EmitMatchMethods(_sb, arity);
        return this;
    }


    public UnionTypeBuilder AddTryMatchMethods()
    {
        TryMatchMethodEmitter.EmitTryMatchMethods(_sb, arity);
        return this;
    }

    public UnionTypeBuilder AddMapMethods()
    {
        MapMethodEmitter.EmitMapMethods(_sb, arity);
        return this;
    }

    public UnionTypeBuilder AddTryGetMethods()
    {
        TryGetMethodEmitter.EmitTryGetMethods(_sb, arity);
        return this;
    }

    public UnionTypeBuilder AddGetMethods()
    {
        GetMethodEmitter.EmitGetMethods(_sb, arity);
        return this;
    }

    public UnionTypeBuilder AddSelectMethods()
    {
        SelectMethodsEmitter.EmitSelectMethods(_sb, arity);
        return this;
    }  
    
    public UnionTypeBuilder AddSwitchMethods()
    {
        SwitchMethodEmitter.EmitSwitchMethods(_sb, arity);
        return this;
    } 
    
    public UnionTypeBuilder AddEqualityMembers()
    {
        EqualityEmitter.EmitEqualityMembers(_sb, arity);
        return this;
    }
    
    public UnionTypeBuilder AddDeconstructMethod()
    {
        DeconstructMethodEmitter.EmitDeconstructMethod(_sb, arity);
        return this;
    }

    public string? Build()
    {
        _sb.AppendLine("    }");
        return _sb.ToString();
    }
}