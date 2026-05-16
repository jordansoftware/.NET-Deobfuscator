using System;
using System.IO;
using System.Diagnostics;
using UniversalNetRemover.Utils;

namespace UniversalNetRemover.Utils
{
    public static class AssemblyHelper
    {
        private static string de4dotPath = @"C:\Users\THE JORDAN\Desktop\UniversalNetRemover\de4dot-net8.0-winx64\de4dot.exe";

        public static byte[] DeobfuscateWithDe4dot(string inputPath, string protectorType)
        {
            string outputPath = Path.GetTempFileName() + "_cleaned.exe";
            string args = $"-p {protectorType} -f \"{inputPath}\" -o \"{outputPath}\"";

            Logger.Info($"Appel de de4dot : {de4dotPath} {args}");
            
            var processStartInfo = new ProcessStartInfo
            {
                FileName = de4dotPath,
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = new Process { StartInfo = processStartInfo };
            process.Start();

            // Timeout de 60 secondes
            if (!process.WaitForExit(60000))
            {
                Logger.Error("de4dot a dépassé le délai de 60 secondes. Arrêt forcé.");
                process.Kill();
                return File.ReadAllBytes(inputPath);
            }

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (!string.IsNullOrEmpty(output))
                Logger.Info(output);
            if (!string.IsNullOrEmpty(error))
                Logger.Warning(error);

            if (File.Exists(outputPath) && new FileInfo(outputPath).Length > 0)
            {
                byte[] data = File.ReadAllBytes(outputPath);
                File.Delete(outputPath);
                Logger.Success($"de4dot a généré {data.Length} octets");
                return data;
            }
            
            Logger.Warning("de4dot n'a pas généré de fichier, retour du fichier original");
            return File.ReadAllBytes(inputPath);
        }

        public static byte[] SimpleClean(byte[] data)
        {
            // Nettoyage basique : supprime les attributs obfusqués
            string text = System.Text.Encoding.UTF8.GetString(data);
            int before = text.Length;
            
            // Supprime les attributs [module: ...] et [assembly: ...]
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\[module:.*?\]", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\[assembly:.*?\]", "");
            // Supprime les attributs obfusqués avec noms bizarres
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\[.*?(ConfusedBy|ObfuscatedBy|PoweredBy|ProcessedBy).*?\]", "");
            
            int after = text.Length;
            Logger.Info($"Nettoyage : {before - after} caractères supprimés");
            
            return System.Text.Encoding.UTF8.GetBytes(text);
        }

        public static bool IsDe4dotAvailable()
        {
            return File.Exists(de4dotPath);
        }
    }
}