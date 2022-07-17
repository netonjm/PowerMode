using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using MonoDevelop.Core;
using PowerMode.Cocoa.Views;
using SceneKit;

namespace PowerMode
{
    class Constants
    {
        public static string BackgroundFolder =>
     System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".cache", "PowerMode", "Animation");

        public static string DefaultBackgroundImageFile =>
          System.IO.Path.Combine(BackgroundFolder, DefaultAnimation);

        public const string DefaultAnimation = "retrowave5.gif";
    }

    [Export(typeof(IBackgroundSession))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class BackgroundSession : IBackgroundSession
    {
        public ICocoaTextView TextView { get; private set; }

        public AnimatedView BackgroundView { get; }
        private ITextDocument textDocument;
        private IXPlatAdornmentLayer powerModeBackgroundAdornment;

        float backgrounOpacity = 1;
        public float Opacity
        {
            get => backgrounOpacity;
            set
            {
                if (backgrounOpacity == value)
                    return;
                backgrounOpacity = value;
                BackgroundView.AlphaValue = value;
            }
        }

        float duration = 1000;
        public float Duration
        {
            get => duration;
            set
            {
                if (duration == value)
                    return;
                duration = value;
                BackgroundView.Duration = value;
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
                BackgroundView.Hidden = !isEnabled;
            }
        }


        string fileName = Constants.DefaultAnimation;
        public string FileName
        {
            get => fileName;
            set
            {
                if (fileName == value)
                    return;
                fileName = value;
            }
        }

        /// <summary>
        /// Checks the folder and creates the default animation if doesn't exists
        /// </summary>
        /// <returns></returns>
        bool TryCreateIfNotExists()
        {
            var backroundPath = Constants.BackgroundFolder;
            var animationPath = System.IO.Path.Combine(backroundPath, Constants.DefaultAnimation);

            bool result = false;

            if (!System.IO.Directory.Exists(backroundPath))
            {
                System.IO.Directory.CreateDirectory(Constants.BackgroundFolder);
                result = true;
            }

            if (!System.IO.File.Exists(animationPath))
            {
                //create also the default gif
                WriteResourceToFile(Constants.DefaultAnimation, animationPath);
                result = true;
            }
            return result;
        }

        public void WriteResourceToFile(string resourceName, string fileName)
        {
            try
            {
                using (var resource = this.GetType().Assembly.GetManifestResourceStream(resourceName))
                {
                    using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                    {
                        resource.CopyTo(file);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError("Cannot generate initial resource", ex);
            }
        }

        public BackgroundSession()
        {
            BackgroundView = new AnimatedView("BackgroundAnimation") {
                RepeatCount = (float)nfloat.PositiveInfinity
            };

            //initial values
            isEnabled = Settings.GetBool(SettingsPropperties.IsBackgroundEnabled);

            var duration = Settings.GetFloat(SettingsPropperties.BackgroundDuration);
            if (duration == 0)
                duration = 1000;
            Duration = duration;

            Opacity = Settings.GetFloat(SettingsPropperties.BackgroundOpacity);

            //let's read the default image
            fileName = Constants.DefaultAnimation;
            if (TryCreateIfNotExists())
            {
                Settings.SetString(SettingsPropperties.BackgroundFileName, Constants.DefaultAnimation);
                fileName = Constants.DefaultAnimation;
            }
            else
            {
                //everything exists
                fileName = Settings.GetString(SettingsPropperties.BackgroundFileName);
            };

            SetAnimation(fileName, duration, 1024);
        }

        public void SetAnimation(string fileName, float duration, float explosionSize)
        {
            //let's read the default image
            var fullFilePath = System.IO.Path.Combine(Constants.BackgroundFolder, fileName);

            var item = new GifUrlPowerModeItem()
            {
                Urls = new Foundation.NSUrl[]
                {
                    new Foundation.NSUrl(fullFilePath, false),
                },
                ExplosionDuration = duration * 1000,
                Description = "default explosion",
                ExplosionSize = explosionSize,
            };

            if (item != null)
            {
                BackgroundView.Process(item);
            }
        }

        public void Configure(ICocoaTextView view)
        {
            Unregister();

            this.TextView = view ?? throw new ArgumentNullException("view");

            powerModeBackgroundAdornment = view.GetXPlatAdornmentLayer("PowerModeBackgroundAdornment");
            powerModeBackgroundAdornment.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, BackgroundView, null);

            SuscribeEvents();
            RecalculateBackgroundViewFrame();
        }

        internal void SuscribeEvents()
        {
            if (this.TextView != null)
            {
                this.TextView.ViewportHeightChanged += View_ViewportSizeChanged;
                this.TextView.ViewportWidthChanged += View_ViewportSizeChanged;
                this.TextView.ViewportLeftChanged += View_ViewportSizeChanged;
                this.TextView.ZoomLevelChanged += View_ViewportSizeChanged;
                this.TextView.Closed += View_Closed;
                this.TextView.LayoutChanged += View_ViewportSizeChanged;
            }
        }

        internal void UnsuscribeEvents()
        {
            if (this.TextView != null)
            {
                this.TextView.ViewportHeightChanged -= View_ViewportSizeChanged;
                this.TextView.ViewportWidthChanged -= View_ViewportSizeChanged;
                this.TextView.ViewportLeftChanged -= View_ViewportSizeChanged;
                this.TextView.ZoomLevelChanged -= View_ViewportSizeChanged;
                this.TextView.LayoutChanged -= View_ViewportSizeChanged;

                this.TextView.Closed -= View_Closed;
            }
        }

        void Unregister()
        {
            if (this.TextView != null)
            {
                UnsuscribeEvents();

                powerModeBackgroundAdornment?.RemoveAdornment(BackgroundView);

                this.TextView = null;
                powerModeBackgroundAdornment = null;
            }
        }

        void View_ViewportSizeChanged(object sender, EventArgs e) => RecalculateBackgroundViewFrame();

        void RecalculateBackgroundViewFrame()
        {
            var parent = BackgroundView?.Superview;
            if (parent == null)
                return;
            BackgroundView.Frame = parent.Bounds;
        }

        void View_Closed(object sender, EventArgs e) => Unregister();

        public void RefreshBackgroundView()
        {
            //nothing
        }
    }
}

