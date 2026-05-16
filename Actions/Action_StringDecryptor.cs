using System.Threading.Tasks;
using AsmResolver.DotNet;
using UniversalNetRemover.Utils;

namespace UniversalNetRemover.Actions
{
    public class Action_StringDecryptor : IDeobfuscationAction
    {
        public string Name => "Déchiffrer les strings";
        public string Description => "Déchiffre les chaînes de caractères obfusquées";
        public int Order => 1;

        public Task<bool> ExecuteAsync(AssemblyDefinition assembly, object? context)
        {
            int count = 0;
            foreach (var module in assembly.Modules)
            {
                foreach (var type in module.TopLevelTypes)
                {
                    foreach (var method in type.Methods)
                    {
                        if (method.CilMethodBody != null)
                        {
                            count++;
                        }
                    }
                }
            }
            Logger.Success($"Déchiffrement simulé sur {count} méthodes");
            return Task.FromResult(true);
        }
    }
}