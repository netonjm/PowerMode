using AppKit;
using Microsoft.VisualStudio.Shell.TableManager;
using MonoDevelop.Core;
using MonoDevelop.Ide.Composition;
using System;
using System.Linq;
using static CoreFoundation.DispatchSource;

namespace PowerMode.Extension.Preferences
{
    class PowerModeOptionsPanelViewController : NSViewController
    {
		NSButton enableCheckBox, enableShake, enableBackground, enablePowerLevel;
		NSPopUpButton effectsPopupButton;
        NSTextField durationTextField;
		NSSlider opacitySlider;

        IPowerModeSession powerSession;
		IBackgroundSession backgroundSession;

		StackView stackview;

        public PowerModeOptionsPanelViewController()
        {
            powerSession = CompositionManager.Instance.GetExportedValue<IPowerModeSession>();
            backgroundSession = CompositionManager.Instance.GetExportedValue<IBackgroundSession>();

            stackview = new StackView();
            View = stackview;

            CreatePowerModeOptions();
            var box = new NSBox() { BoxType = NSBoxType.NSBoxSeparator };
            stackview.AddArrangedSubview(box);
            ViewHelper.AttachHorizontally(stackview, box);
            box.HeightAnchor.ConstraintEqualTo(7).Active = true;
            CreateBackgroundOptions();
        }

        private void CreateBackgroundOptions()
        {
            var backgroundRow = new StackView(NSUserInterfaceLayoutOrientation.Horizontal);
            stackview.AddArrangedSubview(backgroundRow);

            enableBackground = ViewHelper.CreateCheckBoxCardView(backgroundRow, "Animated Background:", "Enable to activate Animated Background");

            var opacityRow = new StackView(NSUserInterfaceLayoutOrientation.Horizontal);
            stackview.AddArrangedSubview(opacityRow);
            opacitySlider = ViewHelper.CreateSliderCardView(opacityRow, "Background Opacity:");

            opacitySlider.MinValue = 0;
            opacitySlider.MaxValue = 1;
            opacitySlider.AltIncrementValue = 0.1f;

            durationTextField = new NSTextField();
            durationTextField.WidthAnchor.ConstraintEqualTo(100).Active = true;
            ViewHelper.CreateCardRow(stackview, "Duration (frame/s)", durationTextField, new NSView() { TranslatesAutoresizingMaskIntoConstraints = false } );

            NSScrollView scrollView = new NSScrollView() { TranslatesAutoresizingMaskIntoConstraints = false };
            stackview.AddArrangedSubview(scrollView);
            ViewHelper.AttachHorizontally(stackview, scrollView);
            scrollView.HeightAnchor.ConstraintEqualTo(200).Active = true;

            tableView = new NSTableView();

            tableView.AddColumn(new NSTableColumn() { Title  = "Animations" });

            tableDataSource = new TableDataSource();
            tableView.DataSource = tableDataSource;

            tableDelegate = new TableDelegate();
            tableView.Delegate = tableDelegate;

            scrollView.DocumentView = tableView;

            //values
            durationTextField.FloatValue = backgroundSession.Duration;

            enableBackground.State = backgroundSession.IsEnabled ? NSCellStateValue.On : NSCellStateValue.Off;

            opacitySlider.FloatValue = backgroundSession.Opacity;

            RefreshFiles();

            SelectData(backgroundSession.FileName);
        }

        void RefreshFiles()
        {
            string data = GetSelectedData();

            var files = System.IO.Directory.EnumerateFiles(Constants.BackgroundFolder, "*.gif")
                .Select(s => System.IO.Path.GetFileName(s))
                .ToArray();
            Array.Sort(files, StringComparer.InvariantCulture);
            tableDataSource.Data.Clear();
            tableDataSource.Data.AddRange(files);
            tableView.ReloadData();

            SelectData(data);
        }

        void SelectData(string file)
        {
            var index = tableDataSource.Data.IndexOf(file);
            if (index > -1)
            {
                tableView.SelectRow(index, false);
                tableView.ScrollRowToVisible(index);
            }
        }

        string GetSelectedData()
        {
            var index = (int)tableView.SelectedRow;
            string data = null;
            if (index > -1)
            {
                data = tableDataSource.Data[index];
            }
            return data;
        }

