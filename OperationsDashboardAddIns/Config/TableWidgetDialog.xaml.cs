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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ESRI.ArcGIS.OperationsDashboard;
using client = ESRI.ArcGIS.Client;

namespace OperationsDashboardAddIns.Config
{
  public partial class TableWidgetDialog : Window
  {
    private DataSource _dataSource = null;
    private IFeatureAction[] _configFeatureActions = 
    {
       new PanToFeatureAction(), 
       new HighlightFeatureAction() { UpdateExtent = UpdateExtentType.Pan }, 
       new ZoomToFeatureAction(), 
       new SearchNearbyFeatureAction(OperationsDashboard.Instance.DataSources.First(), 1, client.Tasks.LinearUnit.Kilometer) 
    };
    private string _caption = "New Table Widget";
    private IEnumerable<IFeatureAction> _selectedFeatureActions = null;

    #region Properties
    public DataSource DataSource
    {
      get { return _dataSource; }
      set
      {
        _dataSource = value;        
        DataSourceSelector.SelectedDataSource = _dataSource;
      }
    }

    public string Caption
    {
      get { return _caption; }
      private set
      {
        _caption = value;
        CaptionTextBox.Text = _caption;
      }
    }

    public IEnumerable<IFeatureAction> SelectedFeatureActions
    {
      get { return _selectedFeatureActions; }
      set
      {
        _selectedFeatureActions = value;
      }
    }

    public IEnumerable<IFeatureAction> AllFeatureActions
    {
      get
      {
        List<IFeatureAction> defaultFAs = new List<IFeatureAction>(_configFeatureActions);

        if (SelectedFeatureActions != null && SelectedFeatureActions.Count() > 0)
        {
          for (int i = 0; i < defaultFAs.Count; i++)
          {
            IFeatureAction definedFeatureAction = SelectedFeatureActions.FirstOrDefault(selectedFA => selectedFA.GetType() == defaultFAs[i].GetType());

            //The list of selected feature action doesn't contain such type of feature action, hence use the default one
            if (definedFeatureAction != null)
              defaultFAs[i] = definedFeatureAction;
          }
        }
        return defaultFAs;
      }
    }
    #endregion

    public TableWidgetDialog()
      : this(null, null, null)
    {
    }

    public TableWidgetDialog(DataSource dataSource, IEnumerable<IFeatureAction> selectedFeatureActions, string caption)
    {
      InitializeComponent();
      DataSource = dataSource == null ? OperationsDashboard.Instance.DataSources.First() : dataSource;
      SelectedFeatureActions = selectedFeatureActions == null ? AllFeatureActions : selectedFeatureActions;
      Caption = caption;
      FeatureActionList.FeatureActions = AllFeatureActions;
      FeatureActionList.SelectedFeatureActions = SelectedFeatureActions;
    }

    private void OnOkButtonClick(object sender, RoutedEventArgs e)
    {
      SelectedFeatureActions = FeatureActionList.SelectedFeatureActions;

      //documented issue-FeatureActionList.Selectedfeatureactions doesnt seem to hold the latest config values for a custom feature action and 
      //hence we are copying the config values from all feature actions 
      if (SelectedFeatureActions.Any(fa => fa is SearchNearbyFeatureAction))
      {
        SearchNearbyFeatureAction searchNearbyFeatureAction = SelectedFeatureActions.Where(fa => fa is SearchNearbyFeatureAction).FirstOrDefault() as SearchNearbyFeatureAction;
        SearchNearbyFeatureAction searchNearbyFeatureActionAll = FeatureActionList.FeatureActions.Where(fac => fac is SearchNearbyFeatureAction).FirstOrDefault() as SearchNearbyFeatureAction;
        searchNearbyFeatureAction.BufferDistance = searchNearbyFeatureActionAll.BufferDistance;
        searchNearbyFeatureAction.BufferUnit = searchNearbyFeatureActionAll.BufferUnit;
        searchNearbyFeatureAction.TargetDataSource = searchNearbyFeatureActionAll.TargetDataSource;
      }

      Caption = CaptionTextBox.Text;
      DialogResult = true;
    }

    private void OnSelectionChanged(object sender, EventArgs e)
    {
      DataSource = DataSourceSelector.SelectedDataSource;
    }
  }
}
