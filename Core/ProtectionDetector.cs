using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversalNetRemover.Models;

namespace UniversalNetRemover.Core
{
    public class ProtectionDetector
    {
        public DetectionResult Detect(byte[] fileData)
        {
            var result = new DetectionResult
            {
                IsObfuscated = false,
                Confidence = 0.2,
                ProtectorName = "Inconnu",
                Version = ""
            };

            try
            {
                // Recherche dans TOUT le fichier (pas limité aux 50000 premiers octets)
                string fullText = Encoding.ASCII.GetString(fileData);
                
                // === SIGNATURES CONFUUSEREX (recherche élargie) ===
                bool hasConfuserStrong = fullText.Contains("ConfusedBy") || 
                                         fullText.Contains("ConfuserEx") ||
                                         fullText.Contains("w{zygAd") ||
                                         fullText.Contains("dT_Fn}") ||
                                         fullText.Contains("DotNetPatcher");
                
                bool hasAntiDebug = fullText.Contains("CheckRemoteDebuggerPresent") ||
                                    fullText.Contains("AntiDebug") ||
                                    fullText.Contains("VirtualProtect");
                
                bool hasAntiDump = fullText.Contains("AntiDump") ||
                                   fullText.Contains("Assembly.Load");

                // === SIGNATURES DES LIBRAIRIES ===
                bool hasCostura = fullText.Contains("Costura") || fullText.Contains("Fody");
                bool hasNewtonsoft = fullText.Contains("Newtonsoft") || fullText.Contains("Json");
                bool hasGuna = fullText.Contains("Guna.UI") || fullText.Contains("Guna.UI2");
                bool hasLog4net = fullText.Contains("log4net");

                // === SECTIONS ÉTRANGES (typiques ConfuserEx packé) ===
                bool hasStrangeSections = fullText.Contains("w{zygAd") || 
                                          fullText.Contains("dT_Fn}") ||
                                          (fullText.Contains(".text") && fullText.Contains(".cctor"));

                // === ENTROPIE (code compressé) ===
                double entropy = CalculateEntropy(fileData.Take(Math.Min(10000, fileData.Length)).ToArray());
                bool hasHighEntropy = entropy > 6.5;

                // === DÉCISION ===
                bool isConfuserEx = hasConfuserStrong || hasAntiDebug || hasAntiDump || hasStrangeSections;
                
                if (isConfuserEx)
                {
                    double confidence = 0.85;
                    if (hasConfuserStrong) confidence += 0.10;
                    if (hasAntiDebug) confidence += 0.05;
                    if (hasAntiDump) confidence += 0.05;
                    if (hasStrangeSections) confidence += 0.10;
                    if (hasHighEntropy) confidence += 0.10;
                    confidence = Math.Min(confidence, 0.95);

                    string libs = "";
                    if (hasCostura) libs += " + Costura.Fody";
                    if (hasNewtonsoft) libs += " + Newtonsoft.Json";
                    if (hasGuna) libs += " + Guna.UI";
                    if (hasLog4net) libs += " + log4net";

                    result = new DetectionResult
                    {
                        IsObfuscated = true,
                        Confidence = confidence,
                        ProtectorName = "ConfuserEx (DotNetPatcher mod)" + libs,
                        Version = "1.x (DotNetPatcher mod)"
                    };
                }
                else if (hasCostura || hasNewtonsoft || hasGuna || hasLog4net)
                {
                    // Au moins des librairies .NET présentes
                    string libs = "";
                    if (hasCostura) libs += " + Costura.Fody";
                    if (hasNewtonsoft) libs += " + Newtonsoft.Json";
                    if (hasGuna) libs += " + Guna.UI";
                    if (hasLog4net) libs += " + log4net";
                    
                    result = new DetectionResult
                    {
                        IsObfuscated = false,
                        Confidence = 0.40,
                        ProtectorName = "Application .NET (non obfusquée)" + libs,
                        Version = ""
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur détection: {ex.Message}");
            }

            return result;
        }

        private double CalculateEntropy(byte[] data)
        {
            if (data == null || data.Length == 0) return 0;
            
            var freq = new Dictionary<byte, int>();
            foreach (byte b in data)
            {
                if (freq.ContainsKey(b))
                    freq[b]++;
                else
                    freq[b] = 1;
            }

            double entropy = 0;
            int length = data.Length;
            foreach (var count in freq.Values)
            {
                double p = (double)count / length;
                entropy -= p * Math.Log(p, 2);
            }
            return entropy;
        }
    }
}