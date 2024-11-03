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
        private StyleSheet _styleSheet = default;

        [SerializeField]
        private VisualTreeAsset _itemLayout = default;


        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            if (_layout != null)
            {
                _layout.CloneTree(root);
            }
            if(_styleSheet != null)
            {
                root.styleSheets.Add(_styleSheet);
            }

            CacheVisualElements(root);
            SetupVisualElements();

            return root;
        }
        public void OnDisable()
        {
            var storage = (ObjectStorage)target;
            EditorUtility.SetDirty(storage);
        }


        // VisualElementフィールドを定義
        // header
        private VisualElement header;

        private VisualElement titleSection;
        private Label         titleSection__title;

        private VisualElement preferenceSection;
        private DropdownField preferenceSection__typeSelector;

        // main
        private VisualElement main;

        private VisualElement objectSection;
        private EnumField     objectSection__listSelector;
        private ListView      objectSection__list;

        // footer
        private VisualElement footer;

        private VisualElement controllerSection;

        private VisualElement objectAppenderSection;
        private DropdownField objectAppenderSection__typeSelector;
        private Button        objectAppenderSection__objectCreater;
        private Button        objectAppenderSection__existingObjectAppender;

        private VisualElement filterSection;
        private DropdownField filterSection__typeFilter;
        private EnumField     filterSection__nameFilterType;
        private TextField     filterSection__nameFilter;
        private Button        filterSection__filterCleaner;

        private VisualElement listOrganizerSection;
        private Button        listOrganizerSection__sort;
        private EnumField     listOrganizerSection__sortTypeSelector;
        private Button        listOrganizerSection__distinct;

        private VisualElement defaultInspectorSection;
        private Foldout       defaultInspectorSection__foldout;

        // フィールドをキャッシュ
        private void CacheVisualElements(VisualElement root)
        {
            T FindOrCreate<T>(VisualElement container, string name)
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


            // header
            header = FindOrCreate<VisualElement>(root, "header");

            titleSection                    = FindOrCreate<VisualElement>(header,           "title-section");
            titleSection__title             = FindOrCreate<Label>        (titleSection,     "title-section__title");

            preferenceSection               = FindOrCreate<VisualElement>(header,            "preference-section");
            preferenceSection__typeSelector = FindOrCreate<DropdownField>(preferenceSection, "preference-section__type-selector");

            // main
            main = FindOrCreate<VisualElement>(root, "main");

            objectSection                   = FindOrCreate<VisualElement>(main,          "object-section");
            objectSection__listSelector     = FindOrCreate<EnumField>    (objectSection, "object-section__list-selector");
            objectSection__list             = FindOrCreate<ListView>     (objectSection, "object-section__list");

            // footer
            footer = FindOrCreate<VisualElement>(root, "footer");

            controllerSection               = FindOrCreate<VisualElement>(footer, "controller-section");

            objectAppenderSection                           = FindOrCreate<VisualElement>(controllerSection,     "object-appender-section");
            objectAppenderSection__typeSelector             = FindOrCreate<DropdownField>(objectAppenderSection, "object-appender-section__type-selector");
            objectAppenderSection__objectCreater            = FindOrCreate<Button>       (objectAppenderSection, "object-appender-section__object-creater");
            objectAppenderSection__existingObjectAppender   = FindOrCreate<Button>       (objectAppenderSection, "object-appender-section__existing-object-appender");

            filterSection                   = FindOrCreate<VisualElement>(controllerSection, "filter-section");
            filterSection__typeFilter       = FindOrCreate<DropdownField>(filterSection,     "filter-section__type-filter");
            filterSection__nameFilterType   = FindOrCreate<EnumField>    (filterSection,     "filter-section__name-filter-type");
            filterSection__nameFilter       = FindOrCreate<TextField>    (filterSection,     "filter-section__name-filter");
            filterSection__filterCleaner    = FindOrCreate<Button>       (filterSection,     "filter-section__filter-cleaner");

            listOrganizerSection                    = FindOrCreate<VisualElement>(controllerSection,    "list-organizer-section");
            listOrganizerSection__sort              = FindOrCreate<Button>       (listOrganizerSection, "list-organizer-section__sort");
            listOrganizerSection__sortTypeSelector  = FindOrCreate<EnumField>    (listOrganizerSection, "list-organizer-section__sort-type-selector");
            listOrganizerSection__distinct          = FindOrCreate<Button>       (listOrganizerSection, "list-organizer-section__distinct");

            defaultInspectorSection          = FindOrCreate<VisualElement>(footer,                  "default-inspector-section");
            defaultInspectorSection__foldout = FindOrCreate<Foldout>      (defaultInspectorSection, "default-inspector-section__foldout");
        }
        private void SetupVisualElements()
        {
            var storage = (ObjectStorage)target;
            var view = new ObjectStorageView(storage, _itemLayout);


            // header
            // Title Section
            view.SetupTitleSection__Title(titleSection__title);

            // Preference Section
            view.SetupPreferenceSection__TypeSelector(preferenceSection__typeSelector);


            // main
            // Object Section
            view.SetupObjectSection__list(objectSection__list);

            view.AssociateObjectSection__listSelectorWithList(
                objectSection__listSelector,
                objectSection__list
            );

            view.PopulateObjectSection__listSelector(objectSection__listSelector);


            // footer
            // Controller Section
            // Object Appender Section
            view.SetupObjectAppenderSection__TypeSelector(objectAppenderSection__typeSelector);
            view.SetupObjectAppenderSection__ObjectCreater(objectAppenderSection__objectCreater);
            view.SetupObjectAppenderSection__ExistingObjectAppender(objectAppenderSection__existingObjectAppender);

            view.AssociateObjectAppenderSection__TypeSelectorWithObjectCreater(
                objectAppenderSection__typeSelector,
                objectAppenderSection__objectCreater
            );

            // Filter Section
            view.SetupFilterSection__TypeFilter(filterSection__typeFilter);
            view.SetupFilterSection__NameFilter(filterSection__nameFilter);
            view.SetupFilterSection__NameFilterType(filterSection__nameFilterType);

            view.AssociateFilterSection__FilterCleanerWithTypeFilter(
                filterSection__filterCleaner,
                filterSection__typeFilter
            );
            view.AssociateFilterSection__FilterCleanerWithNameFilter(
                filterSection__filterCleaner,
                filterSection__nameFilter
            );

            // List Organizer Section
            view.SetupListOrganizerSection__Sort(listOrganizerSection__sort);
            view.SetupListOrganizerSection__SortTypeSelector(listOrganizerSection__sortTypeSelector);
            view.SetupListOrganizerSection__Distinct(listOrganizerSection__distinct);

            // Default Inspector Section
            InspectorElement.FillDefaultInspector(
                defaultInspectorSection__foldout,
                serializedObject,
                this
            );


            // Associate Sections
            view.AssociateListOrganizerSectionWithObjectSection__ButtonWithList(
                objectAppenderSection__objectCreater,
                objectSection__list
            );
            view.AssociateListOrganizerSectionWithObjectSection__ButtonWithList(
                objectAppenderSection__existingObjectAppender,
                objectSection__list
            );
            view.AssociateListOrganizerSectionWithObjectSection__ButtonWithList(
                listOrganizerSection__sort,
                objectSection__list
            );
            view.AssociateListOrganizerSectionWithObjectSection__ButtonWithList(
                listOrganizerSection__distinct,
                objectSection__list
            );

            view.AssociateFilterSectionWithObjectSection__TypeFilterWithList(
                filterSection__typeFilter,
                objectSection__list
            );
            view.AssociateFilterSectionWithObjectSection__NameFilterWithList(
                filterSection__nameFilter,
                objectSection__list
            );
        }
    }
}
