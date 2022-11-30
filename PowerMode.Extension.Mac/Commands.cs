using MonoDevelop.Components.Commands;
using MonoDevelop;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Composition;
using static Microsoft.VisualStudio.Shell.ThreadedWaitDialogHelper;
using Microsoft.VisualStudio.Text.Editor;

namespace PowerMode
{
    class InitCommandHandler : CommandHandler
    {
        protected override void Run()
        {
            IdeApp.Initialized += (s, e) =>
            {
                IdeApp.Workbench.ActiveDocumentChanged += Workbench_ActiveDocumentChanged;
            };
        }

        void Workbench_ActiveDocumentChanged(object sender, MonoDevelop.Ide.Gui.DocumentEventArgs e)
        {
            var document = e.Document;
            if (document == null)
                return;

            ICocoaTextView textView = document.GetContent<ICocoaTextView>();

            IBackgroundSession background = CompositionManager.Instance.GetExportedValue<IBackgroundSession>();
            if (background != null)
                background.Configure(textView);

            IPowerModeSession session = CompositionManager.Instance.GetExportedValue<IPowerModeSession>();
            if (session != null)
                session.Configure(textView);
        }
    }
}
