using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private readonly CreatePostView createPostView;
    private readonly ListPostsView listPostsView;

    public ManagePostsView(IPostRepository postRepository, IUserRepository userRepository,
        ICommentRepository commentRepository)
    {
        createPostView = new CreatePostView(postRepository, userRepository);
        listPostsView = new ListPostsView(postRepository, userRepository, commentRepository);
    }

    public async Task ShowAsync()
    {
        ConsoleOutput.ClearScreen();

        bool showing = true;
        while (showing)
        {
            Console.WriteLine("\n--- Manage Posts ---");
            Console.WriteLine("\n1) Create post");
            Console.WriteLine("\n2) List posts");
            Console.WriteLine("\n0) Back");
            Console.Write("> ");

            string? choice = Console.ReadLine();
            if (choice is null) return;

            switch (choice.Trim())
            {
                case "1":
                    await createPostView.CreatePostAsync();
                    ConsoleOutput.ClearScreen();
                    break;
                case "2":
                    await listPostsView.ListPostsAsync();
                    ConsoleOutput.ClearScreen();
                    break;
                case "0": showing = false; break;
                default: Console.WriteLine($"Invalid choice: {choice}"); break;
            }
        }
    }
}
