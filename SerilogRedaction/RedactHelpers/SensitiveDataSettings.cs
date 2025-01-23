namespace SerilogRedaction.RedactHelpers
{
    public class SensitiveDataSettings
    {
        public Dictionary<string, string> Patterns { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> MaskingPatterns { get; set; } = new Dictionary<string, string>();
    }
}