        NSTableView tableView;
        TableDataSource tableDataSource;
        TableDelegate tableDelegate;

        void CreatePowerModeOptions()
        {
            var nameRow = new StackView(NSUserInterfaceLayoutOrientation.Horizontal);
        stackview.AddArrangedSubview(nameRow);

			enableCheckBox = ViewHelper.CreateCheckBoxCardView(nameRow, "Power Mode:", "Enable to activate POWER MODE!!!");

			var effectsRow = new StackView(NSUserInterfaceLayoutOrientation.Horizontal);
        stackview.AddArrangedSubview(effectsRow);

			effectsPopupButton = ViewHelper.CreatePopupButtonCardView(effectsRow, "Presets:");

            ViewHelper.CreateSubLabel(stackview, "Choose between different preset gifs to use when powermode is activated");

			var shakeRow = new StackView(NSUserInterfaceLayoutOrientation.Horizontal);
        stackview.AddArrangedSubview(shakeRow);
			enableShake = ViewHelper.CreateCheckBoxCardView(shakeRow, "Shake:", "Enable Shake mode");
            ViewHelper.CreateSubLabel(stackview, "Set false to disable shake when typing");

            //disable specific items

            var powerLevelRow = new StackView(NSUserInterfaceLayoutOrientation.Horizontal);
            stackview.AddArrangedSubview(powerLevelRow);
            enablePowerLevel = ViewHelper.CreateCheckBoxCardView(powerLevelRow, "Power Level:", "Enable to show power level");

            //values
            enablePowerLevel.State = powerSession.IsLevelVisible ? NSCellStateValue.On : NSCellStateValue.Off;

            enableCheckBox.State = powerSession.IsEnabled ? NSCellStateValue.On : NSCellStateValue.Off;

            enableShake.State = powerSession.ShakeEnabled ? NSCellStateValue.On : NSCellStateValue.Off;

            foreach (var item in Configurations.Data)
            {
                effectsPopupButton.AddItem(item.Description ?? String.Empty);
            }

            effectsPopupButton.SelectItem(powerSession.PowerModeIndex);
        }

        internal void ApplyChanges()
        {
            powerSession.PowerModeIndex = (int) effectsPopupButton.IndexOfSelectedItem;
			Settings.SetInt(SettingsPropperties.PowerModeIndex, powerSession.PowerModeIndex);

            powerSession.IsEnabled = enableCheckBox.State == NSCellStateValue.On;
			Settings.SetBool(SettingsPropperties.IsEnabled, powerSession.IsEnabled);

            powerSession.IsLevelVisible = enablePowerLevel.State == NSCellStateValue.On;
            Settings.SetBool(SettingsPropperties.IsLevelVisible, powerSession.IsLevelVisible);

            powerSession.ShakeEnabled = enableShake.State == NSCellStateValue.On;
			Settings.SetBool(SettingsPropperties.ShakeEnabled, powerSession.ShakeEnabled);

            powerSession.RefreshGameView();

            //background

            backgroundSession.IsEnabled = enableBackground.State == NSCellStateValue.On;
            Settings.SetBool(SettingsPropperties.IsBackgroundEnabled, backgroundSession.IsEnabled);

            backgroundSession.Opacity = opacitySlider.FloatValue;
            Settings.SetFloat(SettingsPropperties.BackgroundOpacity, backgroundSession.Opacity);

            var currentFile = GetSelectedData();

            bool needsReanimate = durationTextField.FloatValue != backgroundSession.Duration ||
                currentFile != backgroundSession.FileName;

            backgroundSession.Duration = durationTextField.FloatValue;
            Settings.SetFloat(SettingsPropperties.BackgroundDuration, backgroundSession.Duration);

            try
            {
                backgroundSession.FileName = currentFile;
                Settings.SetString(SettingsPropperties.BackgroundFileName, backgroundSession.FileName);

                if (needsReanimate)
                    backgroundSession.SetAnimation(backgroundSession.FileName, backgroundSession.Duration, 1024);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex.Message);
            }
    
			backgroundSession.RefreshBackgroundView();
        }

        internal bool ValidateChanges()
        {
            return true;
        }
    }
}

