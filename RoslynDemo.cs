using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using NuGet.Protocol;
using rendle_ndc;

/// <summary>
/// Demo code from Mark Rendle's NDC 2022 talk on Roslyn
/// </summary>
public class RoslynDemo
{
    public async Task Start()
    {
        MSBuildLocator.RegisterDefaults();

        var workspace = MSBuildWorkspace.Create();
        workspace.LoadMetadataForReferencedProjects = true;

        workspace.WorkspaceFailed += OnWorkspaceFailed;

        var solution = await workspace.OpenSolutionAsync("E:\\git\\steris\\spmi\\SPMi.sln");

        TryGettingProject(solution);
        await TryListingTypes(solution);
        await TryWalkingFilesInstead(solution);
    }

    private async Task TryWalkingFilesInstead(Solution solution)
    {
        Console.WriteLine("Type Walker Demo");
        var walker = new LukeTypeWalker();

        foreach (var document in solution.Projects.SelectMany(p => p.Documents))
        {
            var root = await document.GetSyntaxRootAsync();
            if (root is null) continue;
            walker.Visit(root);
        }
    }

    private async Task TryListingTypes(Solution solution)
    {
        foreach (var project in solution.Projects)
        {
            Console.WriteLine($"PROJECT: {project.Name}");

            foreach (var document in project.Documents)
            {
                Console.WriteLine($"\tFILE: {document.Name}");
                var root = await document.GetSyntaxRootAsync();
                if (root is null) continue;

                foreach (var typeDeclarationSyntax in root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
                {
                    var namespaceDeclarationSyntax =
                        typeDeclarationSyntax.FirstAncestorOrSelf<NamespaceDeclarationSyntax>();

                    var typeName = typeDeclarationSyntax.Identifier.Text;
                    var namespaceName = namespaceDeclarationSyntax?.Name.ToString();

                    Console.WriteLine($"\t\t{namespaceName}.{typeName}");
                }

            }
        }
    }

    private static void TryGettingProject(Solution solution)
    {
        var project = solution.Projects.FirstOrDefault();

        if (project != null)
        {
            Console.WriteLine($"Opening project {project.Name}");
        }
    }

    private void OnWorkspaceFailed(object? sender, WorkspaceDiagnosticEventArgs e)
    {
        Console.WriteLine($"Workspace failed. {e.ToJson()}");
    }
}