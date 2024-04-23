﻿using System.Windows;
using railway_monitor.MVVM.Models.Station;

namespace railway_monitor.MVVM.Models.Station.Units
{
    public class Switch : NodeUnit
    {
        private StraightRailTrack _switchableTrackOne;
        private StraightRailTrack _switchableTrackTwo;
        private bool _trackOneChosen;
        public bool TrackOneChosen
        {
            get => _trackOneChosen;
            set => SetField(ref _trackOneChosen, value);
        }

        public Switch(iPoint pos, StraightRailTrack switchableTrackOne, StraightRailTrack switchableTrackTwo) : base(pos)
        {
            _switchableTrackOne = switchableTrackOne;
            _switchableTrackTwo = switchableTrackTwo;
            _trackOneChosen = true;
        }

        public void Toggle()
        {
            TrackOneChosen = !_trackOneChosen;
        }
    }
}