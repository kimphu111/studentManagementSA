using BCrypt.Net;

namespace AuthGateway.Helpers;

public static class PasswordHelper
{
    //Hash when register
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    //Verify password when login
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}