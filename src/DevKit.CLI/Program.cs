using DevKit.CLI.Commands;
using DevKit.CLI.Helpers;

namespace DevKit.CLI;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            ConsoleHelper.PrintError("Nenhum comando informado.");
            ConsoleHelper.PrintInfo("Uso: devkit <comando> [opções]");
            return;
        }

        var command = args[0].ToLower();
        var commandArgs = args.Skip(1).ToArray();

        switch (command)
        {
            case "hello":
                Console.WriteLine("Bem-vindo ao .NET DevKit CLI 🚀");
                break;
            case "new-project":
                NewProjectCommand.Execute(commandArgs);
                break;

            case "add-package":
                AddPackageCommand.Execute(commandArgs);
                break;

            default:
                ConsoleHelper.PrintError($"Comando desconhecido: {command}");
                ConsoleHelper.PrintInfo("Comandos disponíveis:");
                Console.WriteLine("  new-project     Cria um novo projeto .NET");
                Console.WriteLine("  add-package     Adiciona um pacote NuGet ao projeto atual");
                break;
        }
    }
}
