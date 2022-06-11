using System;
using System.ComponentModel.Composition;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace PowerMode.Extension.Mac
{
    [Export(typeof(ICocoaTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public class TextAdornmentTextViewCreationListener : ICocoaTextViewCreationListener
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
        public ITextDocumentFactoryService textDocumentFactory { get; set; }

        public void TextViewCreated(ICocoaTextView textView)
        {
            new PowerModeAdornment(textView, textDocumentFactory);
        }
    }
}

