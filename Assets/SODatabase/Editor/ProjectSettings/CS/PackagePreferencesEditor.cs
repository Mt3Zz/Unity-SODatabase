using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SODatabase.Editor
{
    [CustomEditor(typeof(PackagePreferences))]
    public class PackagePreferencesEditor : UnityEditor.Editor
    {
        [SerializeField]
        private VisualTreeAsset _layout = default;
        [SerializeField]
        private StyleSheet _styleSheet = default;


        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            _layout.CloneTree(root);
            root.styleSheets.Add(_styleSheet);


            CreateRootFolderPreferences(root);


            return root;
        }


        private void CreateRootFolderPreferences(VisualElement root)
        {
            var instance = PackagePreferences.instance;
            var preferences = instance.FolderPreferences;


            var rootFolderSetter = root.Q<Button>("root-folder-preferences__folder-setter");
            if(rootFolderSetter == null)
            {
                Debug.Log("root-folder-preferences__folder-setter is null");
                return;
            }
            var rootFolderInitializer = root.Q<Button>("root-folder-preferences__folder-initializer");
            if (rootFolderInitializer == null)
            {
                Debug.Log("root-folder-preferences__folder-initializer is null");
                return;
            }
            var rootFolderField = root.Q<TextField>("root-folder-preferences__current-folder");
            if (rootFolderField == null)
            {
                Debug.Log("root-folder-preferences__current-folder is null");
                return;
            }

            rootFolderSetter.clicked += () =>
            {
                preferences.CreateDirectoryRecursively(preferences.RootFolder);

                // 保存先のフォルダパスを取得
                var fullPath = EditorUtility.SaveFolderPanel(
                    "Root Folder", // 開かれるウィンドウのタイトル
                    "Assets", // 開いたとき表示されるフォルダ
                    "SODatabase" // 入力されている保存先
                    );

                // 選択されたならパスが入っている。キャンセルされたなら入っていない。
                if (!string.IsNullOrEmpty(fullPath))
                {
                    // フルパスを相対パスに変換
                    var matchedFullPath = System.Text.RegularExpressions.Regex.Match(fullPath, "Assets/.*");
                    var folderPath = matchedFullPath.Value;


                    // 保存処理
                    //Debug.Log(folderPath);
                    rootFolderField.value = folderPath;
                }
            };


            rootFolderInitializer.clicked += () =>
            {
                preferences.CreateDirectoryRecursively(preferences.InitialRootFolder);

                rootFolderField.value = preferences.InitialRootFolder;
            };
        }
    }
}
