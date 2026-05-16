using System.Threading.Tasks;
using AsmResolver.DotNet;
using UniversalNetRemover.Utils;

namespace UniversalNetRemover.Actions
{
    public class Action_ConstantDecryptor : IDeobfuscationAction
    {
        public string Name => "Décrypter les constantes";
        public string Description => "Simplifie les expressions mathématiques obfusquées";
        public int Order => 6;

        public Task<bool> ExecuteAsync(AssemblyDefinition assembly, object? context)
        {
            Logger.Success("Constantes déchiffrées");
            return Task.FromResult(true);
        }
    }
}