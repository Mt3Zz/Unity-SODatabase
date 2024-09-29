using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace SODatabase.Editor
{
    using BaseObject = DataObject.BaseObject;
    using ObjectStorage = DataObject.ObjectStorage;


    public class ObjectStorageView
    {
        private ObjectStorage _target;

        public ObjectStorageView(ObjectStorage target)
        {
            _target = target;
        }


        public void SetupObjectSectionListItem(VisualElement element, int index)
        {
            var field = element.Q<ObjectField>("object-field");
            if (field == null)
            {
                Debug.LogError("Object field is null.");
                return;
            }
            var inspector = element.Q<VisualElement>("object-inspector");
            if (inspector == null)
            {
                Debug.LogError("Object inspector is null.");
                return;
            }


            field.RegisterValueChangedCallback(evt =>
            {
                inspector.Clear();
                if (evt.newValue is BaseObject obj)
                {
                    _target.Objects[index] = obj;

                    if (obj != null)
                    {
                        inspector.Add(new InspectorElement(obj));
                    }
                }
            });
            // RegisterCallback の後に置くことで、この変更にもコールバックが適用される
            field.value = _target.Objects[index];
        }
        public void SetupObjectSectionTypeSelector(DropdownField dropdown)
        {
            dropdown.index = -1;


            var subclasses = System.Reflection.Assembly
                .GetAssembly(typeof(BaseObject))
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(BaseObject)) && !x.IsAbstract)
                .ToList();

            dropdown.choices = subclasses
                .Select(subclass => subclass.Name)
                .ToList();
        }
        public void SetupObjectSectionObjectCreater(Button button)
        {
            button.SetEnabled(false);
        }


        public void AssociateTypeSelecltorWithObjectCreater(DropdownField dropdown, Button button)
        {
            var folderPreferences = PackagePreferences.instance.FolderPreferences;
            var folder = folderPreferences.ItemFolder;

            var subclasses = System.Reflection.Assembly
                .GetAssembly(typeof(BaseObject))
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(BaseObject)) && !x.IsAbstract)
                .ToList();


            dropdown.RegisterValueChangedCallback(evt =>
            {
                folder = folderPreferences.ItemFolder + $"/{evt.newValue}";

                if (dropdown.index != -1)
                {
                    button.SetEnabled(true);
                }
            });

            button.clicked += () =>
            {
                folderPreferences.CreateDirectoryRecursively(folder);
                var path = EditorUtility.SaveFilePanelInProject(
                    "Save Item",
                    "DataObject",
                    "asset",
                    "",
                    folder
                );

                if (string.IsNullOrEmpty(path)) return;
                if (dropdown.index >= 0 && dropdown.index < subclasses.Count)
                {
                    var type = subclasses[dropdown.index];
                    _target.CreateObjectForEditor(type, path);
                }
            };
        }
    }
}
