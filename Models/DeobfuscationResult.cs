using System.Collections.Generic;

namespace UniversalNetRemover.Models
{
    public class DeobfuscationResult
    {
        public bool Success { get; set; }
        public byte[]? ModifiedData { get; set; }
        public List<string> LogMessages { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }
}