// Sifaka Game Studios (C) 2017

using System.IO;
using System.Linq;
using Assets.Scripts.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.EditorExtensions.Windows
{
    public abstract class BaseWindow <TCurrentType> : EditorWindow
        where TCurrentType : new()
    {
        protected string OutputDataPath { get; set; }
        private TCurrentType TweakableInstance { get; set; }

        private void Awake()
        {
            // Recompile please
            SetDataPath();
            LoadCurrentAsset();
        }

        protected abstract void SetDataPath();

        protected void LoadCurrentAsset()
        {
            if (OutputDataPathExists())
            {
                var tweakableAssetFile = (TextAsset)Resources.Load(OutputDataPath);
                if (tweakableAssetFile != null)
                {
                    TweakableInstance =
                        JsonUtility.FromJson<TCurrentType>(tweakableAssetFile.text);
                }

                if (TweakableInstance != null)
                {
                    return;
                }
            }

            TweakableInstance = new TCurrentType();
        }

        private void OnGUI()
        {
            if (OutputDataPathExists())
            {
                CreateGuiForType();
            }
        }

        protected abstract void OnGUICalled();

        protected void CreateGuiForType()
        {
            var fieldsWithAttribute = typeof(TCurrentType).GetFields()
                .Select(pi => new { Property = pi, Attribute = pi.GetCustomAttributes(typeof(UnityReflectionAttribute), true).FirstOrDefault() as UnityReflectionAttribute })
                .Where(x => x.Attribute != null)
                .ToList();

            foreach (var pa in fieldsWithAttribute)
            {
                var value = pa.Property.GetValue(TweakableInstance);
                switch (pa.Attribute.CurrentType)
                {
                    case UnityReflectionAttributeType.BoolType:
                        value = EditorGUILayout.Toggle(pa.Property.Name, (bool)value);
                        break;
                    case UnityReflectionAttributeType.FloatType:
                        value = EditorGUILayout.FloatField(pa.Property.Name, (float)value);
                        break;
                    case UnityReflectionAttributeType.StringType:
                        value = EditorGUILayout.TextField(pa.Property.Name, (string) value);
                        break;
                    default:
                        Debug.LogError("UnityReflectionAttribute was not a valid type! Could not display" + pa.ToString());
                        break;
                }
                pa.Property.SetValue(TweakableInstance, value);
            }
        }

        protected void OnLostFocus()
        {
            if (OutputDataPathExists() && TweakableInstance != null)
            {
                File.WriteAllText("Assets/Resources/" + OutputDataPath + ".json", JsonUtility.ToJson(TweakableInstance));
            }
        }

        private bool OutputDataPathExists()
        {
            return !string.IsNullOrEmpty(OutputDataPath);
        }
    }
}
