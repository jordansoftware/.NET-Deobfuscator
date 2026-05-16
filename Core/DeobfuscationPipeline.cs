using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalNetRemover.Models;
using UniversalNetRemover.Utils;

namespace UniversalNetRemover.Core
{
    public class DeobfuscationPipeline
    {
        public async Task<DeobfuscationResult> ExecuteAsync(byte[] assemblyData, List<string> actionsToRun, string inputPath, string protectorType)
        {
            return await Task.Run(() => 
            {
                var result = new DeobfuscationResult();
                try
                {
                    byte[] cleaned = assemblyData;

                    if (actionsToRun.Contains("Déchiffrer les strings") || actionsToRun.Count == 0)
                    {
                        Logger.Info("Appel de de4dot pour le déchiffrement...");
                        cleaned = AssemblyHelper.DeobfuscateWithDe4dot(inputPath, protectorType.ToLower());
                        Logger.Success("de4dot terminé");
                    }

                    if (actionsToRun.Contains("Nettoyer le flux de contrôle") || actionsToRun.Count == 0)
                    {
                        Logger.Info("Nettoyage des attributs obfusqués...");
                        cleaned = AssemblyHelper.SimpleClean(cleaned);
                        Logger.Success("Nettoyage terminé");
                    }

                    result.ModifiedData = cleaned;
                    result.Success = true;
                }
                catch (System.Exception ex)
                {
                    result.Success = false;
                    result.ErrorMessage = ex.Message;
                    Logger.Error(ex.Message);
                }
                return result;
            });
        }
    }
}