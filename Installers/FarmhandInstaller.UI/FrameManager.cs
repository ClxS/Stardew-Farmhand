namespace Farmhand.Installers
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;

    using Farmhand.Installers.Frames;
    
    internal class FrameManager
    {
        private Dictionary<BaseFrame, FlowInformation> FlowInformation { get; } = new Dictionary<BaseFrame, FlowInformation>();
        
        private Stack<BaseFrame> FrameStack { get; } = new Stack<BaseFrame>();

        private BaseFrame CurrentFrame { get; set; }

        private Panel ParentContainer { get; set; }

        public void Initialize(Panel parentContainer)
        {
            this.ParentContainer = parentContainer;
        }

        public void RegisterFrame(BaseFrame frame, FlowInformation flowInformation)
        {
            if (this.FlowInformation.ContainsKey(frame))
            {
                throw new Exception("This frame has already been registered");
            }

            this.FlowInformation[frame] = flowInformation;
        }

        public void DisplayInitialFrame(BaseFrame frame)
        {
            if (!this.FlowInformation.ContainsKey(frame))
            {
                throw new Exception("This frame has already not been registered");
            }

            this.CurrentFrame = frame;
            this.FrameStack.Push(frame);
            this.UpdateDisplayedFrame();
        }

        public void Back()
        {
            if (this.FrameStack.Count > 1)
            {
                this.FrameStack.Pop();
                this.CurrentFrame = this.FrameStack.Peek();
                this.UpdateDisplayedFrame();
            }
        }

        public void HandleCommand(string command)
        {
            if (this.FlowInformation.ContainsKey(this.CurrentFrame))
            {
                var info = this.FlowInformation[this.CurrentFrame];
                if (info.TransitionCommands.ContainsKey(command))
                {
                    var nextFrame = info.TransitionCommands[command];
                    this.CurrentFrame = nextFrame;
                    this.FrameStack.Push(this.CurrentFrame);
                    this.UpdateDisplayedFrame();
                }
            }
        }

        private void UpdateDisplayedFrame()
        {
            this.CurrentFrame.ClearFrame();

            this.ParentContainer.Children.Clear();
            this.ParentContainer.Children.Add(this.CurrentFrame);

            this.CurrentFrame.Start();
        }
    }
}
