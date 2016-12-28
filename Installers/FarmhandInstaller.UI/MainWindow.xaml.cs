namespace FarmhandInstaller.UI
{
    using System.Collections.Generic;

    using FarmhandInstaller.UI.Utilities;
    using FarmhandInstaller.UI.Utilities.Packages;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow
    {
        private readonly FrameManager frameManager = new FrameManager();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            
            var frameWelcome = new Frames.Welcome();
            var framePackageSelect = new Frames.PackageSelection();
            var frameDevPackageSelect = new Frames.DeveloperPackageSelection();
            var framePlayerPaths = new Frames.PlayerFindPaths();
            var frameDevPaths = new Frames.DeveloperFindPaths();
            var frameInstall = new Frames.Install();
            var frameFinished = new Frames.Finished();
            var frameError = new Frames.Error();

            var frames = new Frames.BaseFrame[]
                             {
                                 frameWelcome, framePackageSelect, frameDevPackageSelect, framePlayerPaths,
                                 frameDevPaths, frameInstall, frameFinished, frameError
                             };

            this.frameManager.Initialize(this.contentWrapper);

            this.frameManager.RegisterFrame(
                frameWelcome, 
                new FlowInformation
            {
                Element = frameWelcome,
                TransitionCommands = new Dictionary<string, Frames.BaseFrame>
                {
                    { Frames.Welcome.CommandNext, framePackageSelect }
                }
            });

            this.frameManager.RegisterFrame(
                framePackageSelect,
                new FlowInformation
                {
                    Element = framePackageSelect,
                    TransitionCommands = new Dictionary<string, Frames.BaseFrame>
                {
                    { Frames.PackageSelection.CommandPlayerPackage, framePlayerPaths },
                    { Frames.PackageSelection.CommandDeveloperPackage, frameDevPackageSelect }
                }
            });

            this.frameManager.RegisterFrame(
                frameDevPackageSelect,
                new FlowInformation
                {
                    Element = frameDevPackageSelect,
                    TransitionCommands = new Dictionary<string, Frames.BaseFrame>
                {
                    { Frames.DeveloperPackageSelection.CommandDevPackage, frameDevPaths },
                }
                });

            this.frameManager.RegisterFrame(
                frameDevPaths,
                new FlowInformation
                {
                    Element = frameDevPaths,
                    TransitionCommands = new Dictionary<string, Frames.BaseFrame>
                {
                    { Frames.DeveloperFindPaths.CommandInstall, frameInstall },
                }
                });

            this.frameManager.RegisterFrame(
                framePlayerPaths,
                new FlowInformation
                {
                    Element = framePlayerPaths,
                    TransitionCommands = new Dictionary<string, Frames.BaseFrame>
                {
                    { Frames.PlayerFindPaths.CommandInstall, frameInstall },
                }
                });

            this.frameManager.RegisterFrame(
                frameInstall,
                new FlowInformation
                {
                    Element = frameInstall,
                    TransitionCommands = new Dictionary<string, Frames.BaseFrame>
                {
                    { Frames.Install.CommandFinished, frameFinished },
                    { Frames.Install.CommandError, frameError }
                }
                });

            foreach (var frame in frames)
            {
                frame.Navigate += this.Frame_Navigate;
                frame.Back += this.Frame_Back;
            }

            this.frameManager.DisplayInitialFrame(frameWelcome);
        }

        private void Frame_Back(object sender, System.EventArgs e)
        {
            this.frameManager.Back();
        }

        private void Frame_Navigate(object sender, Frames.EventArgsFrameCommand e)
        {
            this.frameManager.HandleCommand(e.Command);
        }
    }
}
