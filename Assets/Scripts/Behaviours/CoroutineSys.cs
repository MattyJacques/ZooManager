using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineSys : MonoBehaviour {

  private static CoroutineSys _instance;

  public static CoroutineSys Instance { get { if(_instance == null) { _instance = new GameObject("CoroutineSys").AddComponent<CoroutineSys>(); }; return _instance; } }

}
