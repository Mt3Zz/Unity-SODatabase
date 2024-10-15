using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace SODatabase.Editor
{
    using ObjectStorage = DataObject.ObjectStorage;

    internal class StorageItemView
    {
        private ObjectStorage _target = default;
        private VisualTreeAsset _layout = default;

        public StorageItemView(ObjectStorage target, VisualTreeAsset layout)
        {
            if (target == null)
            {
                target = ScriptableObject.CreateInstance<ObjectStorage>();
                Debug.LogWarning(
                    "StorageItemView arg : target is null.\n" +
                    "A new instance has been created.\n");
            }
            else
            {
                _target = target;
            }

            if (layout == null)
            {
                _layout = new();
                Debug.LogWarning(
                    "StorageItemView arg : layout is null\n" +
                    "A new instance has been created.");
            }
            else
            {
                _layout = layout;
            }
        }


        public void SetupListView(ListView listView)
        {
            listView.makeItem = _layout.CloneTree;
            listView.bindItem = (element, index) =>
            {
                var cache = new CachedVisualElements(element);
                SetupVisualElements(cache, index);
            };
        }


        private struct CachedVisualElements
        {
            public VisualElement selectorSection;
            public ObjectField selectorSection__object;

            public VisualElement detailsSection;
            public VisualElement detailsSection__iconSection;
            public VisualElement detailsSection__main;
            public VisualElement detailsSection__controllerSection;

            public Button controllerSection__trashButton;


            public CachedVisualElements(VisualElement root)
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


                selectorSection = FindElementOrCreate
                    <VisualElement>(
                    root,
                    "selector-section");
                selectorSection__object = FindElementOrCreate
                    <ObjectField>(
                    selectorSection,
                    "selector-section__object");


                detailsSection = FindElementOrCreate
                    <VisualElement>(
                    root,
                    "details-section");
                detailsSection__iconSection = FindElementOrCreate
                    <VisualElement>(
                    detailsSection,
                    "details-section__icon-section");
                detailsSection__main = FindElementOrCreate
                    <VisualElement>(
                    detailsSection,
                    "details-section__main");
                detailsSection__controllerSection = FindElementOrCreate
                    <VisualElement>(
                    detailsSection,
                    "details-section__controller-section");

                controllerSection__trashButton = FindElementOrCreate
                    <Button>(
                    detailsSection__controllerSection,
                    "controller-section__trash-button");
            }
        }
        private void SetupVisualElements(CachedVisualElements cache, int index)
        {
            // Setup
            SetupSelectorSectionObject(cache.selectorSection__object, index);
            SetupDetailSection(cache.detailsSection);


            // Associate
            AssociateObjectWithDetailsSection(
                cache.selectorSection__object,
                cache.detailsSection);
            AssociateObjectWithDetailsSectionMain(
                cache.selectorSection__object,
                cache.detailsSection__main,
                index);
            AssociateObjectWithDetailsSectionIconSection(
                cache.selectorSection__object,
                cache.detailsSection__iconSection,
                index);


            // Populate
            PopulateSelectorSectionObject(cache.selectorSection__object, index);
        }


        private void SetupSelectorSectionObject(ObjectField field, int index)
        {
            field.SetEnabled(false);
            field.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue == null)
                {
                    _target.Objects[index] = null;
                    return;
                }
                if (evt.newValue is DataObject.BaseObject obj)
                {
                    _target.Objects[index] = obj;
                }
            });
        }
        private void SetupDetailSection(VisualElement section)
        {
            //section.style.display = DisplayStyle.None;
        }

        private void AssociateObjectWithDetailsSection
            (ObjectField field, VisualElement section)
        {
            field.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue is DataObject.BaseObject obj && obj != null)
                {
                    section.style.display = DisplayStyle.Flex;
                }
                else
                {
                    section.style.display = DisplayStyle.None;
                }
            });
        }
        private void AssociateObjectWithDetailsSectionIconSection
            (ObjectField field, VisualElement iconSection, int index)
        {
            field.RegisterValueChangedCallback(evt =>
            {
                iconSection.Clear();
                if (evt.newValue is DataObject.BaseObject obj && obj != null)
                {
                    var container = new InspectorElement(_target.Objects[index]);
                    var titleSection = container.Q<VisualElement>("title-section");
                    if (titleSection != null)
                    {
                        iconSection.Add(titleSection);
                    }
                }
            });
        }
        private void AssociateObjectWithDetailsSectionMain
            (ObjectField field, VisualElement main, int index)
        {
            field.RegisterValueChangedCallback(evt =>
            {
                main.Clear();
                if (evt.newValue is DataObject.BaseObject obj && obj != null)
                {
                    var container = new InspectorElement(_target.Objects[index]);
                    var inspector = container.Q<VisualElement>("default-inspector");
                    if (inspector != null)
                    {
                        main.Add(inspector);
                    }
                }
            });
        }

        private void PopulateSelectorSectionObject(ObjectField field, int index)
        {
            field.value = _target.Objects[index];
        }
    }
}
