using Microsoft.AspNetCore.Identity;
using System;

public class User { } // dummy
public class Program {
    public static void Main() {
        var hasher = new PasswordHasher<User>();
        Console.WriteLine(hasher.HashPassword(new User(), "Admin123!"));
    }
}
