//Copyright 2013 Esri
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.​
// 
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ESRI.ArcGIS.OperationsDashboard;
using client = ESRI.ArcGIS.Client;

namespace OperationsDashboardAddIns
{
  // Provides a custom toolbar to allow cancelation and prevent other tool use until the zoom operation is complete.
  public partial class CancelationToolbar : UserControl, IMapToolbar
  {
    // Reference to the map widget the toolbar is installed to.
    private MapWidget _mapWidget = null;

    // A Client Draw object to allow the user to draw onto the Map.
    private client.Draw _drawObject = null;

    public CancelationToolbar(MapWidget mapWidget)
    {
      InitializeComponent();

      // Store a reference to the MapWidget that the toolbar has been installed to.
      _mapWidget = mapWidget;
    }

    private bool isZoomout = false;
    void Map_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
        isZoomout = true;
    }

    public void OnActivated()
    {
      // Set up the DrawObject with a draw mode and symbol ready for use when a user chooses it.
      if ((_mapWidget != null) && (_mapWidget.Map != null))
      {
        _drawObject = new client.Draw(_mapWidget.Map)
        {
          DrawMode = client.DrawMode.Rectangle,
          FillSymbol = new client.Symbols.FillSymbol()
          {
            Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0)),
            BorderBrush = System.Windows.Media.Brushes.DarkGray,
            BorderThickness = 1
          },
        };
      }

      // Install a draw handler on the Map to handle the next action of the user - dragging
      // a rectangle on the map.
      _drawObject.IsEnabled = true;
      _drawObject.DrawComplete += DrawComplete;
      _mapWidget.Map.KeyDown += Map_KeyDown;
    }

    public void OnDeactivated()
    {
      // Ensure the DrawObject is cleared up.
      if (_drawObject != null)
      {
        _drawObject.IsEnabled = false;
        _drawObject.DrawComplete -= DrawComplete;
         _mapWidget.Map.KeyDown -= Map_KeyDown;
      }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      // If the user cancels the toolbar, clear up the draw object and uninstall this
      // toolbar.
      if (_drawObject != null)
      {
        _drawObject.IsEnabled = false;
        _drawObject.DrawComplete -= DrawComplete;
         _mapWidget.Map.KeyDown -= Map_KeyDown;
      }

      _mapWidget.SetToolbar(null);
    }

    void DrawComplete(object sender, client.DrawEventArgs e)
    {
      // Deactivate the draw object for now.
      if (_drawObject != null)
      {
        _drawObject.IsEnabled = false;
        _drawObject.DrawComplete -= DrawComplete;
         _mapWidget.Map.KeyDown -= Map_KeyDown;
      }

      // Remove the cancelation toolbar 
      _mapWidget.SetToolbar(null);

      // Get the tracked shape passed in as an argument.
      client.Geometry.Envelope env = e.Geometry as client.Geometry.Envelope;
      if (env != null)
      {
        _mapWidget.Map.Extent = env;

        if (isZoomout == false)
        {
          // If zoom in, set the Map Extent to be the extent of the drawn geometry.
          _mapWidget.Map.Extent = env;
        }
        else
        {
          //Zoom out based on the shorter dimension of the drawm rectangle
          double shorterRectangleDimension = env.Height > env.Width ? env.Width : env.Height;
          double selectedMapExtentDimension = shorterRectangleDimension == env.Width ? _mapWidget.Map.Extent.Width : _mapWidget.Map.Extent.Height;

          _mapWidget.Map.Zoom(shorterRectangleDimension / selectedMapExtentDimension);
        }
      }
    }
  }
}
