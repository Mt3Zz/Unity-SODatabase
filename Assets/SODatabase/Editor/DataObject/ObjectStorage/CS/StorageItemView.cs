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
        internal void SetupLink(ObjectField field)
        {
            field.SetEnabled(false);
        }

        internal void AssociateLinkWithTitleSection(ObjectField link, VisualElement titleSection)
        {
            link.RegisterValueChangedCallback(evt =>
            {
                titleSection.Clear();
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

                    titleSection.Add(objTitleField);
                }
            });
        }
        internal void AssociateLinkWithMain(ObjectField link, VisualElement main)
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
        internal void AssociateLinkWithButton(ObjectField link, Button button)
        {
            link.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue is BaseObject obj && obj != null)
                {
                    button.userData = obj;
                }
                else
                {
                    button.userData = null;
                }
            }); 
        }

        internal void PopulateLink(ObjectField link, BaseObject obj)
        {
            link.value = obj;
        }
        internal void PopulateButtonWithAction(Button button, Action<BaseObject> onButtonClicked)
        {
            button.clicked += () =>
            {
                onButtonClicked(button.userData as BaseObject);
            };
        }

        internal void DisplayNormalModeButton(Button button, bool isTrashedItem = false)
        {
            if (isTrashedItem)
            {
                button.style.display = DisplayStyle.None;
            }
            else
            {
                button.style.display = DisplayStyle.Flex;
            }
        }
        internal void DisplayTrashBoxModeButton(Button button, bool isTrashedItem = true)
        {
            if (isTrashedItem)
            {
                button.style.display = DisplayStyle.Flex;
            }
            else
            {
                button.style.display = DisplayStyle.None;
            }
        }
    }
}
