using Farmhand;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Animation;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Pass
        {
            PassOne,
            PassTwo
        }

        private string StardewFile { get; set; }
        private BackgroundWorker Packer { get; set; }

        public MainWindow()
        {
            //Add Constants
            Resources.Add("ProgramName", Constants.ProgramName);

            InitializeComponent();
            canvasLoading.IsHitTestVisible = false;
            canvasComplete.IsHitTestVisible = false;
        }

        private void buttonFindFile_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".exe";
            dlg.Filter = "Executable Files|*.exe";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                textBoxExecPath.Text = filename;
                StardewFile = filename;
            }
        }

        private void buttonInstall_Click(object sender, RoutedEventArgs e)
        {
            SwitchToInstallationUI();
            Packer = new BackgroundWorker();
            Packer.DoWork += Packer_DoWork;
            Packer.ProgressChanged += Packer_ProgressChanged;
            Packer.RunWorkerCompleted += Packer_RunWorkerCompleted;
            Packer.WorkerReportsProgress = true;
            Packer.RunWorkerAsync();
        }

        private void Packer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            canvasComplete.Opacity = 1.0f;
            canvasLoading.Opacity = 0.0f;
            canvasComplete.IsHitTestVisible = true;
            canvasLoading.IsHitTestVisible = false;
        }

        private void Packer_DoWork(object sender, DoWorkEventArgs e)
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
            var outputDirectory = Path.GetDirectoryName(StardewFile);

            try
            {
                var FarmhandExe = outputDirectory + "\\Stardew Farmhand.exe";

                Packer.ReportProgress(10, "Creating Temp Directory");
                Directory.CreateDirectory(tempDirectory);
                Directory.SetCurrentDirectory(tempDirectory);
                File.Copy(StardewFile, tempDirectory + "\\Stardew Valley.exe");
                
                Packer.ReportProgress(20, "Unpackaging Contents");
                ExtractDll(tempDirectory);
                
                Packer.ReportProgress(40, "Injecting Stardew Valley - Pass 1");
                DoInstallationPass1(StardewFile);
                
                Packer.ReportProgress(60, "Injecting Stardew Valley - Pass 2");
                DoInstallationPass2(FarmhandExe);

                Packer.ReportProgress(60, "Copying SMAPI Compatibility Layer");
                File.Copy("StardewModdingAPI.dll", outputDirectory + "\\StardewModdingAPI.dll");
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                Packer.ReportProgress(80, "Clearing Temporary Files");
                if (Directory.Exists(tempDirectory))
                {
                    try
                    {
                        Directory.Delete(tempDirectory, true);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            Packer.ReportProgress(100, "Installation Complete");
        }

        static Patcher CreatePatcher(Pass pass)
        {
            Assembly patcherAssembly = Assembly.LoadFrom(pass == Pass.PassOne ? "FarmhandPatcherFirstPass.dll" : "FarmhandPatcherSecondPass.dll");
            Type patcherType = patcherAssembly.GetType(pass == Pass.PassOne ? "Farmhand.PatcherFirstPass" : "Farmhand.PatcherSecondPass");
            object patcher = Activator.CreateInstance(patcherType);
            return patcher as Patcher;
        }

        private void DoInstallationPass1(string fileName)
        {
            var patcher = CreatePatcher(Pass.PassOne);
            patcher.PatchStardew(Path.GetFileName(fileName));
        }

        private void DoInstallationPass2(string fileName)
        {
            var patcher = CreatePatcher(Pass.PassTwo);
            patcher.PatchStardew(fileName);
        }

        private void Packer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var text = e.UserState as string;
            if (text != null)
            {
                SetInstallationProgress(text, e.ProgressPercentage);
            }
        }

        private void ExtractDll(string path)
        {
            var test = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(n => n.Contains(".Payload."));
            foreach (var file in test)
            {
                using (System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(file))
                {
                    var shortName = file.Replace("FarmhandInstaller.Payload.", "");
                    using (System.IO.FileStream fileStream = new System.IO.FileStream(System.IO.Path.Combine(path, shortName), System.IO.FileMode.Create))
                    {
                        for (int i = 0; i < stream.Length; i++)
                        {
                            fileStream.WriteByte((byte)stream.ReadByte());
                        }
                        fileStream.Close();
                    }
                }
            }
        }

        private void SwitchToInstallationUI()
        {
            SetInstallationProgress("Starting Installation...", 0);
            canvasSelectLocation.Opacity = 0.0f;
            canvasLoading.Opacity = 1.0f;
            canvasSelectLocation.IsHitTestVisible = false;
            canvasLoading.IsHitTestVisible = true;
            //return;

            //Storyboard sb = new Storyboard();

            //DoubleAnimation fadeOut = new DoubleAnimation(1, 0, new Duration(new TimeSpan(0, 0, 0, 500)));
            //DoubleAnimation fadeIn = new DoubleAnimation(0, 1, new Duration(new TimeSpan(0, 0, 0, 500)));


            //Storyboard.SetTargetName(fadeOut, "canvasSelectLocation");
            //Storyboard.SetTargetProperty(fadeOut, new PropertyPath("(Opacity)"));
            //Storyboard.SetTargetName(fadeIn, "canvasLoading");
            //Storyboard.SetTargetProperty(fadeIn, new PropertyPath("(Opacity)"));

            //sb.Children.Add(fadeOut);
            //sb.Children.Add(fadeIn);
            //gridMainContainer.BeginStoryboard(sb);

        }

        public void SetInstallationProgress(string progressStr, int value)
        {
            labelProgressStatus.Content = progressStr;
            progressInstallation.Value = value;
        }

        private void buttonCancelInstallation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
