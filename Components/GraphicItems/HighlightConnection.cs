﻿using railway_monitor.Bases;
using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Components.GraphicItems {
    public class HighlightConnection : GraphicItem {
        public static readonly double ConnectRadius = 15;
        private static readonly Brush _highlightNormalBrush = new SolidColorBrush(Color.FromArgb(100, 51, 153, 255));
        private static readonly Pen _highlightNormalPen = new Pen(_highlightNormalBrush, 0);
        private static readonly Brush _highlightErrorBrush = new SolidColorBrush(Color.FromArgb(100, 230, 20, 20));
        private static readonly Pen _highlightErrorPen = new Pen(_highlightErrorBrush, 0);

        private Point _pos = new Point(0, 0);
        public Point Pos {
            get {
                return _pos;
            }
            set {
                _pos.X = value.X;
                _pos.Y = value.Y;
                Render();
            }
        }

        public HighlightConnection() : base() {
            Visibility = Visibility.Collapsed;
        }

        public bool ConnectionErrorOccured = false;
        public override void Reassign_OnPortMerged(object? sender, Port oldPort) {
            throw new NotImplementedException();
        }

        protected override void Render(DrawingContext dc) {
            if (ConnectionErrorOccured) {
                dc.DrawEllipse(_highlightErrorBrush, _highlightErrorPen, Pos, ConnectRadius, ConnectRadius);
            }
            else {
                dc.DrawEllipse(_highlightNormalBrush, _highlightNormalPen, Pos, ConnectRadius, ConnectRadius);
            }
        }
    }
}
