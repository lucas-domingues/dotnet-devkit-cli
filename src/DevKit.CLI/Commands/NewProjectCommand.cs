using DevKit.CLI.Helpers;
using System.Diagnostics;

namespace DevKit.CLI.Commands;

public static class NewProjectCommand
{
    public static void Execute(string[] args)
    {
        if (args.Length < 2)
        {
            ConsoleHelper.PrintError("Uso inválido do comando new-project.");
            ConsoleHelper.PrintInfo("Exemplo: devkit new-project MeuApp webapi");
            return;
        }

        var name = args[0];
        var type = args[1];

        var tiposValidos = new[] { "console", "webapi", "mvc", "classlib", "blazor" };

        if (!tiposValidos.Contains(type.ToLower()))
        {
            ConsoleHelper.PrintWarning($"Tipo de projeto inválido: \"{type}\"");
            ConsoleHelper.PrintList("Tipos válidos", tiposValidos);
            return;
        }

        var template = type.ToLower() switch
        {
            "console" => "console",
            "webapi" => "webapi",
            "mvc" => "mvc",
            "classlib" => "classlib",
            "blazor" => "blazorserver",
            _ => null
        };

        if (template == null)
        {
            ConsoleHelper.PrintError("Template não encontrado.");
            return;
        }

        ConsoleHelper.PrintInfo($"Criando projeto \"{name}\" com o template \"{template}\"...");

        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"new {template} -n {name}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = Process.Start(psi);
        process.WaitForExit();

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();

        if (process.ExitCode == 0)
        {
            ConsoleHelper.PrintSuccess("Projeto criado com sucesso!");
            Console.WriteLine(output);
        }
        else
        {
            ConsoleHelper.PrintError("Erro ao criar o projeto:");
            Console.WriteLine(error);
        }
    }
}
