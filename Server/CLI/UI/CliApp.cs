using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly ManageUsersView manageUsersView;

    public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        manageUsersView = new ManageUsersView(userRepository);
    }

    public async Task StartAsync()
    {
        bool showing = true;
        while (showing)
        {
            Console.WriteLine("\n--- Main menu ---");
            Console.WriteLine("\n1) Manage users");
            Console.WriteLine("\n2) Manage posts");
            Console.WriteLine("\n0) Exit");
            Console.Write("\n> ");

            string? choice = Console.ReadLine();
            if (choice is null) break;

            switch (choice.Trim())
            {
                case "1": await manageUsersView.ShowAsync(); break;
                case "2": break;
                case "0": showing = false; break;
                default: Console.WriteLine($"Invalid choice: {choice}"); break;
            }
            
        }
    }
}