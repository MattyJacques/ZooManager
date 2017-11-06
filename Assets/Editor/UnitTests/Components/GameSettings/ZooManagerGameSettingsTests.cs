// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.GameSettings;
using Assets.Scripts.Tests.Components.GameSettings;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.GameSettings
{
    [TestFixture]
    public class ZooManagerGameSettingsTestFixture {

        [Test]
        public void Awake_LoadsExpectedFile ()
        {
            var component = new GameObject().AddComponent<TestZooManagerGameSettingsComponent>();
            component.TestAwake();

            var expectedSecondsPerDay = JsonUtility.FromJson<ZooManagerGameSettings>(((TextAsset) Resources.Load(component.SettingsPath)).text).SecondsPerDay;
            Assert.AreEqual(expectedSecondsPerDay, component.CurrentSettings.SecondsPerDay);
        }
    }
}
