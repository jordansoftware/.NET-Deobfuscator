using System.Collections.Generic;

namespace UniversalNetRemover.Models
{
    public class DetectionResult
    {
        public string ProtectorName { get; set; } = "Inconnu";
        public string Version { get; set; } = "";
        public double Confidence { get; set; } = 0.2;
        public bool IsObfuscated { get; set; } = false;
        public List<string> PatternsDetected { get; set; } = new List<string>();

        public override string ToString()
        {
            if (!IsObfuscated)
                return $"Non obfusqué (Confiance: {Confidence:P0})";
            
            return $"{ProtectorName} {Version} (Confiance: {Confidence:P0})";
        }
    }
}