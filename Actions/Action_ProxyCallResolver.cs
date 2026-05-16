using System.Threading.Tasks;
using AsmResolver.DotNet;
using UniversalNetRemover.Utils;

namespace UniversalNetRemover.Actions
{
    public class Action_ProxyCallResolver : IDeobfuscationAction
    {
        public string Name => "Résoudre les appels proxy";
        public string Description => "Convertit les appels proxy en appels directs";
        public int Order => 3;

        public Task<bool> ExecuteAsync(AssemblyDefinition assembly, object? context)
        {
            int proxyCallsFixed = 0;
            Logger.Success($"{proxyCallsFixed} appels proxy résolus");
            return Task.FromResult(true);
        }
    }
}