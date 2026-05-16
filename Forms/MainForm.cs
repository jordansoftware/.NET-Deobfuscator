#nullable disable
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniversalNetRemover.Core;
using UniversalNetRemover.Models;
using UniversalNetRemover.Utils;

namespace UniversalNetRemover.Forms
{
    public partial class MainForm : Form
    {
        // Champs
        private Button btnDetect;
        private Button btnStart;
        private TextBox txtDetectionResult;
        private TextBox txtLogs;
        private ComboBox cmbProtector;
        private CheckedListBox chkActions;
        private RadioButton rbRapide;
        private RadioButton rbExpert;
        private Panel dropPanel;
        private ProgressBar progressBar;
        private string currentFilePath;

        public MainForm()
        {
            InitializeComponent();
            SetupDragDrop();
            Logger.OnLog += AppendLog;
        }

        private void InitializeComponent()
        {
            this.Text = "Universal .NET Defender Remover";
            this.Size = new System.Drawing.Size(950, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // ÉTAPE 1 : Détection
            var lblStep1 = new Label
            {
                Text = "ÉTAPE 1 : Détection automatique",
                Location = new System.Drawing.Point(15, 15),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold),
                Size = new System.Drawing.Size(300, 25)
            };

            btnDetect = new Button
            {
                Text = "[ DÉTECTER LA PROTECTION ]",
                Location = new System.Drawing.Point(15, 45),
                Size = new System.Drawing.Size(200, 35),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDetect.Click += BtnDetect_Click;

            txtDetectionResult = new TextBox
            {
                Location = new System.Drawing.Point(225, 48),
                Size = new System.Drawing.Size(400, 25),
                ReadOnly = true,
                BackColor = System.Drawing.Color.LightYellow,
                Font = new System.Drawing.Font("Consolas", 10)
            };

            // ÉTAPE 2 : Mode
            var lblStep2 = new Label
            {
                Text = "ÉTAPE 2 : Sélectionner le mode",
                Location = new System.Drawing.Point(15, 95),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold),
                Size = new System.Drawing.Size(300, 25)
            };

            rbRapide = new RadioButton
            {
                Text = "Mode Rapide (Défaut)",
                Location = new System.Drawing.Point(15, 125),
                Size = new System.Drawing.Size(200, 25),
                Checked = true
            };

            rbExpert = new RadioButton
            {
                Text = "Mode Expert (Personnalisé)",
                Location = new System.Drawing.Point(15, 155),
                Size = new System.Drawing.Size(200, 25)
            };
            rbExpert.CheckedChanged += RbExpert_CheckedChanged;

            var lblProtector = new Label
            {
                Text = "Famille de Protection",
                Location = new System.Drawing.Point(250, 125),
                Size = new System.Drawing.Size(150, 25),
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold)
            };

            cmbProtector = new ComboBox
            {
                Location = new System.Drawing.Point(250, 150),
                Size = new System.Drawing.Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };
            cmbProtector.Items.AddRange(new[] { "crx", "cr", "un", "ef", "sa" });
            cmbProtector.SelectedIndex = 0;

            var lblActions = new Label
            {
                Text = "Actions à exécuter",
                Location = new System.Drawing.Point(500, 125),
                Size = new System.Drawing.Size(200, 25),
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold)
            };

            chkActions = new CheckedListBox
            {
                Location = new System.Drawing.Point(500, 150),
                Size = new System.Drawing.Size(400, 140),
                CheckOnClick = true,
                Enabled = false
            };
            string[] actions = { "Déchiffrer les strings", "Nettoyer le flux de contrôle", "Supprimer l'Anti-Tamper" };
            chkActions.Items.AddRange(actions);
            for (int i = 0; i < chkActions.Items.Count; i++)
                chkActions.SetItemChecked(i, true);

