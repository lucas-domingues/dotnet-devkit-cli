using System.CommandLine;

var rootCommand = new RootCommand("DevKit CLI - Ferramentas para devs .NET");

// Comando de boas-vindas
var helloCommand = new Command("hello", "Mostra uma mensagem de boas-vindas");
helloCommand.SetHandler(() =>
{
    Console.WriteLine("Bem-vindo ao .NET DevKit CLI 🚀");
});
rootCommand.AddCommand(helloCommand);

// Novo comando: `new ddd <projectName>`
var newDddCommand = new Command("new-ddd", "Cria uma estrutura DDD básica.");

var nameArgument = new Argument<string>("projectName", "Nome do projeto");
newDddCommand.AddArgument(nameArgument);

newDddCommand.SetHandler((string projectName) =>
{
    var root = Path.Combine(Directory.GetCurrentDirectory(), projectName);
    var src = Path.Combine(root, "src");
    var tests = Path.Combine(root, "tests");

    var layers = new (string path, string sdk)[]
    {
        (Path.Combine(src, $"{projectName}.Api"), "Microsoft.NET.Sdk.Web"),
        (Path.Combine(src, $"{projectName}.Application"), "Microsoft.NET.Sdk"),
        (Path.Combine(src, $"{projectName}.Domain"), "Microsoft.NET.Sdk"),
        (Path.Combine(src, $"{projectName}.Infrastructure"), "Microsoft.NET.Sdk"),
        (Path.Combine(tests, $"{projectName}.Tests.Unit"), "Microsoft.NET.Sdk"),
        (Path.Combine(tests, $"{projectName}.Tests.Integration"), "Microsoft.NET.Sdk")
    };

    foreach (var (dir, sdk) in layers)
    {
        Directory.CreateDirectory(dir);
        var csprojContent = $"""
        <Project Sdk="{sdk}">
          <PropertyGroup>
            <TargetFramework>net8.0</TargetFramework>
            <Nullable>enable</Nullable>
            {(sdk == "Microsoft.NET.Sdk.Web" ? "<ImplicitUsings>enable</ImplicitUsings>" : "")}
          </PropertyGroup>
        </Project>
        """;

        File.WriteAllText(Path.Combine(dir, $"{Path.GetFileName(dir)}.csproj"), csprojContent);
        Console.WriteLine($"📁 Criado: {dir}");
    }

    File.WriteAllText(Path.Combine(root, "README.md"), $"# {projectName}\n\nGerado com o DevKit CLI 🚀");
    Console.WriteLine($"\n✅ Projeto {projectName} criado com sucesso!");
}, nameArgument);


rootCommand.AddCommand(newDddCommand);

return await rootCommand.InvokeAsync(args);
