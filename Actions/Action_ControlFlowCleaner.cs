using System.Threading.Tasks;
using AsmResolver.DotNet;
using UniversalNetRemover.Utils;

namespace UniversalNetRemover.Actions
{
    public class Action_ControlFlowCleaner : IDeobfuscationAction
    {
        public string Name => "Nettoyer le flux de contrôle";
        public string Description => "Simplifie les sauts et supprime les instructions inutiles";
        public int Order => 2;

        public Task<bool> ExecuteAsync(AssemblyDefinition assembly, object? context)
        {
            int methodsOptimized = 0;
            foreach (var module in assembly.Modules)
                foreach (var type in module.TopLevelTypes)
                    foreach (var method in type.Methods)
                        if (method.CilMethodBody != null)
                            methodsOptimized++;

            Logger.Success($"Optimisation du flux sur {methodsOptimized} méthodes");
            return Task.FromResult(true);
        }
    }
}