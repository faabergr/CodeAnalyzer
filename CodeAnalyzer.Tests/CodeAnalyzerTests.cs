using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace CodeAnalyzer.Tests
{
    public class CodeAnalyzerTests
    {
        [Fact(DisplayName = "Class with 0 methods returns 0 methods")]
        public void BasicTest()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText("");
            CodeFileAnalyzer ca = new CodeFileAnalyzer(tree);
            CodeAnalysisResults results = ca.Analyze();
            Assert.Equal(0, results.MethodSummaries.Count);
        }

        [Fact(DisplayName = "Class with 1 empty method returns 1 method")]
        public void OneEmptyMethod()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText("public class Testo { public void Foo() {} }");
            CodeFileAnalyzer ca = new CodeFileAnalyzer(tree);
            CodeAnalysisResults results = ca.Analyze();
            Assert.Equal(1, results.MethodSummaries.Count);
        }

        [Fact(DisplayName = "Class with 1 empty method returns 1 method")]
        public void TwoEmptyMethods()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(@"
public class Testo { 
    public void Foo() {} 
    public void Bar() {}
}");
            CodeFileAnalyzer ca = new CodeFileAnalyzer(tree);
            CodeAnalysisResults results = ca.Analyze();
            Assert.Equal(2, results.MethodSummaries.Count);
        }
    }
}
