using System;
using System.ComponentModel.Composition;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Composition;
using MonoDevelop.Ide.Gui.Documents;

namespace PowerMode.Extension.Mac
{
    [Export(typeof(ICocoaTextViewCreationListener))]
    [ContentType("any")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public class PowerModeCreationListener : ICocoaTextViewCreationListener
    {
        /// <summary>
        /// Defines the adornment layer for the scarlet adornment. This layer is ordered
        /// after the selection layer in the Z-order
        /// </summary>
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("PowerModeAdornment")]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        private AdornmentLayerDefinition editorAdornmentLayer;

        [Import]
        public IPowerModeSession Session { get; set; }

        [Import]
        public ITextDocumentFactoryService textDocumentFactory { get; set; }

        public void TextViewCreated(ICocoaTextView textView)
            => MonoDevelop.Core.Runtime.RunInMainThread(() => Session.Configure(textView));
    }
}

