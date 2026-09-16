using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CreatePostView(IPostRepository postRepository, IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        ConsoleOutput.ClearScreen();

        Console.WriteLine("\n--- Create Post ---");
        string? title = ConsoleInput.AskFor("Title");
        if (title is null) return;

        string? body = ConsoleInput.AskFor("Body");
        if (body is null) return;
        
        Console.WriteLine("\nID  |  Name");
        foreach (User user in userRepository.GetMany().OrderBy(u => u.Id))
            Console.WriteLine($"{user.Id}  |  {user.Name}");
        
        int? userId = ConsoleInput.AskForUserId(userRepository);
        if (userId is null) return;

        Post created = await postRepository.AddAsync(new Post
        {
            Title = title,
            Body = body,
            UserId = userId.Value
        });

        Console.WriteLine($"Post created: {created.Title} with id {created.Id}");
        ConsoleOutput.Pause();
    }
}
