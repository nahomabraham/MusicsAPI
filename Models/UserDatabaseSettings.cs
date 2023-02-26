namespace authentication.Model
{
    public class UserDatabaseSettings
    {
        public String ConnectionString { get; set; } = String.Empty;
        public String DatabaseName { get; set; } = String.Empty;
        public String UserCollectionName { get; set; } = String.Empty;
    }
}

