using System.Text.Json;

namespace FileRepositories;

internal static class JsonFile
{
    public static async Task<List<T>> LoadAsync<T>(string filePath)
    {
        string json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<List<T>>(json) ?? [];
    }

    public static List<T> Load<T>(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<T>>(json) ?? [];
    }

    public static Task SaveAsync<T>(string filePath, List<T> items)
    {
        string json = JsonSerializer.Serialize(items);
        return File.WriteAllTextAsync(filePath, json);
    }
}
