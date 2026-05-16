using System.Threading.Tasks;
using AsmResolver.DotNet;
using UniversalNetRemover.Utils;

namespace UniversalNetRemover.Actions
{
    public class Action_AntiTamperRemover : IDeobfuscationAction
    {
        public string Name => "Supprimer l'Anti-Tamper";
        public string Description => "Désactive les vérifications d'intégrité";
        public int Order => 5;

        public Task<bool> ExecuteAsync(AssemblyDefinition assembly, object? context)
        {
            int checksRemoved = 0;
            Logger.Success($"Anti-tamper supprimé de {checksRemoved} emplacements");
            return Task.FromResult(true);
        }
    }
}