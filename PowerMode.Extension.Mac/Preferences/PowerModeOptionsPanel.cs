using AppKit;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Dialogs;
using System;

namespace PowerMode.Extension.Preferences
{
    class PowerModeOptionsPanelViewController : NSViewController
    {
		NSButton enableCheckBox, enableShake;
		NSPopUpButton effectsPopupButton;
		//NSTextField timeoutTextField;

		public PowerModeOptionsPanelViewController()
        {
			var stackview = new StackView();
			View = stackview;

			var nameRow = new StackView(NSUserInterfaceLayoutOrientation.Horizontal);
			stackview.AddArrangedSubview(nameRow);

			enableCheckBox = CreateCheckBoxCardView(nameRow, "Enabled:", "Enable to activate POWER MODE!!!");

			var effectsRow = new StackView(NSUserInterfaceLayoutOrientation.Horizontal);
			stackview.AddArrangedSubview(effectsRow);

			effectsPopupButton = CreatePopupButtonCardView(effectsRow, "Presets:");

			CreateSubLabel(stackview, "Choose between different preset gifs to use when powermode is activated");


			var shakeRow = new StackView(NSUserInterfaceLayoutOrientation.Horizontal);
			stackview.AddArrangedSubview(shakeRow);
			enableShake = CreateCheckBoxCardView(shakeRow, "Shake:", "Enable Shake mode");
			CreateSubLabel(stackview, "Set false to disable shake when typing");

			//timeoutTextField = CreateNumericCardView(stackview, "Timeout");

			////Values
			//timeoutTextField.IntValue = Settings.TimeOut;

			enableCheckBox.State = Session.Current.IsEnabled ? NSCellStateValue.On : NSCellStateValue.Off;

			enableShake.State = Session.Current.ShakeEnabled ? NSCellStateValue.On : NSCellStateValue.Off;

			foreach (var item in Configurations.Data)
            {
				effectsPopupButton.AddItem(item.Description ?? String.Empty);
			}

			effectsPopupButton.SelectItem(Session.Current.PowerModeIndex);
		}

		NSTextField CreateNumericCardView(NSStackView effectsRow, string label)
		{
			effectsRow.AddArrangedTitle(label);
			var effectsPopupButton = CreateNumericTextField();
			effectsPopupButton.WidthAnchor.ConstraintEqualTo(350).Active = true;
			effectsRow.AddArrangedSubview(effectsPopupButton);
			effectsRow.AddArrangedSpace();
			return effectsPopupButton;
		}

		NSPopUpButton CreatePopupButtonCardView(NSStackView effectsRow, string label)
        {
			effectsRow.AddArrangedTitle(label);
			var effectsPopupButton = CreatePopupButton();
			effectsPopupButton.WidthAnchor.ConstraintEqualTo(350).Active = true;
			effectsRow.AddArrangedSubview(effectsPopupButton);
			effectsRow.AddArrangedSpace();
			return effectsPopupButton;
		}

		NSButton CreateCheckBoxCardView(NSStackView nameRow, string label, string description)
		{
			nameRow.AddArrangedTitle(label);

			var enableCheckBox = CreateCheckBox();
			enableCheckBox.Title = description;
			nameRow.AddArrangedSubview(enableCheckBox);
			nameRow.AddArrangedSpace();
			return enableCheckBox;
		}

		public void CreateSubLabel(NSStackView stackview, string label)
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

		//public override void ViewWillAppear()
  //      {
  //          base.ViewWillAppear();
  //          enableCheckBox.Activated += EnableCheckBox_Activated;
  //          effectsPopupButton.Activated += EffectsPopupButton_Activated;
		//}

  //      private void EffectsPopupButton_Activated(object sender, EventArgs e)
  //      {
			
		//}

  //      public override void ViewWillDisappear()
  //      {
		//	enableCheckBox.Activated -= EnableCheckBox_Activated;
		//	effectsPopupButton.Activated -= EffectsPopupButton_Activated;
		//	base.ViewWillDisappear();
  //      }

		NSButton CreateCheckBox()
        {
			var enableCheckBox = new NSButton() { Title = String.Empty, TranslatesAutoresizingMaskIntoConstraints = false, };
			enableCheckBox.SetButtonType(NSButtonType.Switch);
			return enableCheckBox;
		}

		NSTextField CreateNumericTextField()
		{
			var enableCheckBox = new NSTextField() { TranslatesAutoresizingMaskIntoConstraints = false, };
			return enableCheckBox;
		}

		NSPopUpButton CreatePopupButton()
		{
			var enableCheckBox = new NSPopUpButton() { TranslatesAutoresizingMaskIntoConstraints = false, };
			return enableCheckBox;
		}

        internal void ApplyChanges()
        {
			Session.Current.PowerModeIndex = (int) effectsPopupButton.IndexOfSelectedItem;
			Settings.SetInt(SettingsPropperties.PowerModeIndex, Session.Current.PowerModeIndex);

			Session.Current.IsEnabled = enableCheckBox.State == NSCellStateValue.On;
			Settings.SetBool(SettingsPropperties.IsEnabled, Session.Current.IsEnabled);

			Session.Current.ShakeEnabled = enableShake.State == NSCellStateValue.On;
			Settings.SetBool(SettingsPropperties.IsEnabled, Session.Current.ShakeEnabled);

			Session.Current.Reload();
        }

        internal bool ValidateChanges()
        {
            return true;
        }
    }

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

