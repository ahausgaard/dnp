using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;

    public SinglePostView(IPostRepository postRepository, IUserRepository userRepository,
        ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ShowAsync(int postId)
    {
        ConsoleOutput.ClearScreen();

        bool showing = true;
        while (showing)
        {
            Post post = await postRepository.GetSingleAsync(postId);
            List<Comment> comments = commentRepository.GetMany()
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.Id)
                .ToList();

            Console.WriteLine($"\n--- Post {post.Id}: {post.Title} ---");
            Console.WriteLine($"By: {AuthorName(post.UserId)}");
            Console.WriteLine($"\n{post.Body}");

            Console.WriteLine("\nComments:");
            if (comments.Count == 0)
                Console.WriteLine("(none yet)");
            else
                foreach (Comment comment in comments)
                    Console.WriteLine($"[{comment.Id}] {AuthorName(comment.UserId)}: {comment.Body}");

            Console.WriteLine("\n1) Change title");
            Console.WriteLine("2) Change body");
            Console.WriteLine("3) Delete post");
            Console.WriteLine("4) Add comment");
            Console.WriteLine("5) Edit comment");
            Console.WriteLine("6) Delete comment");
            Console.WriteLine("0) Back");
            Console.Write("> ");

            string? choice = Console.ReadLine();
            if (choice is null) return;

            switch (choice.Trim())
            {
                case "1":
                    string? title = ConsoleInput.AskFor("New title");
                    if (title is null) break;
                    post.Title = title;
                    await postRepository.UpdateAsync(post);
                    Console.WriteLine("Title updated");
                    break;

                case "2":
                    string? body = ConsoleInput.AskFor("New body");
                    if (body is null) break;
                    post.Body = body;
                    await postRepository.UpdateAsync(post);
                    Console.WriteLine("Body updated");
                    break;

                case "3":
                    Console.Write($"Delete \"{post.Title}\" and its {comments.Count} comment(s)? (y/n): ");
                    if (Console.ReadLine()?.Trim().ToLower() != "y") break;
                    foreach (Comment comment in comments)
                        await commentRepository.DeleteAsync(comment.Id);
                    await postRepository.DeleteAsync(post.Id);
                    Console.WriteLine($"Post {post.Id} deleted");
                    ConsoleOutput.Pause();
                    return;

                case "4":
                    string? newBody = ConsoleInput.AskFor("Comment");
                    if (newBody is null) break;
                    int? authorId = ConsoleInput.AskForUserId(userRepository);
                    if (authorId is null) break;
                    await commentRepository.AddAsync(new Comment
                    {
                        Body = newBody,
                        UserId = authorId.Value,
                        PostId = post.Id
                    });
                    Console.WriteLine("Comment added");
                    break;

                case "5":
                    Comment? toEdit = AskForComment(comments, "Comment id to edit");
                    if (toEdit is null) break;
                    string? editedBody = ConsoleInput.AskFor("New comment");
                    if (editedBody is null) break;
                    toEdit.Body = editedBody;
                    await commentRepository.UpdateAsync(toEdit);
                    Console.WriteLine("Comment updated");
                    break;

                case "6":
                    Comment? toDelete = AskForComment(comments, "Comment id to delete");
                    if (toDelete is null) break;
                    await commentRepository.DeleteAsync(toDelete.Id);
                    Console.WriteLine($"Comment {toDelete.Id} deleted");
                    break;

                case "0": showing = false; break;
                default: Console.WriteLine($"Invalid choice: {choice}"); break;
            }
        }
    }

    private string AuthorName(int userId)
        => userRepository.GetMany().SingleOrDefault(u => u.Id == userId)?.Name
           ?? $"unknown user {userId}";

    private static Comment? AskForComment(List<Comment> comments, string label)
    {
        if (comments.Count == 0)
        {
            Console.WriteLine("This post has no comments");
            return null;
        }

        while (true)
        {
            int? id = ConsoleInput.AskForInt(label);
            if (id is null) return null;

            Comment? comment = comments.SingleOrDefault(c => c.Id == id);
            if (comment is not null) return comment;

            Console.WriteLine($"No comment with id {id} on this post");
        }
    }
}
