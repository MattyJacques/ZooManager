using Assets.Scripts.Characters.Animals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEmperorPenguin : MonoBehaviour{
  private Animator anim;
  private Vector2 wayPoint;
  private Rigidbody rbody;
  private int radius=47;
  private float minWait=0;
  private float maxWait=5;

  // Use this for initialization
	void Start () 
  {
    anim = GetComponent<Animator>();
    rbody = GetComponent<Rigidbody>();

    StartCoroutine(ChooseDestination());
	}

  IEnumerator ChooseDestination()
  {
    while(true)
    {
      wayPoint = Random.insideUnitCircle * radius;
      float moveX = wayPoint.x * Time.deltaTime;
      float moveZ = wayPoint.y * Time.deltaTime;

      Wander(moveX, moveZ);
      yield return new WaitForSeconds(Random.Range(minWait, maxWait));
    }
  }

  void Wander(float x, float z) 
  {    
    //TODO: blend animations
    //anim.SetFloat("PosX", x);
    //anim.SetFloat("PosY", z);

    rbody.velocity = new Vector3(x, 0, z).normalized;
    //TODO: replace with above with below but range would be size of enclosure
    //rbody.velocity = new Vector3(Random.Range(-x, x), 0, Random.Range(-z, z).normalized;

    Quaternion targetRotation = Quaternion.LookRotation(-rbody.velocity);
    transform.rotation = targetRotation;

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
