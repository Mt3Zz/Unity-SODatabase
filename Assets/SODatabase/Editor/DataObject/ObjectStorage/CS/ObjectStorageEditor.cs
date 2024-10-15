using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace SODatabase.Editor
{
    using Editor = UnityEditor.Editor;
    using ObjectStorage = DataObject.ObjectStorage;


    [CustomEditor(typeof(ObjectStorage))]
    public class ObjectStorageEditor : Editor
    {
        [SerializeField]
        private VisualTreeAsset _layout = default;
        [SerializeField]
        private VisualTreeAsset _itemLayout = default;


        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            if (_layout != null)
            {
                _layout.CloneTree(root);
            }

            CacheVisualElements(root);
            SetupVisualElements();

            return root;
        }


        // VisualElementフィールドを定義
        private VisualElement preferenceSection;
        private DropdownField preferenceSection__typeSelector;

        private VisualElement objectSection;
        private ListView      objectSection__list;
        private DropdownField objectSection__typeSelector;
        private Button        objectSection__objectCreater;

        private VisualElement defaultInspectorSection;
        private Foldout       defaultInspectorSection__foldout;

        // フィールドをキャッシュ
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


            preferenceSection = FindElementOrCreate
                <VisualElement>(
                root, 
                "preference-section");
            preferenceSection__typeSelector = FindElementOrCreate
                <DropdownField>(
                preferenceSection, 
                "preference-section__type-selector");


            objectSection = FindElementOrCreate
                <VisualElement>(
                root, 
                "object-section");
            objectSection__list = FindElementOrCreate
                <ListView>(
                objectSection, 
                "object-section__list");
            objectSection__typeSelector = FindElementOrCreate
                <DropdownField>(
                objectSection, 
                "object-section__type-selector");
            objectSection__objectCreater = FindElementOrCreate
                <Button>(
                objectSection, 
                "object-section__object-creater");


            defaultInspectorSection = FindElementOrCreate
                <VisualElement>(
                root, 
                "default-inspector-section");
            defaultInspectorSection__foldout = FindElementOrCreate
                <Foldout>(
                defaultInspectorSection, 
                "default-inspector-section__foldout");
        }
        private void SetupVisualElements()
        {
            var storage = (ObjectStorage)target;

            var view = new ObjectStorageView(storage);
            var itemView = new StorageItemView(storage, _itemLayout);


            // Preference Section



            // Object Section
            //objectSection__list.makeItem = _itemLayout.CloneTree;
            //objectSection__list.bindItem = view.SetupObjectSectionListItem;
            itemView.SetupListView(objectSection__list);

            view.SetupObjectSectionTypeSelector(objectSection__typeSelector);
            view.SetupObjectSectionObjectCreater(objectSection__objectCreater);

            view.AssociateTypeSelecltorWithObjectCreater
                (objectSection__typeSelector, 
                objectSection__objectCreater);


            // Default Inspector Section
            InspectorElement.FillDefaultInspector(
                defaultInspectorSection__foldout,
                serializedObject,
                this);
        }
    }
}
