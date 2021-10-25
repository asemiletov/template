namespace LementPro.Server.SvcTemplate.Api.Settings
{
    /// <summary>
    /// Settings class, read on app start
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Run in test mode
        /// </summary>
        public bool TestMode { get; private set; }

        /// <summary>
        /// Drop db on start
        /// </summary>
        public bool DropOnStart { get; private set; }

        /// <summary>
        /// Seed db on start
        /// </summary>
        public bool SeedOnStart { get; private set; }

        /// <summary>
        /// Seed template
        /// </summary>
        public string SeedTemplate { get; private set; }
    }
}
