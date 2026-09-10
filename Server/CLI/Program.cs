using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Hello, World!");
IUserRepository userRepository = new UserInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();

ClipApp cliApp = new ClipApp(userRepository, commentRepository, postRepository);
await cliApp.StartAsync();