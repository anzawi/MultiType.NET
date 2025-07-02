namespace MultiType.NET.SourceGenerator;

using System.Text;
using Emitters.AnyEmitters;

internal class AnyTypeBuilder(int arity, string anyNamespace)
{
    private readonly StringBuilder _sb = new();

    public AnyTypeBuilder AddCoreStructure()
    {
        BaseMethodsEmitter.EmitBaseMethods(this._sb, arity, anyNamespace);
        return this;
    }

    public AnyTypeBuilder AddMatchMethods()
    {
        MatchMethodEmitter.EmitMatchMethods(this._sb, arity);
        return this;
    }


    public AnyTypeBuilder AddTryMatchMethods()
    {
        TryMatchMethodEmitter.EmitTryMatchMethods(this._sb, arity);
        return this;
    }

    public AnyTypeBuilder AddMapMethods()
    {
        MapMethodEmitter.EmitMapMethods(this._sb, arity);
        return this;
    }

    public AnyTypeBuilder AddTryGetMethods()
    {
        TryGetMethodEmitter.EmitTryGetMethods(this._sb, arity);
        return this;
    }

    public AnyTypeBuilder AddGetMethods()
    {
        GetMethodEmitter.EmitGetMethods(this._sb, arity);
        return this;
    }

    public AnyTypeBuilder AddSelectMethods()
    {
        SelectMethodsEmitter.EmitSelectMethods(this._sb, arity);
        return this;
    }  
    
    public AnyTypeBuilder AddSwitchMethods()
    {
        SwitchMethodEmitter.EmitSwitchMethods(this._sb, arity);
        return this;
    } 
    
    public AnyTypeBuilder AddEqualityMembers()
    {
        EqualityEmitter.EmitEqualityMembers(this._sb, arity);
        return this;
    }
    
    public AnyTypeBuilder AddDeconstructMethod()
    {
        DeconstructMethodEmitter.EmitDeconstructMethod(this._sb, arity);
        return this;
    } 
    
    public AnyTypeBuilder AddTryParseAndCastMethods()
    {
        ParseAndCastEmitter.EmitParseAndCastMethods(this._sb, arity);
        return this;
    }

    public string? Build()
    {
        this._sb.AppendLine("    }");
        return this._sb.ToString();
    }
}