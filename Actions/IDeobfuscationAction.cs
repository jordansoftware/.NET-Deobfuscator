using System.Threading.Tasks;
using AsmResolver.DotNet;

namespace UniversalNetRemover.Actions
{
    public interface IDeobfuscationAction
    {
        string Name { get; }
        string Description { get; }
        int Order { get; }
        Task<bool> ExecuteAsync(AssemblyDefinition assembly, object? context);
    }
}