            // ÉTAPE 3 : Fichier
            var lblStep3 = new Label
            {
                Text = "ÉTAPE 3 : Fichier & Exécution",
                Location = new System.Drawing.Point(15, 320),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold),
                Size = new System.Drawing.Size(300, 25)
            };

            dropPanel = new Panel
            {
                BackColor = System.Drawing.Color.LightGray,
                Location = new System.Drawing.Point(15, 350),
                Size = new System.Drawing.Size(900, 70),
                BorderStyle = BorderStyle.FixedSingle,
                AllowDrop = true
            };

            var lblDrop = new Label
            {
                Text = "GLISSEZ-DÉPOSEZ VOTRE EXE/DLL ICI",
                Location = new System.Drawing.Point(320, 25),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkGray
            };
            dropPanel.Controls.Add(lblDrop);

            progressBar = new ProgressBar
            {
                Location = new System.Drawing.Point(15, 435),
                Size = new System.Drawing.Size(900, 25),
                Style = ProgressBarStyle.Blocks
            };

            btnStart = new Button
            {
                Text = "[ START DECOY ]",
                Location = new System.Drawing.Point(15, 475),
                Size = new System.Drawing.Size(180, 45),
                BackColor = System.Drawing.Color.ForestGreen,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            btnStart.Click += BtnStart_Click;

            txtLogs = new TextBox
            {
                Location = new System.Drawing.Point(15, 535),
                Size = new System.Drawing.Size(900, 170),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                BackColor = System.Drawing.Color.Black,
                ForeColor = System.Drawing.Color.LightGreen,
                Font = new System.Drawing.Font("Consolas", 9)
            };

            // Ajout des contrôles
            Controls.AddRange(new Control[] {
                lblStep1, btnDetect, txtDetectionResult,
                lblStep2, rbRapide, rbExpert, lblProtector, cmbProtector, lblActions, chkActions,
                lblStep3, dropPanel, progressBar, btnStart, txtLogs
            });
        }

        private void SetupDragDrop()
        {
            dropPanel.DragEnter += (s, e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.Copy;
            };

            dropPanel.DragDrop += (s, e) =>
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    currentFilePath = files[0];
                    AppendLog($"[INFO] Fichier chargé : {currentFilePath}");
                    dropPanel.BackColor = System.Drawing.Color.LightGreen;
                }
            };
        }

        private void AppendLog(string msg)
        {
            if (txtLogs.InvokeRequired)
                txtLogs.Invoke(() => txtLogs.AppendText(msg + Environment.NewLine));
            else
                txtLogs.AppendText(msg + Environment.NewLine);
        }

        private void RbExpert_CheckedChanged(object sender, EventArgs e)
        {
            bool isExpert = rbExpert.Checked;
            chkActions.Enabled = isExpert;
            cmbProtector.Enabled = isExpert;
        }

        private async void BtnDetect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                AppendLog("[ERREUR] Aucun fichier chargé. Glissez-déposez un fichier d'abord.");
                return;
            }

            try
            {
                AppendLog("[DÉTECTION] Analyse en cours...");
                var detector = new ProtectionDetector();
                var data = File.ReadAllBytes(currentFilePath);
                var result = detector.Detect(data);
                txtDetectionResult.Text = result.ToString();
                AppendLog($"[DÉTECTION] Résultat : {result}");
            }
            catch (Exception ex)
            {
                AppendLog($"[ERREUR] Détection échouée : {ex.Message}");
            }
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                AppendLog("[ERREUR] Aucun fichier chargé.");
                return;
            }

            if (!File.Exists(currentFilePath))
            {
                AppendLog($"[ERREUR] Fichier introuvable : {currentFilePath}");
                return;
            }

            btnStart.Enabled = false;
            progressBar.Style = ProgressBarStyle.Marquee;
            dropPanel.BackColor = System.Drawing.Color.LightGray;

            try
            {
                var data = File.ReadAllBytes(currentFilePath);
                var actions = rbExpert.Checked
                    ? chkActions.CheckedItems.Cast<string>().ToList()
                    : new System.Collections.Generic.List<string>();

                string protectorType = cmbProtector.SelectedItem?.ToString() ?? "crx";

                AppendLog($"[INFO] Lancement de la désobfuscation...");
                AppendLog($"[INFO] Mode : {(rbExpert.Checked ? "Expert" : "Rapide")}");
                AppendLog($"[INFO] Protecteur : {protectorType}");
                AppendLog($"[INFO] Actions : {(actions.Count == 0 ? "Toutes (mode rapide)" : string.Join(", ", actions))}");

                var pipeline = new DeobfuscationPipeline();
                var result = await pipeline.ExecuteAsync(data, actions, currentFilePath, protectorType);

                if (result.Success && result.ModifiedData != null)
                {
                    string outPath = Path.Combine(
                        Path.GetDirectoryName(currentFilePath),
                        Path.GetFileNameWithoutExtension(currentFilePath) + "_Slayed.exe"
                    );
                    File.WriteAllBytes(outPath, result.ModifiedData);
                    AppendLog($"[SUCCÈS] Fichier sauvegardé : {outPath}");
                    MessageBox.Show($"Désobfuscation terminée !\nFichier : {outPath}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    AppendLog($"[ERREUR] Échec : {result.ErrorMessage}");
                    MessageBox.Show($"Échec de la désobfuscation :\n{result.ErrorMessage}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"[EXCEPTION] {ex.Message}");
                MessageBox.Show($"Exception : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnStart.Enabled = true;
                progressBar.Style = ProgressBarStyle.Blocks;
            }
        }
    }
}