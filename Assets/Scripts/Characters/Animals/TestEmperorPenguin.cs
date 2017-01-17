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

    //TODO: blend animations, add rotation
    //anim.SetFloat("PosX", wayPoint.x);
    //anim.SetFloat("PosY", wayPoint.y);

    //testing rotation
    x += Time.deltaTime * 10;
    transform.rotation = Quaternion.Euler(x, 0, 0);

    //sets movement point
    float moveX = wayPoint.x*Time.deltaTime;
    float moveZ = wayPoint.y*Time.deltaTime;

    //TODO: replace with pathfinding
    rbody.velocity = new Vector3(moveX, 0, moveZ);
    anim.Play("Walk", -1);

    Debug.Log(wayPoint); 
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
