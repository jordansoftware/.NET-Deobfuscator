using System.Threading.Tasks;
using AsmResolver.DotNet;
using UniversalNetRemover.Utils;

namespace UniversalNetRemover.Actions
{
    public class Action_VirtualizationRemover : IDeobfuscationAction
    {
        public string Name => "Dévirtualiser le code VM";
        public string Description => "Tente de supprimer la virtualisation de code";
        public int Order => 4;

        public Task<bool> ExecuteAsync(AssemblyDefinition assembly, object? context)
        {
            Logger.Warning("Dévirtualisation complète nécessite un émulateur avancé");
            return Task.FromResult(false);
        }
    }
}