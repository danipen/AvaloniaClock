using System;
using Avalonia;
using Avalonia.Controls;
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
            timer.Interval = TimeSpan.FromSeconds(1);
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

            var hour = DateTime.Now.Hour;
            var minute = DateTime.Now.Minute;
            var second = DateTime.Now.Second;

            context.FillRectangle(
                Brushes.Black,
                new Rect(0, 0, Bounds.Width, Bounds.Height));

            Point center = new Point(Bounds.Width / 2, Bounds.Height / 2);

            for (int i =0; i < 12; i++)
            {
                Point tick = new Point(
                    center.X + (Bounds.Width / 2 - margin) * Math.Sin(i * Math.PI / 6),
                    center.Y - (Bounds.Height / 2 - margin) * Math.Cos(i * Math.PI / 6));

                context.DrawRectangle(
                    Brushes.White,
                    new Pen(Brushes.White, 1),
                    new Rect(tick.X - 2, tick.Y - 2, 4, 4));
            }

            Point hourHand = new Point(
                center.X + (Bounds.Width / 3 - margin) * Math.Sin(hour * Math.PI / 6),
                center.Y - (Bounds.Height / 3 - margin) * Math.Cos(hour * Math.PI / 6));
            Point minuteHand = new Point(
                center.X + (Bounds.Width / 2- margin) * Math.Sin(minute * Math.PI / 30),
                center.Y - (Bounds.Height / 2- margin) * Math.Cos(minute * Math.PI / 30));
            Point secondHand = new Point(
                center.X + (Bounds.Width / 2- margin) * Math.Sin(second * Math.PI / 30),
                center.Y - (Bounds.Height / 2- margin) * Math.Cos(second * Math.PI / 30));

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

            /*context.DrawText(
                Brushes.White,
                new Point(0, 0),
                new FormattedText(
                    $"{hour}:{minute}:{second}",
                    new Typeface("Arial"),
                    36,
                    TextAlignment.Left,
                    TextWrapping.NoWrap,
                    Size.Infinity));*/
        }
    }
}