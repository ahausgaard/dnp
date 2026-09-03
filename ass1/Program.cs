namespace ass1;
class Program
{
    static void Main(string[] args)
    {
        Post post = new Post("Title", "Body");
        Post post2 = new Post("Title", "Body");
        Console.WriteLine(post.id);
        Console.WriteLine(post2.id);
    }
}
