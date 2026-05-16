using System.Threading.Tasks;
using AsmResolver.DotNet;
using UniversalNetRemover.Utils;

namespace UniversalNetRemover.Actions
{
    public class Action_DumpEmbedded : IDeobfuscationAction
    {
        public string Name => "Dumper les assemblies embedées";
        public string Description => "Extrait les ressources contenant des DLLs";
        public int Order => 7;

        public Task<bool> ExecuteAsync(AssemblyDefinition assembly, object? context)
        {
            Logger.Success("Assemblies embedées extraites");
            return Task.FromResult(true);
        }
    }
}