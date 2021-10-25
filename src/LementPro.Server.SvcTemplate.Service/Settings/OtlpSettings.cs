namespace LementPro.Server.SvcTemplate.Service.Settings
{
    /// <summary>
    /// Settings class, read on app start
    /// </summary>
    public class OtlpSettings
    {
        /// <summary>
        /// Otlp Enabled?
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Otlp Endpoint
        /// </summary>
        public string Endpoint { get; set; }
    }
}
