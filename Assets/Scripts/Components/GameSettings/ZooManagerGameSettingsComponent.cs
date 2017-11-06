// Sifaka Game Studios (C) 2017

using UnityEngine;

namespace Assets.Scripts.Components.GameSettings
{
    public class ZooManagerGameSettingsComponent : MonoBehaviour
    {
        public string SettingsPath = "GameSettings/ZooManagerGameSettings";
        public ZooManagerGameSettings CurrentSettings;

        protected void Awake ()
        {
            CurrentSettings =
                JsonUtility.FromJson<ZooManagerGameSettings>(((TextAsset) Resources.Load(SettingsPath)).text);
        }
    }
}
