using AvalonDock.Layout;
using satellite_tracker.ViewModels;
using System.Linq;

namespace satellite_tracker
{
    internal class LayoutInitializer : ILayoutUpdateStrategy
    {
        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
        }

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
        {
        }

        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            LayoutAnchorablePane destPane = destinationContainer as LayoutAnchorablePane;
            if (destinationContainer != null && destinationContainer.FindParent<LayoutFloatingWindow>() != null)
                return false;

            anchorableToShow.CanShowOnHover = false;
            anchorableToShow.CanDockAsTabbedDocument = false;

            if (anchorableToShow.Content is SatelliteInfoViewModel)
            {
                var pane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(d => d.Name == "SatelliteInfoPane");
                if (pane != null)
                {
                    pane.Children.Add(anchorableToShow);
                    return true;
                }
            }

            if (anchorableToShow.Content is SatelliteStatsViewModel)
            {
                var pane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(d => d.Name == "SatelliteStatsPane");
                if (pane != null)
                {
                    pane.Children.Add(anchorableToShow);
                    return true;
                }
            }

            if (anchorableToShow.Content is ObserverInfoViewModel)
            {
                var pane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(d => d.Name == "ObserverInfoPane");
                if (pane != null)
                {
                    pane.Children.Add(anchorableToShow);
                    return true;
                }
            }

            return false;
        }

        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
        {
            return false;
        }
    }
}