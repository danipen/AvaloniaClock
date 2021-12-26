using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;

namespace AvaloniaClock
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            ClockPanel clockPanel = new ClockPanel();
            this.Content = clockPanel;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16);
            timer.Tick += (s, e) => 
            {
                clockPanel.InvalidateVisual();
            };
            timer.Start();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

    class ClockPanel : Panel
    {
        public override void Render(DrawingContext context)
        {
            base.Render(context);

            int margin = 10;

            var hour = DateTime.Now.Hour + DateTime.Now.Minute / 60.0;
            var minute = DateTime.Now.Minute + DateTime.Now.Second / 60.0;
            var second = DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0;

            context.FillRectangle(
                Brushes.Black,
                new Rect(0, 0, Bounds.Width, Bounds.Height));

            Point center = new Point(Bounds.Width / 2, Bounds.Height / 2);

            for (int i = 0; i < 12; i++)
            {
                Point tick = new Point(
                    center.X + (Bounds.Width / 2 - margin) * Math.Sin(i * 2 * Math.PI / 12),
                    center.Y - (Bounds.Height / 2 - margin) * Math.Cos(i * 2 * Math.PI / 12));

                context.DrawRectangle(
                    Brushes.White,
                    new Pen(Brushes.White, 1),
                    new Rect(tick.X - 2, tick.Y - 2, 4, 4));
            }

            Point hourHand = new Point(
                center.X + (Bounds.Width / 3 - margin) * Math.Sin(hour * 2 * Math.PI / 12),
                center.Y - (Bounds.Height / 3 - margin) * Math.Cos(hour * 2 * Math.PI / 12));
            Point minuteHand = new Point(
                center.X + (Bounds.Width / 2- margin) * Math.Sin(minute * 2 * Math.PI / 60),
                center.Y - (Bounds.Height / 2- margin) * Math.Cos(minute * 2 * Math.PI / 60));
            Point secondHand = new Point(
                center.X + (Bounds.Width / 2- margin) * Math.Sin(second * 2 * Math.PI / 60),
                center.Y - (Bounds.Height / 2- margin) * Math.Cos(second * 2 * Math.PI / 60));

            context.DrawLine(
                new Pen(Brushes.White, 5),
                center,
                hourHand);

            context.DrawLine(
                new Pen(Brushes.White, 3),
                center,
                minuteHand);

            context.DrawLine(
                new Pen(Brushes.Red, 1),
                center,
                secondHand);
        }
    }
    public static class ExtensionMethods
    {
        public static double Map (this double value, double fromSource, double toSource, double fromTarget, double toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }
    }
}