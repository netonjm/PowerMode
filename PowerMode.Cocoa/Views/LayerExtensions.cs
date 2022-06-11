using AppKit;
using CoreAnimation;
using CoreGraphics;

namespace PowerMode.Cocoa.Views
{
    public static class LayerExtensions
    {
        public static void AddShadow(this NSView nSView, NSColor color, float radius = 4)
        {
            nSView.Shadow = new NSShadow();
            nSView.Shadow.ShadowColor = color;
            nSView.Shadow.ShadowBlurRadius = radius;
            nSView.Shadow.ShadowOffset = new CGSize(0, 0);
        }

        public static CoreAnimation.CAKeyFrameAnimation ShakeAnimation(this NSView layer, float vigourOfShake = 0.055f, int numberOfShakes = 3, float duration = 0.65f)
        {
            CGPoint pos = layer.Layer.Position;
            CoreAnimation.CAKeyFrameAnimation shakeAnimation = new CoreAnimation.CAKeyFrameAnimation();
            shakeAnimation.KeyPath = "position";
            CGPath shakePath = new CGPath();
            shakePath.MoveToPoint(pos.X, pos.Y);
            int index;
            for (index = 0; index < numberOfShakes; index++)
            {
                shakePath.AddLineToPoint(new CGPoint(pos.X - layer.Frame.Size.Width * vigourOfShake, pos.Y));
                shakePath.AddLineToPoint(new CGPoint(pos.X + layer.Frame.Size.Width * vigourOfShake, pos.Y));
            }
            shakePath.CloseSubpath();

            shakePath.AddLineToPoint(pos.X, pos.Y);
            shakePath.CloseSubpath();

            shakeAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            shakeAnimation.Duration = duration;
            shakeAnimation.Path = shakePath;
            layer.Layer.AddAnimation(shakeAnimation, null);
            return shakeAnimation;
        }

        public static void FadeInAnimation(this NSView view, float duration = AnimationStartDuration, Action resultHandler = null)
        {
            NSAnimationContext.RunAnimation((ctx) =>
            {
                ctx.Duration = AnimationEndDuration;
                ctx.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseIn);
                if (view.Animator is NSView vieAnim)
                    vieAnim.AlphaValue = 1;

            }, resultHandler);
        }

        const float AnimationStartDuration = 0.2f;
        const float AnimationEndDuration = AnimationStartDuration;

        public static void FadeOutAnimation(this NSView view, float duration = AnimationEndDuration, Action resultHandler = null)
        {
            NSAnimationContext.RunAnimation((ctx) =>
            {
                ctx.Duration = AnimationEndDuration;
                ctx.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseIn);
                if (view.Animator is NSView vieAnim)
                    vieAnim.AlphaValue = 0;
               
            }, resultHandler);
        }

        //TODO: we should move this to a shared place
        public static CGPath ToCGPath(this NSBezierPath path)
        {
            var numElements = path.ElementCount;
            if (numElements == 0)
            {
                return null;
            }

            CGPath result = new CGPath();
            bool didClosePath = true;

            for (int i = 0; i < numElements; i++)
            {
                CGPoint[] points;
                var element = path.ElementAt(i, out points);
                if (element == NSBezierPathElement.MoveTo)
                {
                    result.MoveToPoint(points[0].X, points[0].Y);
                }
                else if (element == NSBezierPathElement.LineTo)
                {
                    result.AddLineToPoint(points[0].X, points[0].Y);
                    didClosePath = false;

                }
                else if (element == NSBezierPathElement.CurveTo)
                {
                    result.AddCurveToPoint(points[0].X, points[0].Y,
                                            points[1].X, points[1].Y,
                                            points[2].X, points[2].Y);
                    didClosePath = false;
                }
                else if (element == NSBezierPathElement.ClosePath)
                {
                    result.CloseSubpath();
                }
            }

            // Be sure the path is closed or Quartz may not do valid hit detection.
            if (!didClosePath)
            {
                result.CloseSubpath();
            }
            return result;
        }
    }
}

