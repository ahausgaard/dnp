namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    CreatePostView createPostView;
    ListPostsView listPostsView;


    public async Task ShowAsync()
    {
        bool showing = true;
        while (showing)
        {
            Console.WriteLine("\n--- Manage Posts ---");
            Console.WriteLine("\n1) Create post");
            Console.WriteLine("\n2) List posts");
            Console.WriteLine("\n0) Back");
            Console.WriteLine("> ");

            string? choice = Console.ReadLine();
            if (choice is null) return;

            switch (choice.Trim())
            {
                case "1": await createPostView.ShowAsync(); break;
                case "2": await listPostsView.ShowAsync(); break;
                case "0": showing = false; break;
                default: Console.WriteLine($"Invalid choice: {choice}"); break;
            }
        }
    }
}