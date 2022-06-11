using System;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using PowerMode.Cocoa.Views;
using PowerMode.Utils;

namespace PowerMode
{
    class DocumentPowerSession
    {
        private ICocoaTextView view;
        public ICocoaTextView TextView => view;
        private DocumentPowerView gameView;
        public DocumentPowerView GameView => gameView;
        private ITextDocumentFactoryService textDocumentFactory;
        private IXPlatAdornmentLayer adornmentLayer;
        private ITextDocument textDocument;

        const double DeltaLeft = 12;
        const double DeltaTop = 20;

        public DocumentPowerSession(ICocoaTextView view, ITextDocumentFactoryService textDocumentFactory)
        {
            gameView = new DocumentPowerView(Session.Current.LevelManager);
            gameView.Offset = new CGPoint(-25, -30);
            gameView.Size = 50;

            Reload();

            this.view = view ?? throw new ArgumentNullException("view");
            adornmentLayer = view.GetXPlatAdornmentLayer("PowerModeAdornment");
            this.textDocumentFactory = textDocumentFactory;

            adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, gameView, null);

            Refresh();

            SuscribeEvents();
        }

        public void Unregister()
        {
            if (this.view != null)
            {
                UnsuscribeEvents();

                adornmentLayer.RemoveAdornment(gameView);

                this.view = null;
                adornmentLayer = null;
                textDocumentFactory = null;
            }
        }

        internal void SuscribeEvents()
        {
            this.view.TextBuffer.Changed += TextBuffer_Changed;
            this.view.ViewportHeightChanged += View_ViewportSizeChanged;
            this.view.ViewportWidthChanged += View_ViewportSizeChanged;
            this.view.Closed += View_Closed;
            Session.Current.LevelManager.LevelChanged += LevelManager_LevelChanged;
        }

        private void LevelManager_LevelChanged(object sender, EventArgs e)
        {
            gameView.ShowCurrentLevel();
        }

        internal void UnsuscribeEvents()
        {
            this.view.TextBuffer.Changed -= TextBuffer_Changed;
            this.view.ViewportHeightChanged -= View_ViewportSizeChanged;
            this.view.ViewportWidthChanged -= View_ViewportSizeChanged;
            this.view.Closed -= View_Closed;
            //this.view.LayoutChanged -= View_LayoutChanged;
            Session.Current.LevelManager.LevelChanged -= LevelManager_LevelChanged;
        }

        private void View_Closed(object sender, EventArgs e)
        {
            Session.Current.Unattach(this.view);
        }

        private async Task ShakeAsync(int delta)
        {
            for (var i = 0; i < delta && i < Session.Current.MaxShakeAmount; i++)
            {
                int leftAmount = Session.Current.ExplosionAmount * RandomUtils.NextSignal(),
                    topAmount = Session.Current.ExplosionAmount * RandomUtils.NextSignal();

                view.ViewportLeft += leftAmount;
                view.ViewScroller.ScrollViewportVerticallyByPixels(topAmount);
                await Task.Delay(Session.Current.ExplosionDelay);
                view.ViewportLeft -= leftAmount;
                view.ViewScroller.ScrollViewportVerticallyByPixels(-topAmount);
            }
        }

        //HACK: sometimes 
        bool firstTime = true;

        private void TextBuffer_Changed(object sender, TextContentChangedEventArgs e)
        {
            if (!Session.Current.IsEnabled)
                return;

            if (e.Changes?.Count > 0)
            {
                var delta = e.Changes.Sum(x => x.Delta);

                MonoDevelop.Core.Runtime.RunInMainThread(() =>
                {
                    var point = view.Caret;
                    gameView.Cursor = new CGPoint(
                        point.Left + DeltaLeft,
                        point.Top + DeltaTop
                        );
                    gameView.Step();

                    if (Session.Current.ShakeEnabled && gameView.TryGetSelectedPowerMode(out var powerModeItem) && powerModeItem.ShakeIntensity > 0)
                    {
                        ShakeAsync(delta).ConfigureAwait(false);
                    }

                    if (firstTime)
                    {
                        firstTime = false;
                        Refresh();
                    }
                });
            }
        }

        void Reload()
        {
            var powerMode = Session.Current.PowerMode;
            gameView.SetData(powerMode);
        }

        private void View_ViewportSizeChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        void Refresh()
        {
            var parent = gameView.Superview;
            if (parent == null)
                return;
            gameView.Frame = parent.Bounds;
        }

        //private void View_LayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        //{
        //    if (textDocument == null && textDocumentFactory.TryGetTextDocument(view.TextBuffer, out textDocument))
        //    {
        //       // adornmentLayer.RemoveAdornment(gameView);
        //    }
        //}

    }
}

