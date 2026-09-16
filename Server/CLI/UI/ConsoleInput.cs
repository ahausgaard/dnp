using RepositoryContracts;

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

    public static int? AskForInt(string label)
    {
        while (true)
        {
            string? input = AskFor(label);
            if (input is null) return null;
            if (int.TryParse(input, out int value)) return value;
            Console.WriteLine($"Not a number: {input}");
        }
    }

    public static int? AskForUserId(IUserRepository userRepository, string label = "Author id")
    {
        while (true)
        {
            int? id = AskForInt(label);
            if (id is null) return null;
            if (userRepository.GetMany().Any(u => u.Id == id)) return id;
            Console.WriteLine($"No user with id: {id}");
        }
    }
}
