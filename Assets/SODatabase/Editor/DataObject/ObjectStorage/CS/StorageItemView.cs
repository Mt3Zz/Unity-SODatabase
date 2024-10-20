using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace SODatabase.Editor
{
    using BaseObject = DataObject.BaseObject;

    internal class StorageItemView
    {
        private bool _isTrashedItem;


        internal StorageItemView(bool isTrashedObject = false)
        {
            _isTrashedItem = isTrashedObject;
        }


        internal void SetupDetailsSectionLink(ObjectField field)
        {
            field.SetEnabled(false);
        }
        internal void SetupDetailsSectionTrashButton(Button button)
        {
            if (_isTrashedItem)
            {
                button.style.display = DisplayStyle.None;
            }
            else
            {
                button.style.display = DisplayStyle.Flex;
            }
        }
        internal void SetupDetailsSectionRestoreButton(Button button)
        {
            if (_isTrashedItem)
            {
                button.style.display = DisplayStyle.Flex;
            }
            else
            {
                button.style.display = DisplayStyle.None;
            }
        }
        internal void SetupDetailsSectionDeleteButton(Button button)
        {
            if (_isTrashedItem)
            {
                button.style.display = DisplayStyle.Flex;
            }
            else
            {
                button.style.display = DisplayStyle.None;
            }
        }

        internal void AssociateDetailsSection__LinkWithTitleSection
            (ObjectField link, VisualElement titleSection)
        {
            link.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue is BaseObject obj && obj != null)
                {
                    var container = new InspectorElement(obj);

                    var objTitleSection = container.Q<VisualElement>("title-section");
                    if (objTitleSection == null)
                    {
                        Debug.Log($"title-section is null.\n");
                        return;
                    }
                    var objTitleField = objTitleSection.Q<TextField>("title-section__placeholder");
                    if (objTitleField == null)
                    {
                        Debug.Log($"title-section__placeholder is null.\n");
                        return; 
                    }

                    titleSection.Clear();
                    titleSection.Add(objTitleField);
                }
            });
        }
        internal void AssociateDetailsSection__LinkWithMain
            (ObjectField link, VisualElement main)
        {
            link.RegisterValueChangedCallback(evt =>
            {
                main.Clear();
                if (evt.newValue is BaseObject obj && obj != null)
                {
                    var container = new InspectorElement(obj);

                    var inspector = container.Q<VisualElement>("default-inspector");
                    if (inspector == null)
                    {
                        Debug.Log($"default-inspector is null.\n");
                        return;
                    }

                    main.Clear();
                    main.Add(inspector);
                }
            });
        }

        internal void PopulateDetailsSection__Link(ObjectField link, BaseObject obj)
        {
            //link.value = _target.Objects[index];
            link.value = obj;
        }
        internal void PopulateButtonWithAction(Button button, Action onButtonClicked)
        {
            button.clicked += onButtonClicked;
        }
    }
}
