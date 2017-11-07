// Sifaka Game Studios (C) 2017

using Assets.Scripts.GameSettings;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.GameSettings
{
    [TestFixture]
    public class ZooManagerGameSettingsFunctionsTestFixture
    {
        [Test]
        public void LoadSettings_InvalidPath_LogsErrorAndReturnsNull()
        {
            LogAssert.Expect(LogType.Error, "Failed to load GameSettings!");

            Assert.IsNull(ZooManagerGameSettingsFunctions.LoadSettings("Invalid/Path/For/Sure"));
        }

        [Test]
        public void LoadSettings_GameSettingsPath_ValidSettingsLoaded()
        {
            Assert.IsNotNull(ZooManagerGameSettingsFunctions.LoadSettings(GameSettingsConstants.GameSettingsPath));
        }
    }
}
