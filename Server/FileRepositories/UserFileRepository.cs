using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";
    
    public UserFileRepository()
    {
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "[]");
    }

    public async Task<User> AddAsync(User user)
    {
        List<User> users = await JsonFile.LoadAsync<User>(filePath);
        user.Id = users.Any()
            ? users.Max(u => u.Id) + 1
            : 1;
        users.Add(user);
        await JsonFile.SaveAsync(filePath, users);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        List<User> users = await JsonFile.LoadAsync<User>(filePath);
        User? existingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        }

        int index = users.IndexOf(existingUser);
        users[index] = user;
        await JsonFile.SaveAsync(filePath, users);
    }

    public async Task DeleteAsync(int id)
    {
        List<User> users = await JsonFile.LoadAsync<User>(filePath);
        User? userToRemove = users.SingleOrDefault(u => u.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        users.Remove(userToRemove);
        await JsonFile.SaveAsync(filePath, users);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        List<User> users = await JsonFile.LoadAsync<User>(filePath);
        User? user = users.SingleOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }
        return user;
    }
    
    public IQueryable<User> GetMany()
    {
        List<User> users = JsonFile.Load<User>(filePath);
        return users.AsQueryable();
    }
}