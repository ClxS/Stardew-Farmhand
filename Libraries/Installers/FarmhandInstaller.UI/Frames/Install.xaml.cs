namespace Farmhand.Installers.Frames
{
    using System;
    using System.Threading.Tasks;

    using Farmhand.Installers.Utilities;

    /// <summary>
    /// Interaction logic for Install
    /// </summary>
    public partial class Install
    {
        internal static string CommandFinished => "installed";

        internal static string CommandError => "error";

        /// <summary>
        /// Initializes a new instance of the <see cref="Install"/> class.
        /// </summary>
        public Install()
        {
            this.InitializeComponent();
        }

        internal override void ClearFrame()
        {
        }

        internal override void Start()
        {
            TitleInfoService.SetCurrentPage("Installing");
            Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var context = new PackageStatusContext();
                        context.ProgressUpdate += this.Context_ProgressUpdate;
                        context.MessageUpdate += this.Context_MessageUpdate;

                        PackageManager.InstallPackage(context);
                        this.Complete();
                    }
                    catch (Exception ex)
                    {
                        this.Failure(ex);
                    }
                });
        }

        private void Context_MessageUpdate(object sender, EventArgs e)
        {
            var context = sender as PackageStatusContext;
            if (context != null)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.LabelMessage.Content = context.Message;
                }));
            }
        }

        private void Context_ProgressUpdate(object sender, EventArgs e)
        {
            var context = sender as PackageStatusContext;
            if (context != null)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.InstallationProgress.Value = context.Progress;
                }));
            }
        }

        private void Failure(Exception ex)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                InstallationContext.Exception = ex;
                this.OnNavigate(CommandError);
            }));
        }

        internal void Complete()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.OnNavigate(CommandFinished);
            }));
        }
    }
}
