using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly CreateUserView createUserView;
    private readonly ListUsersView listUsersView;

    public ManageUsersView(IUserRepository userRepository, IPostRepository postRepository)
    {
        createUserView = new CreateUserView(userRepository);
        listUsersView = new ListUsersView(userRepository, postRepository);
    }

    public async Task ShowAsync()
    {
        ConsoleOutput.ClearScreen();
        bool showing = true;
        while (showing)
        {
            Console.WriteLine("\n--- Manage Users ---" );
            Console.WriteLine("\n1) Create user" );
            Console.WriteLine("\n2) List users" );
            Console.WriteLine("\n0) Back" );
            Console.Write("> " );
            
            string? choice = Console.ReadLine();
            if (choice is null) return;

            switch (choice.Trim())
            {
                case "1":
                    await createUserView.ShowAsync();
                    ConsoleOutput.ClearScreen();
                    break;
                case "2":
                    await listUsersView.ShowAsync();
                    ConsoleOutput.ClearScreen();
                    break;
                case "0": showing = false; break;
                default: Console.WriteLine($"Invalid choice: {choice}"); break;
            }
        }
    }
}