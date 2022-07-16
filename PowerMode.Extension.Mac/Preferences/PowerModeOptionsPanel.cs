using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Dialogs;
using System.ComponentModel.Composition;
using static Microsoft.VisualStudio.Shell.ThreadedWaitDialogHelper;

namespace PowerMode.Extension.Preferences
{
    class PowerModeOptionsPanel : OptionsPanel
	{
		PowerModeOptionsPanelViewController widget;

		public override Control CreatePanelWidget()
		{
			widget ??= new PowerModeOptionsPanelViewController();
			return widget.View;
		}

        public override bool ValidateChanges()
        {
           return widget.ValidateChanges();
        }

        public override void ApplyChanges()
		{
			widget.ApplyChanges();
		}
	}
}

