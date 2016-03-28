using Revolution;
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

        public MainWindow()
        {
            //Add Constants
            Resources.Add("ProgramName", Constants.ProgramName);

            InitializeComponent();
            canvasLoading.IsHitTestVisible = false;

            buttonInstall_Click(null, new RoutedEventArgs());
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
            }
        }

        private void buttonInstall_Click(object sender, RoutedEventArgs e)
        {
            SwitchToInstallationUI();
            BackgroundWorker packer = new BackgroundWorker();
            packer.DoWork += Packer_DoWork;
            packer.ProgressChanged += Packer_ProgressChanged;
            packer.RunWorkerAsync();
        }

        private void Packer_DoWork(object sender, DoWorkEventArgs e)
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
            
            try
            {
                var fileName = @"Z:\Games\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe";
                //SetInstallationProgress("Creating Temp Directory", 1);
                var outputDirectory = Path.GetDirectoryName(fileName);
                Directory.CreateDirectory(tempDirectory);
                Directory.SetCurrentDirectory(tempDirectory);

                //SetInstallationProgress("Unpackaging Contents", 20);
                ExtractDll(tempDirectory);

                //SetInstallationProgress("Injecting Stardew Valley - Pass 1", 40);
                DoInstallationPass1(fileName);

                //SetInstallationProgress("Injecting Stardew Valley - Pass 2", 60);
                DoInstallationPass2();
            }
            finally
            {
                //SetInstallationProgress("Clearing Temporary Files", 80);
                if (Directory.Exists(tempDirectory))
                {
                    Directory.Delete(tempDirectory, true);
                }
            }

            //SetInstallationProgress("Installation Complete", 100);
        }

        static Patcher CreatePatcher(Pass pass)
        {
            Assembly patcherAssembly = Assembly.LoadFrom(pass == Pass.PassOne ? "RevolutionPatcherFirstPass.dll" : "RevolutionPatcherSecondPass.dll");
            Type patcherType = patcherAssembly.GetType(pass == Pass.PassOne ? "Revolution.PatcherFirstPass" : "Revolution.PatcherSecondPass");
            object patcher = Activator.CreateInstance(patcherType);
            return patcher as Patcher;
        }

        private void DoInstallationPass1(string fileName)
        {
            var patcher = CreatePatcher(Pass.PassOne);
            patcher.PatchStardew();
        }

        private void DoInstallationPass2()
        {

        }

        private void Packer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        private void ExtractDll(string path)
        {
            var test = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(n => n.Contains(".Payload."));
            foreach (var file in test)
            {
                using (System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(file))
                {
                    var shortName = file.Replace("RevolutionInstaller.Payload.", "");
                    using (System.IO.FileStream fileStream = new System.IO.FileStream(System.IO.Path.Combine(path, file), System.IO.FileMode.Create))
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
    }
}
