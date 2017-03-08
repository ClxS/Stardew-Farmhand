namespace Farmhand.Installers
{
    using System;
    using System.Collections.Generic;

    using Farmhand.Installers.Frames;
    using Farmhand.Installers.Utilities;

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
            
            TitleInfoService.TitleInfoElement = this.ButtonRightTitle;

            var frameWelcome = new Welcome();
            var framePackageSelect = new PackageSelection();
            var frameDevPackageSelect = new DeveloperPackageSelection();
            var framePlayerPaths = new PlayerFindPaths();
            var frameDevPaths = new DeveloperFindPaths();
            var frameEmptyMod = new CreateEmptyMod();
            var frameInstall = new Install();
            var frameFinished = new Finished();
            var frameError = new Error();

            var frames = new BaseFrame[]
                             {
                                 frameWelcome, framePackageSelect, frameDevPackageSelect, framePlayerPaths,
                                 frameDevPaths, frameInstall, frameFinished, frameError, frameEmptyMod
                             };

            this.frameManager.Initialize(this.contentWrapper);

            this.frameManager.RegisterFrame(
                frameWelcome, 
                new FlowInformation
            {
                Element = frameWelcome,
                TransitionCommands = new Dictionary<string, BaseFrame>
                {
                    { Welcome.CommandNext, framePackageSelect }
                }
            });

            this.frameManager.RegisterFrame(
                framePackageSelect,
                new FlowInformation
                {
                    Element = framePackageSelect,
                    TransitionCommands = new Dictionary<string, BaseFrame>
                {
                    { PackageSelection.CommandPlayerPackage, framePlayerPaths },
                    { PackageSelection.CommandDeveloperPackage, frameDevPackageSelect }
                }
            });

            this.frameManager.RegisterFrame(
                frameDevPackageSelect,
                new FlowInformation
                {
                    Element = frameDevPackageSelect,
                    TransitionCommands = new Dictionary<string, BaseFrame>
                {
                    { DeveloperPackageSelection.CommandNext, frameDevPaths },
                    { DeveloperPackageSelection.CommandCreateEmptyMod, frameEmptyMod },
                }
                });

            this.frameManager.RegisterFrame(
                frameEmptyMod,
                new FlowInformation
                {
                    Element = frameEmptyMod,
                    TransitionCommands = new Dictionary<string, BaseFrame>
                {
                    { CreateEmptyMod.CommandNext, frameDevPaths },
                }
                });

            this.frameManager.RegisterFrame(
                frameDevPaths,
                new FlowInformation
                {
                    Element = frameDevPaths,
                    TransitionCommands = new Dictionary<string, BaseFrame>
                {
                    { DeveloperFindPaths.CommandInstall, frameInstall },
                }
                });

            this.frameManager.RegisterFrame(
                framePlayerPaths,
                new FlowInformation
                {
                    Element = framePlayerPaths,
                    TransitionCommands = new Dictionary<string, BaseFrame>
                {
                    { PlayerFindPaths.CommandInstall, frameInstall },
                }
                });

            this.frameManager.RegisterFrame(
                frameInstall,
                new FlowInformation
                {
                    Element = frameInstall,
                    TransitionCommands = new Dictionary<string, BaseFrame>
                {
                    { Install.CommandFinished, frameFinished },
                    { Install.CommandError, frameError }
                }
                });

            foreach (var frame in frames)
            {
                frame.Navigate += this.Frame_Navigate;
                frame.Back += this.Frame_Back;
            }

            this.frameManager.DisplayInitialFrame(frameWelcome);
        }

        private void Frame_Back(object sender, EventArgs e)
        {
            this.frameManager.Back();
        }

        private void Frame_Navigate(object sender, EventArgsFrameCommand e)
        {
            this.frameManager.HandleCommand(e.Command);
        }
    }
}
