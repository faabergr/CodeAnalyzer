using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace CodeAnalyzer
{
    public class CodeFileAnalyzer
    {
        private SyntaxTree _tree;

        public CodeFileAnalyzer(SyntaxTree tree)
        {
            _tree = tree;
        }

        public CodeAnalysisResults Analyze()
        {
            var methods = GetMethods(_tree.GetRoot());

            return new CodeAnalysisResults() { MethodSummaries = methods.Select(AnalyzeMethod).ToList() };
        }

        private List<MethodDeclarationSyntax> GetMethods(SyntaxNode rootNode)
        {
            List<MethodDeclarationSyntax> methods = new List<MethodDeclarationSyntax>();

            MethodDeclarationSyntax method = rootNode as MethodDeclarationSyntax;
            if (method != null)
            {
                methods.Add(method);
            }
            else if (rootNode.ChildNodes() != null)
            {
                foreach (var childNode in rootNode.ChildNodes())
                {
                    methods.AddRange(GetMethods(childNode));
                }
            }

            return methods;
        }

        public MethodSummary AnalyzeMethod(MethodDeclarationSyntax methodSyntax)
        {
            var firstDirective = methodSyntax.GetFirstDirective();

            if (firstDirective == null)
            {
                return new MethodSummary();
            }

            var firstLocation = firstDirective.GetLocation();
            var firstDirectiveLineSpan = firstLocation.GetLineSpan();
            var firstLinePosition = firstDirectiveLineSpan.StartLinePosition;

            var lastDirective = methodSyntax.GetLastDirective();
            var lastLocation = lastDirective.GetLocation();
            var lastDirectiveLineSpan = lastLocation.GetLineSpan();
            var lastLinePosition = lastDirectiveLineSpan.EndLinePosition;

            var methodSummary = new MethodSummary();
            methodSummary.Name = methodSyntax.Identifier.ValueText;

            methodSummary.LinesOfCode = (ulong)(lastLinePosition.Line - firstLinePosition.Line);

            return methodSummary;
        }
    }

    public class CodeAnalysisResults
    {
        public List<MethodSummary> MethodSummaries { get; set; }
    }

    public class MethodSummary
    {
        public string Name { get; set; }
        public ulong LinesOfCode { get; set; }
        public ulong AverageLineLength { get; set; }
    }
}
