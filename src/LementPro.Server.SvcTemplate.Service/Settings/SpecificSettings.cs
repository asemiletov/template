namespace LementPro.Server.SvcTemplate.Service.Settings
{
    /// <summary>
    /// Settings class, read on app start
    /// </summary>
    public class SpecificSettings
    {
        /// <summary>
        /// Адреса других сервисов
        /// </summary>
        public ServicesSettings Services { get; set; }

        /// <summary>
        /// Otlp
        /// </summary>
        public OtlpSettings Otlp { get; set; }
    }
}
