using System.ComponentModel.Composition;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace PowerMode.Extension.Mac
{
    [Export(typeof(ICocoaTextViewCreationListener))]
    [ContentType("any")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public class BackgroundCreationListener : ICocoaTextViewCreationListener
    {
        /// <summary>
        /// Defines the adornment layer for the scarlet adornment. This layer is ordered
        /// after the selection layer in the Z-order
        /// </summary>
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("PowerModeBackgroundAdornment")]
        [Order(Before = PredefinedAdornmentLayers.Text)]
        private AdornmentLayerDefinition editorAdornmentLayer;

        [Import]
        public IBackgroundSession Session { get; set; }

        [Import]
        public ITextDocumentFactoryService textDocumentFactory { get; set; }

        public void TextViewCreated(ICocoaTextView textView)
            => MonoDevelop.Core.Runtime.RunInMainThread(() => Session.Configure(textView));
    }
}

