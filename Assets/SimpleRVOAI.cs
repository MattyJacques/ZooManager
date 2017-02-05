using UnityEngine;
using System.Collections;
using Pathfinding.RVO;

public class SimpleRVOAI : MonoBehaviour
{

    RVOController controller;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<RVOController>();
    }

    // Update is called once per frame
    void Update()
    {
        controller.Move(transform.forward * 10);
    }
}