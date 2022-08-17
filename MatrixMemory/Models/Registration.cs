using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace MatrixMemory.Models;

public class Registration
{
    private readonly string _path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                                    Path.DirectorySeparatorChar + "MatrixMemory";
    public Registration()
    {
        if (!Directory.Exists(_path))
        {
            Directory.CreateDirectory(_path);
        }
    }

    public async void addUser(User user)
    {
        string json = JsonSerializer.Serialize<User>(user);
        string userFile = _path + Path.DirectorySeparatorChar + user.UserName + ".json";
        
        if (!File.Exists(userFile))
        {
            File.Create(userFile);
            await File.WriteAllTextAsync(userFile, json);
        }
        else
        {
            throw  new ArgumentException($"User {user.UserName} already exists");
        }
    }
}