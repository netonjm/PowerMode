using AppKit;
using System;

namespace PowerMode.Extension.Preferences
{
    static class ViewHelper
	{
        public static void AttachHorizontally(NSView parent, NSView view)
        {
            view.LeadingAnchor.ConstraintEqualTo(parent.LeadingAnchor).Active = true;
            view.TrailingAnchor.ConstraintEqualTo(parent.TrailingAnchor).Active = true;
        }

        public static NSTextField CreateNumericCardView(NSStackView effectsRow, string label)
        {
            effectsRow.AddArrangedTitle(label);
            var effectsPopupButton = CreateNumericTextField();
            effectsPopupButton.WidthAnchor.ConstraintEqualTo(350).Active = true;
            effectsRow.AddArrangedSubview(effectsPopupButton);
            effectsRow.AddArrangedSpace();
            return effectsPopupButton;
        }

        public static NSSlider CreateSliderCardView(NSStackView effectsRow, string label)
        {
            effectsRow.AddArrangedTitle(label);
            var effectsPopupButton = new NSSlider() { TranslatesAutoresizingMaskIntoConstraints = false };
            effectsPopupButton.WidthAnchor.ConstraintEqualTo(100).Active = true;
            effectsRow.AddArrangedSubview(effectsPopupButton);
            effectsRow.AddArrangedSpace();
            return effectsPopupButton;
        }

        public static NSPopUpButton CreatePopupButtonCardView(NSStackView effectsRow, string label)
        {
            effectsRow.AddArrangedTitle(label);
            var effectsPopupButton = CreatePopupButton();
            effectsPopupButton.WidthAnchor.ConstraintEqualTo(350).Active = true;
            effectsRow.AddArrangedSubview(effectsPopupButton);
            effectsRow.AddArrangedSpace();
            return effectsPopupButton;
        }

        public static NSButton CreateCheckBoxCardView(NSStackView nameRow, string label, string description)
        {
            nameRow.AddArrangedTitle(label);

            var enableCheckBox = CreateCheckBox();
            enableCheckBox.Title = description;
            nameRow.AddArrangedSubview(enableCheckBox);
            nameRow.AddArrangedSpace();
            return enableCheckBox;
        }

        public static void CreateSubLabel(NSStackView stackview, string label)
        {
            var descriptionEffectRow = new StackView(NSUserInterfaceLayoutOrientation.Horizontal);
            stackview.AddArrangedSubview(descriptionEffectRow);

            var descriptionLabel = NSTextField.CreateLabel(label);
            descriptionLabel.AlphaValue = 0.7f;
            descriptionLabel.WidthAnchor.ConstraintEqualTo(350).Active = true;
            descriptionLabel.SetContentCompressionResistancePriority(250, NSLayoutConstraintOrientation.Horizontal);
            descriptionLabel.LineBreakMode = NSLineBreakMode.ByWordWrapping;

            descriptionEffectRow.AddArrangedTitle(string.Empty);
            descriptionEffectRow.AddArrangedSubview(descriptionLabel);
            descriptionEffectRow.AddArrangedSpace();
        }

        public static NSButton CreateCheckBox()
        {
            var enableCheckBox = new NSButton() { Title = String.Empty, TranslatesAutoresizingMaskIntoConstraints = false, };
            enableCheckBox.SetButtonType(NSButtonType.Switch);
            return enableCheckBox;
        }

        public static NSTextField CreateNumericTextField()
        {
            var enableCheckBox = new NSTextField() { TranslatesAutoresizingMaskIntoConstraints = false, };
            return enableCheckBox;
        }

        //     NSOpenPanel CreateSelectFileField(NSStackView stackview, string title)
        //     {
        //         //var openPanel =new NSOpenPanel();
        //         //openPanel.AllowsMultipleSelection = false;
        //         //openPanel.CanChooseDirectories = false;
        //         //openPanel.CanCreateDirectories = false;
        //         //openPanel.CanChooseFiles = true;

        //CreateCardRow(stackview, openPanel);


        //         return openPanel;
        //     }

        public static void CreateCardRow(NSStackView stackview, string title, params NSView[] views)
        {
            var descriptionEffectRow = new StackView(NSUserInterfaceLayoutOrientation.Horizontal);
            stackview.AddArrangedSubview(descriptionEffectRow);

            descriptionEffectRow.AddArrangedTitle(title);
            foreach (var view in views)
            {
                descriptionEffectRow.AddArrangedSubview(view);
            }

            descriptionEffectRow.AddArrangedSpace();
        }

        public static NSPopUpButton CreatePopupButton()
        {
            var enableCheckBox = new NSPopUpButton() { TranslatesAutoresizingMaskIntoConstraints = false, };
            return enableCheckBox;
        }

    }
}

