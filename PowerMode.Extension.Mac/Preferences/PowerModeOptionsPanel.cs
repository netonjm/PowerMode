using AppKit;
using Foundation;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using static Microsoft.VisualStudio.Shell.ThreadedWaitDialogHelper;

namespace PowerMode.Extension.Preferences
{
    class TextCell : NSView
    {
        public string StringValue
        {
            get => textField.StringValue;
            set => textField.StringValue = value;
        }

        NSTextField textField;

        public TextCell()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;

            textField = NSTextField.CreateLabel("");
            textField.TranslatesAutoresizingMaskIntoConstraints = false;
            AddSubview(textField);
            ViewHelper.AttachHorizontally(this, textField);

            textField.CenterYAnchor.ConstraintEqualTo(CenterYAnchor).Active = true;
        }
    }

    class TableDelegate : NSTableViewDelegate
    {
        const string identifer = "myCellIdentifier";

        // Returns the NSView for a given column/row. NSTableView is strange as unlike NSOutlineView 
        // it does not pass in the data for the given item (obtained from the DataSource) for the NSView APIs
        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            // This pattern allows you reuse existing views when they are no-longer in use.
            // If the returned view is null, you instance up a new view
            // If a non-null view is returned, you modify it enough to reflect the new data

            TextCell view = (TextCell)tableView.MakeView(identifer, this);
            if (view == null)
            {
                view = new TextCell();
                view.Identifier = identifer;
            }

            var dataSource = (TableDataSource)tableView.DataSource;
            view.StringValue = dataSource.Data[(int)row];
            return view;
        }

        // An example of responding to user input 
        public override bool ShouldSelectRow(NSTableView tableView, nint row)
        {
            return true;
        }
    }

    // Data sources in general walk a given data source and respond to questions from AppKit to generate
    // the data used in your Delegate. However, as noted in GetViewForItem above, NSTableView
    // only requires the row count from the data source, instead of also requesting the data for that item
    // and passing that into the delegate.
    class TableDataSource : NSTableViewDataSource
    {
        public List<string> Data = new List<string>();

        public override nint GetRowCount(NSTableView tableView)
        {
            return Data.Count;
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

