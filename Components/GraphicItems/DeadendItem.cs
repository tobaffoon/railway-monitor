﻿using railway_monitor.Bases;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace railway_monitor.Components.GraphicItems
{
    public class DeadendItem : GraphicItem
    {
        public DeadendItem() : base() { }

        public override void Reassign_OnPortMerged(object? sender, Port oldPort)
        {
            throw new NotImplementedException();
        }

        protected override void Render(DrawingContext drawingContext)
        {
            throw new NotImplementedException();
        }
    }
}
