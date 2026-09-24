using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;
    private readonly SingleUserView singleUserView;

    public ListUsersView(IUserRepository userRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        singleUserView = new SingleUserView(userRepository, postRepository);
    }

    public async Task ListUsersAsync()
    {
        ConsoleOutput.ClearScreen();
        List<User> users = userRepository.GetMany().OrderBy(u => u.Id).ToList();
        bool showing = true;
        
        while (showing)
        {
            Console.WriteLine("\n--- User List ---");
            Console.WriteLine("\nID |  Name  ");
            Console.WriteLine("-------------");
            foreach (User user in users)
                Console.WriteLine($"{user.Id}  |  {user.UserName}");

            Console.WriteLine("\nChoose a user by id or type 0 to go back");
            Console.Write("> ");

            string? choice = Console.ReadLine();
            if (choice is null) return;
            choice = choice.Trim();

            if (choice == "0")
            {
                showing = false;
            }
            else if (int.TryParse(choice, out int id) && users.Any(u => u.Id == id))
            {
                await singleUserView.ShowAsync(id);
                ConsoleOutput.ClearScreen();
            }
            else
            {
                Console.WriteLine($"No user with id: {choice}");
            }
        }
    }
}
