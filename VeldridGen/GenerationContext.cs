using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VeldridGen
{
    class GenerationContext
    {
        public GenerationContext(Compilation compilation, VeldridSyntaxReceiver receiver)
        {
            Symbols = new Symbols(compilation);
#pragma warning disable RS1024 // Compare symbols correctly
            Types = new Dictionary<INamedTypeSymbol, VeldridTypeInfo>(SymbolEqualityComparer.Default);
#pragma warning restore RS1024 // Compare symbols correctly

            ISymbol GetSymbol(MemberDeclarationSyntax memberDeclarationSyntax)
            {
                var model = compilation.GetSemanticModel(memberDeclarationSyntax.SyntaxTree);
                if (memberDeclarationSyntax is FieldDeclarationSyntax field)
                {
                    VariableDeclaratorSyntax variable = field.Declaration.Variables.Single();
                    return model.GetDeclaredSymbol(variable);
                }

                return model.GetDeclaredSymbol(memberDeclarationSyntax);
            }

            void PopulateMember(MemberDeclarationSyntax memberDeclarationSyntax)
            {
                var sym = GetSymbol(memberDeclarationSyntax);
                if (sym == null)
                    return;

                var typeSym = sym.ContainingType;
                if (!Types.TryGetValue(typeSym, out var typeInfo))
                {
                    typeInfo = new VeldridTypeInfo(typeSym, Symbols);
                    Types[typeSym] = typeInfo;
                }

                typeInfo.AddMember(sym, Symbols);
            }

            foreach (var field in receiver.Fields)
                PopulateMember(field);

            foreach (var property in receiver.Properties)
                PopulateMember(property);

            foreach (var type in receiver.Types)
            {
                var sym = (INamedTypeSymbol)GetSymbol(type);
                if (sym == null)
                    return;

                if (Types.TryGetValue(sym, out var typeInfo)) 
                    continue;

                typeInfo = new VeldridTypeInfo(sym, Symbols);
                Types[sym] = typeInfo;
            }

        }

        public Symbols Symbols { get; }
        public Dictionary<INamedTypeSymbol, VeldridTypeInfo> Types { get; }
    }
}