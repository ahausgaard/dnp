using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;


public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task CreateUserAsync()
    {
        ConsoleOutput.ClearScreen();
        Console.WriteLine("\n--- Create User ---");
        string? name = ConsoleInput.AskFor("Username");
        if (name is null) return;

        string? password = ConsoleInput.AskFor("Password");
        if (password is null) return;

        User created = await userRepository.AddAsync(new User
        {
            UserName = name,
            Password = password
        });

        Console.WriteLine($"User created: {created.UserName} with id {created.Id}");
        ConsoleOutput.Pause();
    }
}