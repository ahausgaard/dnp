namespace ass1;

public class IdGenerator
{
    private static int _currentId = 0;
    public static int NextId() => Interlocked.Increment(ref _currentId);
}