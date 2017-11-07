// Sifaka Game Studios (C) 2017

using Assets.Scripts.GameSettings;
using UnityEditor;

namespace Assets.Editor.EditorExtensions.Windows
{
    public class ZooManagerGameSettingsWindow : BaseWindow<ZooManagerGameSettings>
    {
        [MenuItem("Window/Zoo Manager Game Settings")]
        static void Init()
        {
            var window = (ZooManagerGameSettingsWindow)GetWindow(typeof(ZooManagerGameSettingsWindow));
            window.Show();
        }

        protected override void SetDataPath()
        {
            OutputDataPath = GameSettingsConstants.GameSettingsPath;
        }

        protected override void OnGUICalled()
        {
        }
    }
}
