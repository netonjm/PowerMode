using AppKit;
using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace PowerMode.Cocoa.Views
{
    public class DocumentPowerView : NSControl
    {
        public PowerModeItem SelectedPowerMode => powerMode;
        PowerModeItem powerMode;

        NSTextField bonusTextField, levelTextField;
        NSTextField numberTextField;
        ProgressBar progressBar;
        AnimatedView cursorImageView;

        nfloat TopMarginSeparation = 10;
        nfloat RightMarginSeparation = 0;
        nfloat ItemMarginSeparation = 20;

        bool isPowermodeActive = true;

        float initialPowermodeCombo = 0;
        float comboCounterSize = 1;
        float frameCount = 0;

        const float defaultFont = 31;
        const float defaultBarSize = 100;
        const float sizeModifier = 5;

        const int TextFadeoutTimeout = 1000;
        const int DefaultLevelWidth = 200;
        const int ItemSeparation = 0;

        const int DefaultFontSize = 20;

        float expectedProgressBarWidth = defaultBarSize;

        public override bool IsFlipped => true;

        public float Size { get; set; }

        public CGPoint Cursor { get; set; }
        public CGPoint Offset { get; set; }

        public bool Shake { get; set; }

        public bool TimerVisible {
            get => !numberTextField.Hidden;
            set => numberTextField.Hidden = !value;
        }

        Level currentLevel;

        public DocumentPowerView(Level level)
        {
            this.currentLevel = level;

            WantsLayer = true;

            bonusTextField = NSTextField.CreateLabel("Level Up!!");
            bonusTextField.Alignment = NSTextAlignment.Center;
            bonusTextField.Font = NSFont.BoldSystemFontOfSize(DefaultFontSize);
            bonusTextField.TextColor = NSColor.SystemRed;
            AddSubview(bonusTextField);
            bonusTextField.AddShadow(NSColor.Red);

            levelTextField = NSTextField.CreateLabel("Level:");
            levelTextField.Alignment = NSTextAlignment.Right;
            levelTextField.Font = NSFont.BoldSystemFontOfSize(DefaultFontSize);
            levelTextField.TextColor = NSColor.White;
            AddSubview(levelTextField);
            levelTextField.AddShadow(NSColor.Red);

            numberTextField = NSTextField.CreateLabel("1x");
            numberTextField.Alignment = NSTextAlignment.Right;
            numberTextField.TextColor = NSColor.White;
            AddSubview(numberTextField);
            numberTextField.AddShadow(NSColor.Red);

            progressBar = new ProgressBar() {
                Frame = new CGRect(100, 100, 100, ProgressBar.DefaultHeight),
                Progress = 0
            };
            AddSubview(progressBar);
            progressBar.ShadowColor = NSColor.Red;

            cursorImageView = new AnimatedView();
            AddSubview(cursorImageView);

            RefreshCurrentLevel();
            Reposition();
            Refresh();
            ClearAll();
        }

        public void SetData(PowerModeItem powerMode)
        {
            this.powerMode = powerMode;

            Offset = powerMode.Offset;
            //Offset = new CGPoint(-25, -30);
            Size = powerMode.ExplosionSize * sizeModifier;
            cursorImageView.Duration = powerMode.ExplosionDuration / 1000f;

            cursorImageView.Process(powerMode);

            Reposition();
            Refresh();
        }

        public bool TryGetSelectedPowerMode(out PowerModeItem pwd)
        {
            pwd = powerMode;
            return pwd != null;
        }

        void Reposition()
        {
            var frame = Frame;

            var progressBarWidth = expectedProgressBarWidth;
            var xPosition =  frame.Width - progressBarWidth - RightMarginSeparation;
            var yPosition = TopMarginSeparation;

            //progressbar
            var progressBarFrame = progressBar.Frame;
            progressBarFrame.X = xPosition;
            progressBarFrame.Y = yPosition;
            progressBarFrame.Width = progressBarWidth;
            progressBar.Frame = progressBarFrame;

            yPosition += progressBarFrame.Height + ItemSeparation;

            //number
            numberTextField.Frame = new CGRect(0, yPosition, frame.Width - RightMarginSeparation, numberTextField.Frame.Height);

            //level
            var levelFrame = levelTextField.Frame;
            xPosition = frame.Width - levelFrame.Width - RightMarginSeparation;
            yPosition = frame.Height - levelFrame.Height - TopMarginSeparation;

            levelFrame.X = xPosition;
            levelFrame.Y = yPosition;

            levelTextField.Frame = levelFrame;

            if (!bonusTextField.Hidden)
            {
                yPosition -= bonusTextField.Frame.Height;

                //leveldescription
                bonusTextField.Frame = new CGRect(xPosition, yPosition, levelFrame.Width, bonusTextField.Frame.Height);
            }
            //cursor
            cursorImageView.Frame = new CGRect(Cursor.X + Offset.X, Cursor.Y + Offset.Y, Size, Size);
        }

        void RefreshCurrentLevel() => levelTextField.StringValue = String.Format("Level: {0}", currentLevel.GetCurrentDescription());

        public void ShowCurrentLevel()
        {
            RefreshCurrentLevel();

            if (Shake)
                levelTextField.ShakeAnimation();

            bonusTextField.StringValue = "Level Up!";
            bonusTextField.AlphaValue = 0;
            bonusTextField.Hidden = false;

            Refresh();
            bonusTextField.FadeInAnimation();
            Task.Delay(TextFadeoutTimeout).ContinueWith(s =>
            {
                AppKit.NSApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    bonusTextField.FadeOutAnimation(resultHandler: () => bonusTextField.Hidden = true);
                });
            });
        }

        NSTimer timer;

        public void Step()
        {
            if (powerMode == null)
                return;

            currentLevel.Step();
            Refresh();
            StartCounter();
        }

        void StartCounter()
        {
            numberTextField.Hidden = progressBar.Hidden = false;

            //init animation
            cursorImageView.Process(powerMode);

            if (Shake && powerMode.ShakeIntensity == 0)
            {
                numberTextField.ShakeAnimation(numberOfShakes: powerMode.ShakeIntensity);
            }

            //timer
            if (timer != null)
            {
                timer.Invalidate();
                //timer.Dispose();
            }

            //restore progress
            progressBar.Progress = 100;
            timer = NSTimer.CreateRepeatingScheduledTimer(0.1, s =>
            {
                //progressBar.Progress = cointer / 100;
                //s.Invalidate();
                if (progressBar.Progress <= 0)
                {
                    s.Invalidate();
                    ClearAll();
                }
                else
                {
                    progressBar.Progress -= 2;
                }
            });
        }

        public override bool Hidden
        {
            get =>base.Hidden;
            set
            {
                levelTextField.Hidden =
                base.Hidden = value;
            }
        }

        public void ClearAll()
        {
            bonusTextField.Hidden = numberTextField.Hidden = progressBar.Hidden = true;
            currentLevel.Reset();
            RefreshCurrentLevel();
            Refresh();
        }

        float GetComputedSize(float styleCount, float baseTextSize)
        {
            return (float)(this.isPowermodeActive ? ((styleCount * baseTextSize) / 100 * Math.Pow(0.5, frameCount * 0.2) + baseTextSize) : baseTextSize);
        }

        void Refresh()
        {
            numberTextField.StringValue = string.Format("{0}×", currentLevel.Count);

            var powerModeCombo = this.isPowermodeActive ? currentLevel.Count - this.initialPowermodeCombo : 0;

            var styleCount = Math.Min(powerModeCombo, 20);

            var textSize = GetComputedSize(styleCount, defaultFont);
            numberTextField.Font = NSFont.BoldSystemFontOfSize(textSize);

            bonusTextField.SizeToFit();
            numberTextField.SizeToFit();
            levelTextField.SizeToFit();

            expectedProgressBarWidth = GetComputedSize(styleCount, defaultBarSize);

            float hue = (100 - (this.isPowermodeActive ? powerModeCombo : 0) * 1.2f) / 100;
            //Console.WriteLine(String.Format("hue: {0} powerModeCombo:{1}", hue, powerModeCombo));
            //number.TextColor = NSColor.FromHsb(hue, 1, 0.45f);
            var color = NSColor.FromHsb(hue, 1, 0.45f);
            bonusTextField.Shadow.ShadowColor = numberTextField.Shadow.ShadowColor = color;
            //AddShadow(number, NSColor.FromHsb(hue, 1, 0.45f), 4);
            //AddShadow(number, NSColor.Red, 4);
            progressBar.ShadowColor = color;

            Reposition();
        }

        public override NSView HitTest(CGPoint aPoint)
        {
            return null;
        }

        public override void SetFrameSize(CGSize newSize)
        {
            base.SetFrameSize(newSize);
            Reposition();
        }
    }
}

