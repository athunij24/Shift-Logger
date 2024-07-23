using Spectre.Console;

namespace ShiftLoggerApp
{
    class Program
    {
        public static string promptUser()
        {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like to do?")
                .PageSize(10)
                .AddChoices(new[] {
                    "Log Shift", "View Shifts", "Delete Shift",
                    "Register Employee", "Email Contact", "Quit"
                }));
            return choice;
        }
        static void Main(string[] args)
        {

        }
    }
}
