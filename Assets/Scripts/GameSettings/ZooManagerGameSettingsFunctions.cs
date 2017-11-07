// Sifaka Game Studios (C) 2017

using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.GameSettings
{
    public static class ZooManagerGameSettingsFunctions
    {
        public static Optional<ZooManagerGameSettings> LoadSettings(string settingsPath)
        {
            var settingsAsset = Resources.Load<TextAsset>(settingsPath);

            if (settingsAsset != null)
            {
                return new Optional<ZooManagerGameSettings>(JsonUtility.FromJson<ZooManagerGameSettings>(settingsAsset.text));
            }

            Debug.LogError("Failed to load GameSettings!");

            return new Optional<ZooManagerGameSettings>();
        }
    }
}
