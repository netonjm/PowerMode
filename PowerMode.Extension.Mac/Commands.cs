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
            IDocumentPowerSession session = CompositionManager.Instance.GetExportedValue<IDocumentPowerSession>();
            ICocoaTextView textView = e.Document.GetContent<ICocoaTextView>();
            session.SetTextView(textView);
        }
    }
}
