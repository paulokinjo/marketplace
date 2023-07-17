namespace Marketplace
{
    public class AppSettings
    {
        public MongoSettings Mongo { get; set; } = default!;
    }

    public class MongoSettings
    {
        public string DatabaseName { get; set; } = default!;
        public string ConnectionString => $"{Server.Host.Replace("{credentials}", $"{Server.User}:{Server.Password}")}";
        public MongoServerSettings Server { get; set; } = default!;
        public string PasswordPepper { get; set; } = default!;
        public int TimeoutSessionHours { get; set; } = default!;
    }
    public class MongoServerSettings
    {
        public string Host { get; set; } = default!;
        public string User { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
