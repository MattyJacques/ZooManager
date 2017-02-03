//Title: SmokeRemover.cs
//Author: Aimmmmmmmmm
//Date: 1/28/2017
//Purpose: To remove smoke after it's done.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeRemover : MonoBehaviour
{
  private float _time = 0;

  void Update ()
  {
    _time = _time + Time.deltaTime;
    if (_time > 5) { Destroy (this.gameObject); }
  }
}
