using DevKit.CLI.Helpers;
using System.Diagnostics;

namespace DevKit.CLI.Commands;

public static class AddPackageCommand
{
    public static void Execute(string[] args)
    {
        if (args.Length < 1)
        {
            ConsoleHelper.PrintError("Uso inválido do comando add-package.");
            ConsoleHelper.PrintInfo("Exemplo: devkit add-package Serilog");
            return;
        }

        var packageName = args[0];
        ConsoleHelper.PrintInfo($"Adicionando pacote NuGet: {packageName}");

        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"add package {packageName}",
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
            ConsoleHelper.PrintSuccess($"Pacote \"{packageName}\" adicionado com sucesso!");
            Console.WriteLine(output);
        }
        else
        {
            ConsoleHelper.PrintError("Erro ao adicionar o pacote:");
            Console.WriteLine(error);
        }
    }
}
