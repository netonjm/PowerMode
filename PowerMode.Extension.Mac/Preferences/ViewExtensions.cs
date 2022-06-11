using AppKit;

namespace PowerMode.Extension.Preferences
{
    static class ViewExtensions
    {
		public static void AddArrangedSpace(this NSStackView view)
		{
			view.AddArrangedSubview(new NSView() { TranslatesAutoresizingMaskIntoConstraints = false });
		}
		
		public static NSTextField AddArrangedTitle(this NSStackView view, string title)
        {
			var enableLabel = NSTextField.CreateLabel(title);
			enableLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			enableLabel.Alignment = NSTextAlignment.Right;
			enableLabel.WidthAnchor.ConstraintEqualTo(140).Active = true;
			view.AddArrangedSubview(enableLabel);
			return enableLabel;
		}
	}
}

