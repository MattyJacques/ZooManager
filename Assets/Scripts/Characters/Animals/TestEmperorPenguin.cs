// Title        : TestEmperorPenguin.cs
// Purpose      : Basic Animation Controller Script that makes animal objects stand and walk around
// Author       : Chii
// Date         : 02/06/2017

using Assets.Scripts.Characters.Animals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEmperorPenguin : MonoBehaviour
{
  private Animator anim;  //for accessing the GameObject's animator
  private Rigidbody rbody;  //for accessing the GameObject's rigidbidy
  private int radius = 47;  //radius of the circle the animals can wander...this will be removed and replaced
  private float minWalk = 2;  //minimum amount of time taken walking
  private float maxWalk = 5;  //maximum amount of time taken walking
  private float minWait = 3;  //minimum amount of time taken idling
  private float maxWait = 5;  //maximum amount of time taken idling
  private float walkSpeed, rotationSpeed; //walking speed and rotation speed

  // Use this for initialization
  void Start()
  {
    anim = GetComponent<Animator>();
    rbody = GetComponent<Rigidbody>();
    walkSpeed = anim.GetFloat("walkSpeed");
    rotationSpeed = anim.GetFloat("rotationSpeed");
    WalkorIdle();
  }//Start()

  void WalkorIdle()
  {//50% chance to idle when destination reached
    float randomFloat = Random.Range(0f, 1f);
    if (randomFloat < 0.5f)
    {
      StartCoroutine(Idle());
    }
    else
    {
      StartCoroutine(Wander());
    }
  }//WalkorIdle()

  IEnumerator Wander()
  {//picks a random waypoint and has the animal walk towards it
    float moveX = 10;
    float moveZ = 10;
    //TODO: replace below with range of enclosure
    Vector3 wayPoint = new Vector3(Random.Range(-moveX, moveX), 0, Random.Range(-moveZ, moveZ));

    rbody.velocity = wayPoint.normalized * walkSpeed;
    yield return new WaitForSeconds(Random.Range(minWalk, maxWalk));
    WalkorIdle();
  }//Wander()

  IEnumerator Idle()
  {//this makes the animal stand 
    rbody.velocity = Vector3.zero;
    yield return new WaitForSeconds(Random.Range(minWait, maxWait));
    WalkorIdle();
  }//Idle()

  // Update is called once per frame
  void Update()
  { // Process the needs of the base then process the behaviour for AI
    Quaternion targetRotation = Quaternion.LookRotation(-rbody.velocity);
    if (rbody.velocity.magnitude != 0)
    {
      transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
    anim.SetFloat("speed", rbody.velocity.magnitude);
  }//Update()
}
