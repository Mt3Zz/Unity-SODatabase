using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


namespace SODatabase.Editor
{
    using ObjectStorage = DataObject.ObjectStorage;


    public class SODatabaseWindow : EditorWindow
    {
        // メニューの名前
        private const string MENU_TITLE = "SODatabase Editor";
        // ウィンドウの名前
        private const string WINDOW_TITLE = "Database Editor";


        [SerializeField]
        private VisualTreeAsset _layout = default;
        [SerializeField]
        private StyleSheet _style = default;

        [SerializeField]
        private VisualTreeAsset _asideItemLayout = default;


        [MenuItem("Window/" + MENU_TITLE)]
        public static void ShowWindow()
        {
            var window = GetWindow<SODatabaseWindow>();
            window.titleContent = new GUIContent(WINDOW_TITLE);
        }


        private void CreateGUI()
        {
            var root = rootVisualElement;
            if (_layout != null)
            {
                _layout.CloneTree(root);
            }
            if(_style != null)
            {
                root.styleSheets.Add(_style);
            }


            CacheVisualElements(root);
            SetupVisualElements();
        }
        private void OnFocus()
        {
            SetupAsideMain(aside__main);
        }


        // VisualElement をキャッシュ
        private VisualElement aside;
        private ScrollView    aside__main;
        private VisualElement aside__footer;
        private Button        footer__preferencesLink;

        private VisualElement main;

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


            aside = FindElementOrCreate
                <VisualElement>(
                root,
                "aside"
                );
            aside__main = FindElementOrCreate
                <ScrollView>(
                aside,
                "aside__main");
            aside__footer = FindElementOrCreate
                <VisualElement>(
                aside,
                "aside__footer");
            footer__preferencesLink = FindElementOrCreate
                <Button>(
                aside__footer,
                "footer__preferences-link");


            main = FindElementOrCreate
                <VisualElement>(
                root,
                "main");
        }
        private void SetupVisualElements()
        {
            // Setup aside
            SetupAsideMain(aside__main);
            SetupFooterPreferencesLink(footer__preferencesLink);
        }


        private void SetupAsideMain(VisualElement aside)
        {
            var instance = PackagePreferences.instance;
            var storages = instance.DistinctStorages;
            //Debug.Log(string.Join("\n", storages));

            var distinctTypes = storages
                .Select(storage => storage.Preferences.ObjectType)
                .Distinct()
                .ToList();
            //Debug.Log(string.Join("\n", distinctTypes));

            if (distinctTypes.Count == 0) return;


            aside.Clear();
            foreach (var type in distinctTypes)
            {
                var container = _asideItemLayout.CloneTree();
                container.styleSheets.Add(_style);
                var list = container.Q<ListView>("aside-item__storage-list");

                if (list == null) return;


                var source = storages
                        .Where(storage => storage.Preferences.ObjectType == type)
                        .ToList();
                //Debug.Log(string.Join("\n", sourceList));

                list.itemsSource = source;
                list.headerTitle = type.Name == "BaseObject" ? "(None)" : type.Name;
                list.makeItem = () =>
                {
                    var label = new Label();
                    label.AddToClassList("aside__storage-list--label-style");
                    return label;
                };
                list.bindItem = (element, index) =>
                {
                    var label = element as Label;
                    label.text = source[index].name;

                    element.RegisterCallback<ClickEvent>(evt =>
                    {
                        RefreshMain(source[index]);
                    });
                };

                aside.Add(container);
            }
        }
        private void SetupFooterPreferencesLink(Button button)
        {
            button.clicked += () =>
            {
                SettingsService.OpenProjectSettings("Project/SO Database");
            };
        }


        private void RefreshMain(ScriptableObject obj)
        {
            if (obj == null) return;

            main.Clear();
            if (false) // objがエディターを持っている場合の分岐を書きたい
            {
                //var container = UnityEditor.Editor.CreateEditor(obj);
            }
            else
            {
                var container = new InspectorElement(obj);
                main.Add(container);
            }
        }
    }
}
