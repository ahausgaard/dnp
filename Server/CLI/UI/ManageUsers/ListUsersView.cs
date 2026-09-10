using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;
    
    public ListUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        bool showing = true;
        while (showing)
        {
            Console.WriteLine("\n--- Manage Posts ---" );
            Console.WriteLine("\n1) Create post" );
            Console.WriteLine("\n2) List posts" );
            Console.WriteLine("\n0) Back" );
            Console.WriteLine("> " );
            
            string? choice = Console.ReadLine();
            if (choice is null) return;

            switch (choice.Trim())
            {
                case "0": showing = false; break;
                default: Console.WriteLine($"Invalid choice: {choice}"); break;
            }
        }
    }
}