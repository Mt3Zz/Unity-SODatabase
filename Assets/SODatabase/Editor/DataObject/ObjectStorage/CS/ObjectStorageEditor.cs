using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using UnityEditor.SceneManagement;


namespace SODatabase.Editor
{
    using Editor = UnityEditor.Editor;
    using BaseObject = DataObject.BaseObject;
    using ObjectStorage = DataObject.ObjectStorage;


    [CustomEditor(typeof(ObjectStorage))]
    public class ObjectStorageEditor : Editor
    {
        [SerializeField]
        private VisualTreeAsset _layout = default;
        [SerializeField]
        private VisualTreeAsset _itemLayout = default;


        private ObjectStorage _targetInstance => (ObjectStorage)target;


        public override VisualElement CreateInspectorGUI()
        {
            //ShowPropertyNames();


            var root = new VisualElement();
            if(_layout != null)
            {
                _layout.CloneTree(root);
            }


            CreateObjectList(root);
            CreateDefaultInspector(root);


            return root;
        }


        private void CreateObjectList(VisualElement root)
        {
            var container = root.Q<VisualElement>("object-list");
            if (container == null)
            {
                Debug.Log("object-list is null");
                return;
            }


            CreateObjectListView(container);
            CreateObjectCreationField(container);
        }
        private void CreateObjectListView(VisualElement container)
        {
            var list = container.Q<ListView>("object-list__view");
            if (container == null)
            {
                Debug.Log("object-list__view is null");
                return;
            }


            if(_itemLayout == null)
            {
                Debug.Log("_itemLayout is null");
                return;
            }
            list.makeItem = _itemLayout.CloneTree;
            list.bindItem = (element, index) =>
            {
                var field = element.Q<ObjectField>("object-field");
                if (field == null)
                {
                    Debug.Log("object-field is null");
                    return;
                }
                var inspector = element.Q<VisualElement>("object-inspector");
                if (inspector == null)
                {
                    Debug.Log("object-inspector is null");
                    return;
                }
                var storage = (ObjectStorage)target;


                field.RegisterValueChangedCallback(evt =>
                {
                    var obj = (BaseObject)evt.newValue;

                    inspector.Clear();
                    storage.Objects[index] = obj;

                    if (obj != null)
                    {
                        inspector.Add(new InspectorElement(obj));
                    }
                });
                field.value = storage.Objects[index];
            };
            //*/
        }
        private void CreateObjectCreationField(VisualElement container)
        {
            var dropdown = container.Q<DropdownField>("object-list__type-selector");
            if (dropdown == null)
            {
                Debug.Log("object-list__type-selector is null");
                return;
            }
            var button = container.Q<Button>("object-list__addition-button");
            if(button == null)
            {
                Debug.Log("object-list__addition-button is null");
                return;
            }


            var folderPreferences = PackagePreferences.instance.FolderPreferences;
            var folder = folderPreferences.ItemFolder;

            var subclasses = System.Reflection.Assembly
                .GetAssembly(typeof(BaseObject))
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(BaseObject)) && !x.IsAbstract)
                .ToList();


            dropdown.index = -1;
            dropdown.choices = subclasses
                .Select(subclass => subclass.Name)
                .ToList();
            dropdown.RegisterValueChangedCallback(evt =>
            {
                button.SetEnabled(true);
                folder = folderPreferences.ItemFolder + $"/{evt.newValue}";
            });


            button.SetEnabled(false);
            button.clicked += () =>
            {
                folderPreferences.CreateDirectoryRecursively(folder);
                

                // 保存先のファイルパスを取得
                var path = EditorUtility.SaveFilePanelInProject(
                    "Save Item", // 開かれるウィンドウのタイトル
                    "DataObject", // 入力されている名前
                    "asset", // 入力されている拡張子
                    "", // なんだかわからない
                    folder // 開いたとき表示されるフォルダ
                    );

                // 選択されたならパスが入っている。キャンセルされたなら入っていない。
                if (!string.IsNullOrEmpty(path))
                {
                    // 保存処理
                    if (dropdown.index >= 0 && dropdown.index < subclasses.Count)
                    {
                        var storage = CreateInstance(subclasses[dropdown.index]) as BaseObject;
                        AssetDatabase.CreateAsset(storage, path);

                        _targetInstance.Objects.Add(storage);
                    }
                }
            };
        }


        private void CreateDefaultInspector(VisualElement root)
        {
            var container = root.Q<Foldout>("default-inspector");
            if (container == null)
            {
                Debug.Log("default-inspector is null");
                return;
            }


            InspectorElement.FillDefaultInspector(container, serializedObject, this);
        }


        private void ShowPropertyNames()
        {
            var iterator = serializedObject.GetIterator();
            while (iterator.NextVisible(true))
            {
                Debug.Log(iterator.propertyPath);
            }
        }
    }
}
