// Sifaka Game Studios (C) 2017



using Assets.Scripts.Core.Attributes;

namespace Assets.Scripts.GameSettings
{
    public static class GameSettingsConstants
    {
        public const string GameSettingsPath = "GameSettings/ZooManagerGameSettings";
    }

    public class ZooManagerGameSettings
    {
      [UnityReflection(UnityReflectionAttributeType.FloatType)]
      public float SecondsPerDay = 100f;

      // CAMERA controls
      public static UnityEngine.KeyCode ZOOM_CAMERA       = UnityEngine.KeyCode.Z;

      // BUILDING PLACEMENT controls
      public static UnityEngine.KeyCode TOGGLE_GRID       = UnityEngine.KeyCode.Slash;
      public static UnityEngine.KeyCode TOGGLE_ROTATE     = UnityEngine.KeyCode.Comma;

      // ENCLOSURE BUILDING controls
      public static UnityEngine.KeyCode LOCK_TO_AXIS      = UnityEngine.KeyCode.LeftShift;
      public static UnityEngine.KeyCode FINISH_ENCLOSURE  = UnityEngine.KeyCode.Mouse1;

  }
}
