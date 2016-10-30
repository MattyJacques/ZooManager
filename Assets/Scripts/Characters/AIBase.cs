using UnityEngine;
using System.Collections;


namespace Assets.Scripts.Characters
{
  public class AIBase
  {
    public enum FeedType { Food, Water }     // Enum for feed() to tell which stat to increase

    // This reference can be used to move the character around without needing to make AIBase a MonoBehaviour
    public Transform UnityTransform { get; set; }

    // The animal's temporary properties change over the life of the animal
    public float Health { get; set; }
    public float Hunger { get; set; }
    public float Thirst { get; set; }
    public float Age { get; set; }
    public float Boredom { get; set; }
    public GameObject Target { get; set; }
    public GameObject Path { get; set; }      // Change to path object
    public BehaviourTree.Base.Behaviour Behave { get; set; }

    public virtual void Feed(FeedType type, int amount)
    { // Increase the hunger or thirst meter, depending on type
      if (type == FeedType.Food)
      {
        Hunger += amount;
      }
      else
      {
        Thirst += amount;
      }
    } // Feed()

    public virtual void CheckNeeds()
    { // Perform the behaviour for this base

      Behave.Behave(this);

    } // CheckNeeds()

    public virtual void Kill()
    { // Kill the character

      Health = 0;

    } // Kill()

  } // AIBase
} // namespace
