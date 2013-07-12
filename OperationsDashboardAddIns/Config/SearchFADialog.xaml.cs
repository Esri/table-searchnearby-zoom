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

using System.ComponentModel;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Controls;
using ESRI.ArcGIS.Client.Tasks;
using System;
using ESRI.ArcGIS.OperationsDashboard;

namespace OperationsDashboardAddIns.Config
{
  /// <summary>
  /// Configuration dialog for the search nearby tool
  /// </summary>
  public partial class SearchFADialog : Window, IDataErrorInfo
  {
    private bool dataNoError = true;
    private int _distance = 1;
    private List<LinearUnit> _units;
    private LinearUnit _selectedUnit;

    #region Properties
    public DataSource TargetDataSource { get; private set; }

    //buffer radius
    public int Distance
    {
      get { return _distance; }
      set
      { _distance = value; }
    }

    //list of radius units
    public List<LinearUnit> Units
    {
      get { return _units; }
      set { _units = value; }
    }

    //selected buffer radius unit
    public LinearUnit SelectedUnit
    {
      get { return _selectedUnit; }
      set { _selectedUnit = value; }
    }
    #endregion

    public SearchFADialog()
    {
      InitializeComponent();

      base.DataContext = this;
    }

    public SearchFADialog(DataSource targetDataSource, int BufferDistance, LinearUnit BufferUnit)
      : this()
    {
      //Set the default selected data source
      TargetDataSource = targetDataSource == null ? OperationsDashboard.Instance.DataSources.FirstOrDefault(d => d.IsSelectable) : targetDataSource;
      DSSelector.SelectedDataSource = TargetDataSource;

      //Set buffer distance
      if (BufferDistance > 0)
        Distance = BufferDistance;

      //Set the available units 
      List<LinearUnit> units = new List<LinearUnit>() { LinearUnit.Kilometer, LinearUnit.Meter, LinearUnit.SurveyMile, LinearUnit.SurveyYard };
      Units = units;

      //Set the unit of the buffer radius
      if (BufferUnit == 0)
        SelectedUnit = Units[0];
      else
        SelectedUnit = BufferUnit;
    }

    //Set the selected data source to be the target data source 
    private void DSSelector_SelectionChanged(object sender, EventArgs e)
    {
      TargetDataSource = DSSelector.SelectedDataSource;
    }

    #region Validate distance text box input
    public string Error
    {
      get { return ""; }
    }

    public string this[string columnName]
    {
      get
      {
        string result = null;
        double pDistance;
        if (columnName == "Distance")
        {
          if (!double.TryParse(Distance.ToString(), out pDistance) || Distance <= 0)
            result = "Distance is not > 0";
        }
        return result;
      }
    }

    /// <summary>
    /// Handle when data in the distance text box has error 
    /// </summary>
    private void txtDistance_DataError(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
    {
      if (e.Action == ValidationErrorEventAction.Added)
        dataNoError = false;
      else
        dataNoError = true;
    }
    #endregion

    #region OK button command
    /// <summary>
    /// Check if the OK button's command can be executed
    /// </summary>
    private void OK_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = dataNoError;
    }

    /// <summary>
    /// Handle when OK button's command has been executed
    /// </summary>
    private void OK_HasExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
      DialogResult = true;
    }
    #endregion

  }
}
