using System;
using HotChocolate.Data.Filters;
using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace HotChocolate.Types;

public static class FilterFieldDescriptorExtensions
{
    public static void MakeNullable(this IFilterFieldDescriptor descriptor) =>
        descriptor.Extend()
            .OnBeforeCreate(
                (c, def) => def.Type = RewriteTypeToNullableType(def, c.TypeInspector));

    public static void MakeNullable(this IFilterOperationFieldDescriptor descriptor) =>
        descriptor.Extend()
            .OnBeforeCreate(
                (c, def) => def.Type = RewriteTypeToNullableType(def, c.TypeInspector));

    private static TypeReference RewriteTypeToNullableType(
        FilterFieldDefinition definition,
        ITypeInspector typeInspector)
    {
        var reference = definition.Type;

        if (reference is ExtendedTypeReference extendedTypeRef)
        {
            return extendedTypeRef.Type.IsNullable
                ? extendedTypeRef
                : extendedTypeRef.WithType(
                    typeInspector.ChangeNullability(extendedTypeRef.Type, true));
        }

        if (reference is SchemaTypeReference schemaRef)
        {
            return schemaRef.Type is NonNullType nnt
                ? schemaRef.WithType(nnt.Type)
                : schemaRef;
        }

        if (reference is SyntaxTypeReference syntaxRef)
        {
            return syntaxRef.Type is NonNullTypeNode nnt
                ? syntaxRef.WithType(nnt.Type)
                : syntaxRef;
        }

        throw new NotSupportedException();
    }
}
