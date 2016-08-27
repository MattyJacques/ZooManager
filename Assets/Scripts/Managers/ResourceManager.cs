using UnityEngine;
using System.Collections;


namespace ZooManager
{
  public class ResourceManager : MonoBehaviour
  {

    public AnimalTemplateCollection templates;

    void Start()
    {
      //JSONReader JSONHandler;
      templates = JSONReader.ReadJSON();
    }
  }
}
