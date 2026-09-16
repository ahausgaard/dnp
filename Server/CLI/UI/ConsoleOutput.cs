namespace CLI.UI;


public static class ConsoleOutput
{
    public static void ClearScreen()
    {
        try
        {
            Console.Clear();
        }
        catch (IOException)
        {
            // output is redirected, nothing to clear
        }
    }

    public static void Pause()
    {
        Console.Write("\nPress Enter to continue...");
        Console.ReadLine();
    }
}
