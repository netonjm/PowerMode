using ObjCRuntime;
using PowerMode.Cocoa.Views;

namespace PowerMode.Example.Cocoa;

public partial class ViewController : NSViewController {

    LevelManager counter;

    DocumentPowerView gameLayer;
	ViewFrameChanged changed;

    NSPopUpButton combo;
    NSTextView textField;
    NSTextField offsetX, offsetY, sizeTextField;

    protected ViewController (NativeHandle handle) : base (handle)
	{
	}

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        changed = new ViewFrameChanged();
        View = changed;

        counter = new LevelManager();

        // Do any additional setup after loading the view.
        gameLayer = new DocumentPowerView(counter);

        offsetX = new NSTextField()
        {
            Frame = new CGRect(150, 250, 40, 23)
        };
        View.AddSubview(offsetX);

        offsetY = new NSTextField()
        {
            Frame = new CGRect(190, 250, 40, 23)
        };
        View.AddSubview(offsetY);

        offsetY = new NSTextField()
        {
            Frame = new CGRect(190, 250, 40, 23)
        };
        View.AddSubview(offsetY);

        sizeTextField = new NSTextField()
        {
            Frame = new CGRect(150, 280, 40, 23)
        };
        View.AddSubview(sizeTextField);

        //text area
        textField = new NSTextView()
        {
            Frame = new CGRect(300, 300, 500, 23)
        };
        View.AddSubview(textField);

        combo = new NSPopUpButton()
        {
            Frame = new CGRect(100, 100, 500, 23)
        };

         View.AddSubview(combo);

        foreach (var item in Configurations.Data)
        {
            combo.AddItem(item.Description);
        }

        View.AddSubview(gameLayer);
    }

    private void SizeTextField_Activated(object? sender, EventArgs e)
    {
        var item = GetSelectedItem();
        item.ExplosionSize = sizeTextField.FloatValue;
        gameLayer.SetData(item);
    }

    public override void ViewWillAppear()
    {
        base.ViewWillAppear();

        changed.FrameChanged += Changed_FrameChanged;

        offsetX.Activated += OffsetX_Activated;
        offsetY.Activated += OffsetY_Activated;
        sizeTextField.Activated += SizeTextField_Activated;
        combo.Activated += Combo_Activated;

        textField.TextDidChange += Button_Activated;

        counter.LevelChanged += Counter_LevelChanged;

        combo.SelectItem(0);
        Combo_Activated(combo, EventArgs.Empty);
    }

    private void Counter_LevelChanged(object? sender, EventArgs e)
    {
        gameLayer.ShowCurrentLevel();
    }

    PowerModeItem GetSelectedItem()
    {
        var data = Configurations.Data[(int)combo.IndexOfSelectedItem];
        return data;
    }

    private void OffsetX_Activated(object? sender, EventArgs e)
    {
        var item = GetSelectedItem();
        item.Offset = new CGPoint((nfloat)offsetX.FloatValue, item.Offset.Y);
        gameLayer.SetData(item);
    }

    private void OffsetY_Activated(object? sender, EventArgs e)
    {
        var item = GetSelectedItem();
        item.Offset = new CGPoint(item.Offset.X, (nfloat)offsetY.FloatValue);
        gameLayer.SetData(item);
    }

    private void Combo_Activated(object? sender, EventArgs e)
    {
        var cmb = (NSPopUpButton)sender;

        var index = (int)cmb.IndexOfSelectedItem;
        var data = Configurations.Data[index];
        offsetX.FloatValue = (float)data.Offset.X;
        offsetY.FloatValue = (float)data.Offset.Y;
        sizeTextField.FloatValue = (float)data.ExplosionSize;
        gameLayer.SetData(data);
    }

    private void Changed_FrameChanged(object? sender, EventArgs e)
    {
        gameLayer.Frame = View.Frame;
    }

    private void Button_Activated(object? sender, EventArgs e)
    {
        var cursorLocation = new NSRange(textField.SelectedRange.Location, 0);

        var cursorCoordinates = textField.LayoutManager.GetBoundingRect
            (cursorLocation, textField.TextContainer).Location;
        gameLayer.Cursor = new CGPoint(
            textField.Frame.X + cursorCoordinates.X,
          gameLayer.Frame.Height - textField.Frame.Y// + cursorCoordinates.Y
            );

        //var cursorCoordinates = textField.LayoutManager.GetGlyphRangeForBoundingRect( BoundingRectForGlyphRange(cursorLocation, textField.TextContainer).Location;
        //gameLayer.Cursor = textField.sele
        gameLayer.Step();
    }

    public override NSObject RepresentedObject {
		get => base.RepresentedObject;
		set {
			base.RepresentedObject = value;

			// Update the view, if already loaded.
		}
	}
}

public class ViewFrameChanged : NSView
{
	public ViewFrameChanged()
	{
	}

	public override void SetFrameSize(CGSize newSize)
	{
		base.SetFrameSize(newSize);
		FrameChanged?.Invoke(this, EventArgs.Empty);
	}

	public event EventHandler FrameChanged;
}
