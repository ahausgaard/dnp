namespace CLI.UI.ManagePosts;

public class ListPostsView
{
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
                default: Console.WriteLine($"Invalid choice: {choice}"); break;
            }
        }
    }
}