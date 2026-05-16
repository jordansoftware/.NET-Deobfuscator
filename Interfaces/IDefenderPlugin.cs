using System.Threading.Tasks;
using UniversalNetRemover.Models;

namespace UniversalNetRemover.Interfaces
{
    public interface IDefenderPlugin
    {
        string Name { get; }
        string Version { get; }
        bool CanHandle(DetectionResult detection);
        Task<DeobfuscationResult> ExecuteAsync(byte[] assemblyData, System.Collections.Generic.List<string> actionsToRun);
    }
}