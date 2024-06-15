using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;
using railway_monitor.Utils;
using System.Diagnostics;

namespace railway_monitor.Tools.Actions {
    public static class RightClickToolActions {
        public static void ToggleExternalTrackType(RailwayCanvasViewModel canvas) {
            ExternalTrackItem? externalTrackItem = canvas.LatestGraphicItem as ExternalTrackItem;
            if (externalTrackItem == null) {
                return;
            }

            switch (externalTrackItem.Type) {
                case ExternalTrackItem.ExternalTrackType.IN:
                    externalTrackItem.Type = ExternalTrackItem.ExternalTrackType.OUT;
                    break;
                case ExternalTrackItem.ExternalTrackType.OUT:
                    externalTrackItem.Type = ExternalTrackItem.ExternalTrackType.IN;
                    break;
            }
        }
        public static void ScrollPlatformType(RailwayCanvasViewModel canvas) {
            StraightRailTrackItem? srtItem = canvas.LatestGraphicItem as StraightRailTrackItem;
            if (srtItem == null) {
                return;
            }

            switch (srtItem.PlatformType) {
                case StraightRailTrackItem.RailPlatformType.NONE:
                    srtItem.PlatformType = StraightRailTrackItem.RailPlatformType.CARGO;
                    break;
                case StraightRailTrackItem.RailPlatformType.CARGO:
                    srtItem.PlatformType = StraightRailTrackItem.RailPlatformType.PASSENGER;
                    break;
                case StraightRailTrackItem.RailPlatformType.PASSENGER:
                    srtItem.PlatformType = StraightRailTrackItem.RailPlatformType.NONE;
                    break;
            }
        }
        public static void ScrollMiniPlatformType(RailwayCanvasViewModel canvas) {
            StraightRailTrackItem? connectionSrt = canvas.ConnectionPlatformTrack;
            PlatformItem? platformItem = canvas.LatestGraphicItem as PlatformItem;
            if (platformItem == null) return;

            switch (platformItem.PlatformType) {
                case PlatformItem.MiniPlatformType.PASSENGER:
                    platformItem.PlatformType = PlatformItem.MiniPlatformType.CARGO;
                    break;
                case PlatformItem.MiniPlatformType.CARGO:
                    platformItem.PlatformType = PlatformItem.MiniPlatformType.PASSENGER;
                    break;
            }

            if (connectionSrt == null || ConnectConditions.RailHasPlatform(connectionSrt)) return;
            // Change underlying platform type if there is one
            switch (platformItem.PlatformType) {
                case PlatformItem.MiniPlatformType.PASSENGER:
                    connectionSrt.PlatformType = StraightRailTrackItem.RailPlatformType.PASSENGER_HOVER;
                    break;
                case PlatformItem.MiniPlatformType.CARGO:
                    connectionSrt.PlatformType = StraightRailTrackItem.RailPlatformType.CARGO_HOVER;
                    break;
            }
        }
    }
}
