// Title        : BuildingManager.cs
// Purpose      : Initiates templates, manages instances of animals
// Author       : Matthew Jacques
// Date         : 03/09/2016

using UnityEngine;
using System.Collections;

public class BuildingBase
{

  // Template values never get set, they represent the building's permanent properties
  public BuildingTemplate _template { get; set; }

  BuildingBase(BuildingTemplate template)
  { // Initialise the template for the building

    _template = template;

  } // BuildingBase()

} // BuildingBase
