using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Burn.Editor
{
    public class BurnWindow : EditorWindow
    {
        private static BurnWindow window;
        public Vector2 scrollPosition;

        protected void OnGUI()
        {
            if (Application.isPlaying)
            {
                return;
            }

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);

            var count = SceneManager.sceneCountInBuildSettings;
            for (var i = 0; i < count; ++i)
            {
                var path = SceneUtility.GetScenePathByBuildIndex(i);
                var pathSubstring = path.Substring(path.LastIndexOf('/') + 1);
                var buttonName = pathSubstring.Substring(0, pathSubstring.Length - 6);
                if (GUILayout.Button(buttonName))
                {
                    EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                }
            }

            GUILayout.EndScrollView();
        }

        [MenuItem("Burn/Burn Window")]
        protected static void Init()
        {
            window = (BurnWindow) GetWindow(typeof(BurnWindow));
            window.Show();
        }
    }
}