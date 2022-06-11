using AppKit;

namespace PowerMode.Extension.Preferences
{
    internal class StackView : AppKit.NSStackView
	{
		public override bool IsFlipped => true;

		public StackView(AppKit.NSUserInterfaceLayoutOrientation orientation = NSUserInterfaceLayoutOrientation.Vertical, float spacing = 10)
		{
			TranslatesAutoresizingMaskIntoConstraints = false;
			Orientation = orientation;
			Spacing = spacing;
			Distribution = NSStackViewDistribution.Fill;
			if (orientation == NSUserInterfaceLayoutOrientation.Vertical)
				Alignment = NSLayoutAttribute.Leading;
			else
				Alignment = NSLayoutAttribute.CenterY;
		}
	}
}

