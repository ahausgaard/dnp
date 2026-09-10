namespace CLI.UI;


public static class ConsoleInput
{
    public static string? AskFor(string label)
    {
        while (true)
        {
            Console.Write($"{label} (blank to cancel): ");
            string? input = Console.ReadLine();
            if (input is null) return null;
            input = input.Trim();
            if (input.Length == 0) return null;
            return input;
        }
    }
}
