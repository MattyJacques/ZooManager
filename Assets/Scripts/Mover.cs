using UnityEngine;
using System.Collections;
using Pathfinding.RVO;
using Pathfinding;
/*
public class Mover : MonoBehaviour {


    public Transform target;
    public float maxWaypointDistance = 2;
    public float moveSpeed = 10;

    Seeker seeker;
    Path path;
    int currentWaypoint;
    RVOController characterController;

    void Start () {
        seeker = GetComponent<Seeker>();
        // Tell Seeker to generate path
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        characterController = GetComponent<RVOController>();

    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        { path = p;
            currentWaypoint = 0;
        }
        else
        {
            Debug.LogError(p.error);
        }
    }

	void FixedUpdate () {
	if (path == null)
        {
            return;
        }
    if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized * moveSpeed;
        characterController.Move(dir);
        if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < maxWaypointDistance)
        {
            currentWaypoint++;
        }
	}
}
*/


// /this is the out of box pathfinding
namespace Pathfinding
{
    /** AI controller specifically made for the spider robot.
	 * The spider robot (or mine-bot) which is got from the Unity Example Project
	 * can have this script attached to be able to pathfind around with animations working properly.\n
	 * This script should be attached to a parent GameObject however since the original bot has Z+ as up.
	 * This component requires Z+ to be forward and Y+ to be up.\n
	 *
	 * It overrides the AIPath class, see that class's documentation for more information on most variables.\n
	 * Animation is handled by this component. The Animation component refered to in #anim should have animations named "awake" and "forward".
	 * The forward animation will have it's speed modified by the velocity and scaled by #animationSpeed to adjust it to look good.
	 * The awake animation will only be sampled at the end frame and will not play.\n
	 * When the end of path is reached, if the #endOfPathEffect is not null, it will be instantiated at the current position. However a check will be
	 * done so that it won't spawn effects too close to the previous spawn-point.
	 * \shadowimage{mine-bot.png}
	 *
	 * \note This script assumes Y is up and that character movement is mostly on the XZ plane.
	 */ //remove
    [RequireComponent(typeof(Seeker))]

    public class Mover : AIPath
    {
        /** Animation component.
		 * Should hold animations "awake" and "forward"
		 */
        //public Animation anim;

        /** Minimum velocity for moving */
        public float sleepVelocity = 0.4F;
    public Assets.Scripts.Characters.AIBase aiBase;

        /** Speed relative to velocity with which to play animations */
        //public float animationSpeed = 0.2F;

        /** Effect which will be instantiated when end of path is reached.
		 * \see OnTargetReached */
        public GameObject endOfPathEffect;

    public Mover (ref Assets.Scripts.Characters.AIBase theBase)
    {
      aiBase = theBase;
    }

        public new void Start()
        {

      //animalBase = GetComponent<Assets.Scripts.Characters.Animals.AnimalBase>();
      endOfPathEffect = new GameObject();
      //Prioritize the walking animation
      //anim["forward"].layer = 10;

      //Play all animations
      //anim.Play("awake");
      //anim.Play("forward");

      //Setup awake animations properties
      //anim["awake"].wrapMode = WrapMode.Clamp;
      //anim["awake"].speed = 0;
      //anim["awake"].normalizedTime = 1F;

      //Call Start in base script (AIPath)
      base.Start();
      
        }

        /** Point for the last spawn of #endOfPathEffect */
        protected Vector3 lastTarget;

        /**
		 * Called when the end of path has been reached.
		 * An effect (#endOfPathEffect) is spawned when this function is called
		 * However, since paths are recalculated quite often, we only spawn the effect
		 * when the current position is some distance away from the previous spawn-point
		 */
        public override void OnTargetReached()
        {
            if (Vector3.Distance(tr.position, lastTarget) > 1)
            {
        //We've reached our target, so do something!
        //GameObject.Instantiate(endOfPathEffect, tr.position, tr.rotation);
        aiBase.HasArrived = true;
                lastTarget = tr.position;
        target = null;
            }
        }

        public override Vector3 GetFeetPosition()
        {
            return tr.position;
        }

        protected new void Update()
        {
            //Get velocity in world-space
            Vector3 velocity;

            if (canMove)
            {
                //Calculate desired velocity
                Vector3 dir = CalculateVelocity(GetFeetPosition());

                //Rotate towards targetDirection (filled in by CalculateVelocity)
                RotateTowards(targetDirection);

                dir.y = 0;
                if (dir.sqrMagnitude > sleepVelocity * sleepVelocity)
                {
                    //If the velocity is large enough, move
                }
                else
                {
                    //Otherwise, just stand still (this ensures gravity is applied)
                    dir = Vector3.zero;
                }

                if (rvoController != null)
                {
                    rvoController.Move(dir);
                    velocity = rvoController.velocity;
                }
                else
                if (controller != null)
                {
                    controller.SimpleMove(dir);
                    velocity = controller.velocity;
                }
                else
                {
                    Debug.LogWarning("No NavmeshController or CharacterController attached to GameObject");
                    velocity = Vector3.zero;
                }
            }
            else
            {
                velocity = Vector3.zero;
            }


            /*Animation

            //Calculate the velocity relative to this transform's orientation
            Vector3 relVelocity = tr.InverseTransformDirection(velocity);
            relVelocity.y = 0;

            if (velocity.sqrMagnitude <= sleepVelocity * sleepVelocity)
            {
                //Fade out walking animation
                anim.Blend("forward", 0, 0.2F);
            }
            else
            {
                //Fade in walking animation
                anim.Blend("forward", 1, 0.2F);

                //Modify animation speed to match velocity
                AnimationState state = anim["forward"];

                float speed = relVelocity.z;
                state.speed = speed * animationSpeed;
            } */
        }
    }
}
