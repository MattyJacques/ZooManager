// Title        : BuildingManager.cs
// Purpose      : Initiates templates, manages instances of pavement
// Author       : Jacob Miller
// Date         : 09/11/2016

using UnityEngine;
using System.Collections;

public class PavementBase
{

  // Template values never get set, they represent the building's permanent properties
  public PavementTemplate _template { get; set; }

  PavementBase(PavementTemplate template)
  { // Initialise the template for the building

    _template = template;

  } // BuildingBase()

} // BuildingBase
