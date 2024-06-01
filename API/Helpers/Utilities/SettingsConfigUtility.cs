namespace API.Helpers.Utilities
{
    public class SettingsConfigUtility
    {
        // Get a valued stored in the appsettings.
        // Pass in a key like TestArea:TestKey to get TestValue
        public static string GetCurrentSettings(string key)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            return configuration.GetSection(key).Value;
        }
    }

    public class SettingsConfigUtility<T> where T : class
    {
        // Get a valued stored in the appsettings.
        // Pass in a key like TestArea:TestKey to get TestValue
        public static T GetCurrentSettings(string key)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            return configuration.GetSection(key).Get<T>();
        }
    }
}