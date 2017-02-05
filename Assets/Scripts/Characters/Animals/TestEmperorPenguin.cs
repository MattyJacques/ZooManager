using Assets.Scripts.Characters.Animals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEmperorPenguin : MonoBehaviour{
  private Animator anim;
  private Vector2 wayPoint;
  private Rigidbody rbody;

  float x;

  // Use this for initialization
	void Start () 
  {
      anim = GetComponent<Animator>();
      rbody = GetComponent<Rigidbody>();
      
      //TODO, move to Update() later...
      InvokeRepeating("Wander", 2, 20);
	}

  void Wander() 
  { 
    //random point to wander to, 47 is the radius
    wayPoint = Random.insideUnitCircle *47;

    //TODO: blend animations
    //anim.SetFloat("PosX", wayPoint.x);
    //anim.SetFloat("PosY", wayPoint.y);

    //sets movement point
    float moveX = wayPoint.x*Time.deltaTime;
    float moveZ = wayPoint.y*Time.deltaTime;

    //TODO: replace with pathfinding
    rbody.velocity = new Vector3(moveX, 0, moveZ).normalized;

    //changed rotation to -velocity because GameObject was moonwalking
    Quaternion DesiredRotation = Quaternion.LookRotation(-rbody.velocity);
    transform.rotation = DesiredRotation;

    anim.Play("Walk", -1);

    Debug.Log("Waypoint=" + wayPoint + "Velocity=" + rbody.velocity.magnitude + "Rotation=" + rbody.rotation); 
  }
	
  // Update is called once per frame
  void Update()
  { // Process the needs of the base then process the behaviour for AI
    
    /*
    if(****ISHUNGRY******){
     //TODO: move to food
      anim.Play("PenguinEat",-1);
    }
     */
	}
}
