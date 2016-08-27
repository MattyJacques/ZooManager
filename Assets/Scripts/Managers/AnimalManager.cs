// Title        : AnimalManager.cs
// Purpose      : Initiates templates, manages instances of animals
// Author       : Matthew Jacques
// Date         : 27/08/2016

using UnityEngine;
using System.Collections;


namespace ZooManager
{

  public class AnimalManager : MonoBehaviour
  {

    // Holds all animal templates read from JSON array
    public AnimalTemplateCollection templates;

    void Start()
    { // Call to get the templates from JSON

      templates = JSONReader.ReadJSON();

    } // Start()

  } // AnimalManager

} // Namespace
