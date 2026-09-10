using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    
    public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- Main menu ---");
            Console.WriteLine("\n1) Manage users");
            Console.WriteLine("\n2) Manage posts");
            Console.WriteLine("\n0) Exit");
            Console.WriteLine("> ");

            string? choice = Console.ReadLine();
            if (choice is null) break;

            switch (choice.Trim().ToLower())
            {
                case "1":
                    break;
                case "2": ;
                    break;
                case "0":
                    running = false;
                    break;
                default: 
                    Console.WriteLine($"Invalid choice: {choice}");
                    break;
            }
            
        }
    }
}