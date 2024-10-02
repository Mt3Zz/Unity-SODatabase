using UnityEditor;
using UnityEditor.UIElements;
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
            if (_layout != null)
            {
                _layout.CloneTree(root);
            }
            if (_styleSheet != null)
            {
                root.styleSheets.Add(_styleSheet);
            }

            CacheVisualElements(root);
            SetupVisualElements();

            return root;
        }


        // VisualElement をキャッシュ
        private VisualElement folderPathSection;
        private TextField     folderPathSection__pathField;
        private Button        folderPathSection__pathSetter;
        private Button        folderPathSection__pathInitializer;

        private VisualElement storagesSection;
        private ListView      storagesSection__list;
        private Button        storagesSection__storageCreater;

        private void CacheVisualElements(VisualElement root)
        {
            T FindElementOrCreate<T>(VisualElement container, string name)
                where T : VisualElement, new()
            {
                T element = container.Q<T>(name);
                if (element == null)
                {
                    var msg = ""
                        + $"{name} is null.\n"
                        + $"A new instance of {typeof(T)} has been created.\n";
                    Debug.Log(msg);
                    element = new T();
                }
                return element;
            }


            folderPathSection = FindElementOrCreate
                <VisualElement>(
                root,
                "folder-path-section");
            folderPathSection__pathField = FindElementOrCreate
                <TextField>(
                folderPathSection,
                "folder-path-section__text-field");
            folderPathSection__pathSetter = FindElementOrCreate
                <Button>(
                folderPathSection,
                "folder-path-section__path-setter");
            folderPathSection__pathInitializer = FindElementOrCreate
                <Button>(
                folderPathSection,
                "folder-path-section__path-initializer");


            storagesSection = FindElementOrCreate
                <VisualElement>(
                root,
                "managed-storage-section");
            storagesSection__list = FindElementOrCreate
                <ListView>(
                storagesSection,
                "managed-storage-section__storage-list");
            storagesSection__list = FindElementOrCreate
                <ListView>(
                storagesSection,
                "managed-storage-section__storage-list");
            storagesSection__storageCreater = FindElementOrCreate
                <Button>(
                storagesSection,
                "managed-storage-section__storage-creater");
        }
        private void SetupVisualElements()
        {
            // Setup folder path section
            AssociatePathSetterWithPathField
                (folderPathSection__pathSetter,
                folderPathSection__pathField);
            AssociatePathInitializerWithPathField
                (folderPathSection__pathInitializer,
                folderPathSection__pathField);


            // Setup storages section
            SetupStorageList(storagesSection__list);
            SetupStorageCreater(storagesSection__storageCreater);
        }


        private void AssociatePathSetterWithPathField
            (Button pathSetter, TextField pathField)
        {
            var instance = PackagePreferences.instance;
            var preferences = instance.FolderPreferences;

            pathSetter.clicked += () =>
            {
                preferences.CreateDirectoryRecursively(preferences.RootFolder);
                var fullPath = EditorUtility.SaveFolderPanel(
                    "Root Folder",
                    "Assets",
                    "SODatabase"
                );

                if (!string.IsNullOrEmpty(fullPath))
                {
                    var matchedFullPath = System.Text.RegularExpressions.Regex.Match(fullPath, "Assets/.*");
                    var folderPath = matchedFullPath.Value;
                    pathField.value = folderPath;
                }
            };
        }
        private void AssociatePathInitializerWithPathField
            (Button pathInitializer, TextField pathField)
        {
            var instance = PackagePreferences.instance;
            var preferences = instance.FolderPreferences;


            pathInitializer.clicked += () =>
            {
                preferences.CreateDirectoryRecursively(preferences.InitialRootFolder);
                pathField.value = preferences.InitialRootFolder;
            };
        }


        private void SetupStorageList(ListView listView)
        {
            var prop = serializedObject.FindProperty("_storages");
            if (prop != null)
            {
                listView.BindProperty(prop);
            }
            else
            {
                Debug.Log($"property : _storages is null");
            }
        }
        private void SetupStorageCreater(Button storageCreater)
        {
            var instance = PackagePreferences.instance;
            var folderPreferences = instance.FolderPreferences;
            var folderPath = folderPreferences.StorageFolder;

            storageCreater.clicked += () =>
            {
                folderPreferences.CreateDirectoryRecursively(folderPath);


                // 保存先のファイルパスを取得
                var path = EditorUtility.SaveFilePanelInProject(
                    "Save New Storage",
                    "ObjectStorage",
                    "asset",
                    "",
                    folderPath // 開いたとき表示されるフォルダ
                    );

                // 選択されたならパスが入っている。キャンセルされたなら入っていない。
                if (!string.IsNullOrEmpty(path))
                {
                    // 保存処理
                    var storage = CreateInstance<DataObject.ObjectStorage>();
                    AssetDatabase.CreateAsset(storage, path);

                    instance.DistinctStorages.Add(storage);
                }
            };
        }
    }
}
