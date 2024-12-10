namespace Infrastructure
{
    public static class Hasher
    {
        public static string HashPassword(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            return hash.Length > 100 ? hash.Substring(0, 100) : hash;
        }

        public static bool VerifyPassword(string password, string hash)
        {
            // Truncate the hash to 100 characters before verifying
            var truncatedHash = hash.Length > 100 ? hash.Substring(0, 100) : hash;
            return BCrypt.Net.BCrypt.Verify(password, truncatedHash);
        }
    }
}