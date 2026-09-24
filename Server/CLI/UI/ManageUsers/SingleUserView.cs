using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class SingleUserView
{
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;

    public SingleUserView(IUserRepository userRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.postRepository = postRepository;
    }

    public async Task ShowAsync(int userId)
    {
        ConsoleOutput.ClearScreen();

        bool showing = true;
        while (showing)
        {
            User user = await userRepository.GetSingleAsync(userId);
            List<Post> posts = postRepository.GetMany()
                .Where(p => p.UserId == userId)
                .OrderBy(p => p.Id)
                .ToList();

            Console.WriteLine($"\n--- User {user.Id}: {user.UserName} ---");

            Console.WriteLine("\nPosts:");
            if (posts.Count == 0)
                Console.WriteLine("(none yet)");
            else
                foreach (Post post in posts)
                    Console.WriteLine($"[{post.Id}] {post.Title}");

            Console.WriteLine("\n1) Change username");
            Console.WriteLine("2) Change password");
            Console.WriteLine("3) Delete user");
            Console.WriteLine("0) Back");
            Console.Write("> ");

            string? choice = Console.ReadLine();
            if (choice is null) return;

            switch (choice.Trim())
            {
                case "1":
                    string? name = ConsoleInput.AskFor("New username");
                    if (name is null) break;
                    user.UserName = name;
                    await userRepository.UpdateAsync(user);
                    Console.WriteLine("Username updated");
                    break;

                case "2":
                    string? password = ConsoleInput.AskFor("New password");
                    if (password is null) break;
                    user.Password = password;
                    await userRepository.UpdateAsync(user);
                    Console.WriteLine("Password updated");
                    break;

                case "3":
                    Console.Write($"Delete {user.UserName}? Their {posts.Count} post(s) will be kept. (y/n): ");
                    if (Console.ReadLine()?.Trim().ToLower() != "y") break;
                    await userRepository.DeleteAsync(user.Id);
                    Console.WriteLine($"User {user.UserName} deleted");
                    ConsoleOutput.Pause();
                    return;

                case "0": showing = false; break;
                default: Console.WriteLine($"Invalid choice: {choice}"); break;
            }
        }
    }
}
