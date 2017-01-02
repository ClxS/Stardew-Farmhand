namespace Farmhand.Installers.Utilities
{
    using System;

    internal class PackageStatusContext
    {
        public event EventHandler ProgressUpdate = delegate { };

        public event EventHandler MessageUpdate = delegate { };

        private double progress = 0;

        public double Progress
        {
            get
            {
                return this.progress;
            }

            set
            {
                this.progress = value;
                this.OnProgressUpdate();
            }
        }

        private string message = string.Empty;

        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.message = value;
                this.OnMessageUpdate();
            }
        }

        protected virtual void OnProgressUpdate()
        {
            this.ProgressUpdate(this, EventArgs.Empty);
        }

        protected virtual void OnMessageUpdate()
        {
            this.MessageUpdate(this, EventArgs.Empty);
        }

        public void SetState(double stateProgress, string stateMessage)
        {
            this.Progress = stateProgress;
            this.Message = stateMessage;
        }
    }
}
