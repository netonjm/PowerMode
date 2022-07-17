using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.Win32;
using PowerMode.Cocoa.Views;
using PowerMode.Utils;
using static Microsoft.VisualStudio.Shell.ThreadedWaitDialogHelper;

namespace PowerMode
{
    [Export(typeof(IPowerModeSession))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class PowerModeSession : IPowerModeSession
    {
        bool shakeEnabled = true;
        public bool ShakeEnabled
        {
            get => shakeEnabled;
            set
            {
                if (shakeEnabled == value)
                    return;
                shakeEnabled = value;
                DocumentPowerView.Shake = shakeEnabled;
            }
        }

        bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (isEnabled == value)
                    return;
                isEnabled = value;

                DocumentPowerView.Hidden = !isEnabled;
                UnsuscribeEvents();
                if (value)
                {
                    SuscribeEvents();
                }
            }
        }

        public bool IsLevelVisible
        {
            get => DocumentPowerView.LevelVisible;
            set
            {
                DocumentPowerView.LevelVisible = value;
            }
        }

        public int ExplosionAmount { get; set; } = 2;
        public int ExplosionDelay { get; set; } = 50;
        public int MaxShakeAmount { get; set; } = 5;

        int powerModeIndex = 0;
        public int PowerModeIndex
        {
            get => powerModeIndex;
            set
            {
                powerModeIndex = value;
            }
        }

        public PowerModeItem PowerMode => Configurations.Data[PowerModeIndex];

        const double DeltaLeft = 12;
        const double DeltaTop = 20;

        private ITextDocument textDocument;
        private IXPlatAdornmentLayer powerModeAdornment;

        public Level Level { get; }
        public DocumentPowerView DocumentPowerView { get; }
      
        public ICocoaTextView TextView { get; private set; }

        public PowerModeSession()
        {
            this.Level = new Level();
            DocumentPowerView = new DocumentPowerView(Level)
            {
                Offset = new CGPoint(-25, -30),
                Size = 50
            };

            //initial values
            IsEnabled = Settings.GetBool(SettingsPropperties.IsEnabled);
            IsLevelVisible = Settings.GetBool(SettingsPropperties.IsLevelVisible);
            shakeEnabled = Settings.GetBool(SettingsPropperties.ShakeEnabled);
            powerModeIndex = Settings.GetInt(SettingsPropperties.PowerModeIndex);
        }

        public void Configure(ICocoaTextView view)
        {
            Unregister();

            this.TextView = view ?? throw new ArgumentNullException("view");

            powerModeAdornment = view.GetXPlatAdornmentLayer("PowerModeAdornment");
            powerModeAdornment.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, DocumentPowerView, null);

            SuscribeEvents();
            RecalculateGameViewFrame();
        }

        void Unregister()
        {
            if (this.TextView != null)
            {
                UnsuscribeEvents();

                powerModeAdornment?.RemoveAdornment(DocumentPowerView);
               
                this.TextView = null;
                powerModeAdornment = null;
            }
        }

        internal void SuscribeEvents()
        {
            if (this.TextView != null)
            {
                this.TextView.TextBuffer.Changed += TextBuffer_Changed;
                this.TextView.ViewportHeightChanged += View_ViewportSizeChanged;
                this.TextView.ViewportWidthChanged += View_ViewportSizeChanged;
                this.TextView.ViewportLeftChanged += View_ViewportSizeChanged;
                this.TextView.ZoomLevelChanged += View_ViewportSizeChanged;
                this.TextView.Closed += View_Closed;
                this.TextView.LayoutChanged += View_ViewportSizeChanged;
            }
         
            this.Level.Changed += Level_Changed;
        }

        internal void UnsuscribeEvents()
        {
            if (this.TextView != null)
            {
                this.TextView.TextBuffer.Changed -= TextBuffer_Changed;
                this.TextView.ViewportHeightChanged -= View_ViewportSizeChanged;
                this.TextView.ViewportWidthChanged -= View_ViewportSizeChanged;
                this.TextView.ViewportLeftChanged -= View_ViewportSizeChanged;
                this.TextView.ZoomLevelChanged -= View_ViewportSizeChanged;
                this.TextView.LayoutChanged -= View_ViewportSizeChanged;

                this.TextView.Closed -= View_Closed;
            }
          
            this.Level.Changed -= Level_Changed;
        }

        async Task ShakeAsync(int delta)
        {
            for (var i = 0; i < delta && i < MaxShakeAmount; i++)
            {
                int leftAmount = ExplosionAmount * RandomUtils.NextSignal(),
                    topAmount = ExplosionAmount * RandomUtils.NextSignal();

                TextView.ViewportLeft += leftAmount;
                TextView.ViewScroller.ScrollViewportVerticallyByPixels(topAmount);
                await Task.Delay(ExplosionDelay);
                TextView.ViewportLeft -= leftAmount;
                TextView.ViewScroller.ScrollViewportVerticallyByPixels(-topAmount);
            }
        }

        public void RefreshGameView()
        {
            DocumentPowerView.SetData(PowerMode);
        }

        void RecalculateGameViewFrame()
        {
            var parent = DocumentPowerView?.Superview;
            if (parent == null)
                return;
            DocumentPowerView.Frame = parent.Bounds;
        }

        void TextBuffer_Changed(object sender, TextContentChangedEventArgs e)
        {
            if (!IsEnabled)
                return;

            if (e.Changes?.Count > 0)
            {
                var delta = e.Changes.Sum(x => x.Delta);

                MonoDevelop.Core.Runtime.RunInMainThread((System.Action)(() =>
                {
                    RecalculateGameViewFrame();

                    var point = TextView.Caret;

                    this.DocumentPowerView.Cursor = new CGPoint(
                        point.Left + DeltaLeft,
                        point.Top + DeltaTop - TextView.ViewportTop
                        );
                    this.DocumentPowerView.Step();

                    if (ShakeEnabled)
                    {
                        ShakeAsync(delta).ConfigureAwait(false);
                    }
                }));
            }
        }

        void View_ViewportSizeChanged(object sender, EventArgs e)
        {
            RecalculateGameViewFrame();
        }

        void View_Closed(object sender, EventArgs e) => Unregister();

        void Level_Changed(object sender, EventArgs e)
            => DocumentPowerView.ShowCurrentLevel();
    }
}

