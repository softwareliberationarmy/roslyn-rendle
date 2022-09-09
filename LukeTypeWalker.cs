using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace rendle_ndc
{
    public class LukeTypeWalker: CSharpSyntaxWalker
    {
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var namespaceParent = node.FirstAncestorOrSelf<NamespaceDeclarationSyntax>();
            var typeName = node.Identifier.Text;
            var namespaceName = namespaceParent?.Name.ToString();

            Console.WriteLine($"{namespaceName}.{typeName}");
            
            base.VisitClassDeclaration(node);
        }
    }
}
