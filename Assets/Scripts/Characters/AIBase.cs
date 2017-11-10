// Sifaka Game Studios (C) 2017

using UnityEngine;

namespace Assets.Scripts.Characters
{
  public class AIBase
  {
    public enum FeedType { Food, Water }          // Enum for feed() to tell which stat to increase

    // The animal's temporary properties change over the life of the animal
    public float Health { get; set; }    // How healthy the animal is, low is dying
    public float Hunger { get; set; }    // How hungry the animal is, low is hungry
    public float Thirst { get; set; }    // How thirsty the animal is, low is thirsty
    public float Boredom { get; set; }   // How bored the animal is, low is bored
    public float Age { get; set; }       // Animal Age, increases with time

    public GameObject Model { get; set; } // Model of the object, used to render the object

    // Behaviour object for AI
    public Behaviours.Base.BehaviourTree Behave { get; set; }

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

    public virtual void AddFun(int amount)
    { // Increase the fun meter
      Boredom += amount;
    } // AddFun()

    public virtual void Kill()
    { // Kill the character

      Health = 0;

    } // Kill()

  } // AIBase
} // namespace
    
