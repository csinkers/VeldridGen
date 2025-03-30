﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VeldridGen;

public class VeldridSyntaxReceiver : ISyntaxContextReceiver
{
    public List<TypeDeclarationSyntax> Types { get; } = new();
    public List<FieldDeclarationSyntax> Fields { get; } = new();
    public List<PropertyDeclarationSyntax> Properties { get; } = new();
    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        switch (context.Node)
        {
            case TypeDeclarationSyntax { AttributeLists: { Count: > 0 } } type: Types.Add(type); break;
            case FieldDeclarationSyntax { AttributeLists: { Count: > 0 } } field: Fields.Add(field); break;
            case PropertyDeclarationSyntax { AttributeLists: { Count: > 0 } } property: Properties.Add(property); break;
        }
    }
}
