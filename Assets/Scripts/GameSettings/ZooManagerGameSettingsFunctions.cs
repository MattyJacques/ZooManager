// Sifaka Game Studios (C) 2017

using UnityEngine;

namespace Assets.Scripts.GameSettings
{
    public static class ZooManagerGameSettingsFunctions
    {
        public static ZooManagerGameSettings LoadSettings(string settingsPath)
        {
            var settingsAsset = Resources.Load<TextAsset>(settingsPath);

            if (settingsAsset != null)
            {
                return JsonUtility.FromJson<ZooManagerGameSettings>(settingsAsset.text);
            }

            Debug.LogError("Failed to load GameSettings!");

            return null;
        }
    }
}
