using System.Diagnostics;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

public class AddPackageCommand : Command<AddPackageCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<packages>")]
        [Description("Nome(s) do(s) pacote(s) a serem adicionados")]
        public string[] Packages { get; set; }

        [CommandOption("-v|--version <version>")]
        [Description("Versão específica do pacote (opcional, vale para todos os pacotes)")]
        public string Version { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        if (!File.Exists("*.csproj") && Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj").Length == 0)
        {
            AnsiConsole.MarkupLine("[red]Nenhum arquivo .csproj encontrado no diretório atual.[/]");
            return 1;
        }

        foreach (var package in settings.Packages)
        {
            string args = $"add package {package}";
            if (!string.IsNullOrWhiteSpace(settings.Version))
                args += $" --version {settings.Version}";

            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = args,
                RedirectStandardOutput = true,
                UseShellExecute = false
            });

            process.WaitForExit();
            AnsiConsole.MarkupLine($"[green]Pacote [bold]{package}[/] adicionado com sucesso.[/]");
        }

        return 0;
    }
}
