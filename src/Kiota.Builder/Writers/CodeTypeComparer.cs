using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Kiota.Builder.CodeDOM;

namespace Kiota.Builder.Writers;

// Used to order proerties to generate code for methods like serializer, deserializer, factory, etc..
internal sealed class CodeTypeComparer : IComparer<CodeTypeBase>
{
    // set to true when used for union model (anyOf)
    // set to false when used for intersection model (oneOf)
    private readonly bool OrderByDesc;
    public CodeTypeComparer(bool orderByDesc = false)
    {
        OrderByDesc = orderByDesc;
    }
    public int GetHashCode([DisallowNull] CodeTypeBase obj)
    {
        if (obj is CodeType type && !OrderByDesc)
            return (type.TypeDefinition, type.IsCollection) switch
            {
                (CodeClass or CodeInterface or CodeEnum, true) => 7,
                (null, false) => 11,
                (CodeClass or CodeInterface or CodeEnum, false) => 13,
                (_, _) => 17,
            };
        if (obj is CodeType type2 && OrderByDesc)
            return (type2.TypeDefinition, type2.IsCollection) switch
            {
                (null, false) => 7,
                (CodeClass or CodeInterface or CodeEnum, false) => 11,
                (CodeClass or CodeInterface or CodeEnum, true) => 13,
                (_, _) => 17,
            };
        return 23;
    }
    public int Compare(CodeTypeBase? x, CodeTypeBase? y)
    {
        return (x, y) switch
        {
            (null, null) => 0,
            (null, _) => -1,
            (_, null) => 1,
            _ => GetHashCode(x).CompareTo(GetHashCode(y)),
        };
    }
}
