// Copyright 2013 ESRI
// 
// All rights reserved under the copyright laws of the United States
// and applicable international laws, treaties, and conventions.
// 
// You may freely redistribute and use this sample code, with or
// without modification, provided you include the original copyright
// notice and use restrictions.
// 
// See the use restrictions http://help.arcgis.com/en/sdk/10.0/usageRestrictions.htm.
// 
using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using ESRI.ArcGIS.OperationsDashboard;

namespace OperationsDashboardAddIns
{
  [Export("ESRI.ArcGIS.OperationsDashboard.MapTool")]
  [ExportMetadata("DisplayName", "Zoom In/out")]
  [ExportMetadata("Description", "Custom zoom to extent map tool")]
  [ExportMetadata("ImagePath", "/OperationsDashboardAddIns;component/Images/ZoomBox16.png")]
  [DataContract]
  public partial class ZoomExtentMapTool : UserControl, IMapTool
  {
    // This map tool uses a custom toolbar to help implement cancelation.
    private CancelationToolbar _cancelToolbar = null;

    public ZoomExtentMapTool()
    {
      InitializeComponent();
    }

    #region IMapTool
    // IMapTool MapWidget property just saves a reference to the map that the tool is hosted in.
    public MapWidget MapWidget { get; set; }

    public void OnActivated()
    {
      // No actions required when this tool is activated.
    }

    public void OnDeactivated()
    {
      // Clean up any objects if still set.
      if (_cancelToolbar != null)
      {
        _cancelToolbar = null;
      }
    }

    public bool CanConfigure
    {
      // No configuration required for this tool.
      get { return false; }
    }

    public bool Configure(System.Windows.Window owner)
    {
      // Not implemented as CanConfigure returned false.
      throw new NotImplementedException();
    }
    #endregion

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      // When the user clicks the map tool button, begin installing zoom code.
      // Ensure the tool has a valid map to work with.
      if ((MapWidget != null) && (MapWidget.Map != null))
      {
        // Provide a way for the user to cancel the operation, by installing a temporary toolbar.
        // This also prevents other tools from being used in the meantime.
        _cancelToolbar = new CancelationToolbar(MapWidget);
        MapWidget.SetToolbar(_cancelToolbar);
      }

    }
  }
}
