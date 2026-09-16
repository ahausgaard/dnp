using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;
    private readonly SinglePostView singlePostView;

    public ListPostsView(IPostRepository postRepository, IUserRepository userRepository,
        ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        singlePostView = new SinglePostView(postRepository, userRepository, commentRepository);
    }

    public async Task ShowAsync()
    {
        ConsoleOutput.ClearScreen();

        bool showing = true;
        while (showing)
        {
            Console.WriteLine("\n--- Post List ---");
            Console.WriteLine("\nID |  Title  ");
            Console.WriteLine("-------------");
            foreach (Post post in postRepository.GetMany().OrderBy(p => p.Id))
                Console.WriteLine($"{post.Id}  |  {post.Title}");

            Console.WriteLine("\nChoose a post by id or type 0 to go back");
            Console.Write("> ");

            string? choice = Console.ReadLine();
            if (choice is null) return;
            choice = choice.Trim();

            if (choice == "0")
            {
                showing = false;
            }
            else if (int.TryParse(choice, out int id) && postRepository.GetMany().Any(p => p.Id == id))
            {
                await singlePostView.ShowAsync(id);
                ConsoleOutput.ClearScreen();
            }
            else
            {
                Console.WriteLine($"No post with id: {choice}");
            }
        }
    }
}
