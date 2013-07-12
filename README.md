OpsDashboard-Table-SearchNearby-Zoom
================================

Presented in UC2013, The Operations Dashboard for ArcGIS addins include an attribute table widget, a search nearby feature action and a zoom in/out tool
This addin is also available to you in ArcGIS Online at http://nitro.maps.arcgis.com/home/item.html?id=162fc6b1899448db800d8a738ca02cc6

## Features

* Attribute table widget 
* Search nearby feature action
* Zoom in/out map tool

## Instructions

### Running this add-in

1. Ensure all requirements below are met.
2. Open and compile project in Visual Studio 2012.
3. Update Project Debug properties to correctly locate add-in build path in /addinpath command line argument.
4. Run project in Debug mode.
5. When Operations Dashboard application starts, edit an operation view that contains a map widget.
6. To use the table widget, choose it from Add Widget dialog box, and click OK. 
6. Select a map widget for the table widget to consume and click OK.
7. The widget should be added to the operation view. 
8. To use the search nearby feature action. Open the configuration dialog of the table widget (or other widgets).
9. Check to add the feature action to the view, open its configuration dialog to set the data source and the buffer distance to search.
10. From the widget with the feature action, right click on a feature, click Search Nearby Features. 
11. Features that are within the buffer distance will be highlighted. 
12. To use the zoom in/out map tool, click the configure button on the default map toolbar, add the zoom in/out map tool
13. Run the map tool and follow the on screen description to zoom in/out.
11. To run the addin in Release environment, build the project, and upload the .opdashboardaddin file from the build output directory to ArcGIS Online, and then download using Manage Add-Ins in Operations Dashboard, then follow the steps above from step 5.

### General Help
[New to Github? Get started here.](http://htmlpreview.github.com/?https://github.com/Esri/esri.github.com/blob/master/help/esri-getting-to-know-github.html)

## Requirements
### ArcGIS for Organizations

* Operations Dashboard for ArcGIS requires an [ArcGIS for Organizations account] (http://www.esri.com/software/arcgis/arcgisonline/evaluate).

### ArcGIS Runtime SDK for WPF

* Requires an installation of the Esri ArcGIS Runtime SDK for WPF version 10.1.1 or later [Esri website](http://resources.arcgis.com/en/communities/runtime-wpf/)
* Also requires an installation of Microsoft Visual Studio 2012.

## Resources

Learn more about Esri's [Operations Dashboard for ArcGIS](http://www.esri.com/software/arcgis/arcgisonline/features/operations-dashboard).

## Issues

Find a bug or want to request a new feature?  Please let us know by submitting an issue.

## Contributing

Anyone and everyone is welcome to contribute.

## Licensing

Copyright 2013 Esri

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0
         
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
                                 
A copy of the license is available in the repository's
[license.txt](https://github.com/esri/table-searchnearby-zoom/blob/master/license.txt) file.
                                                                  
[](Esri Tags: User Conference)
[](Esri Tags: 2013)
[](Esri Language: C-Sharp)
                                                                                                               
                                                                                                                                                            
                                                                                                                                                            
