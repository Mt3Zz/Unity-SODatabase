using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace SODatabase.Editor
{
    using BaseObject = DataObject.BaseObject;
    using ObjectStorage = DataObject.ObjectStorage;


    internal class ObjectStorageView
    {
        private ObjectStorage _storage;
        private VisualTreeAsset _itemLayout;


        private readonly List<Type> _classAndSubclasses;
        private readonly List<Type> _subclasses;

        /*/
        private (bool  , bool TrashBoxMode) State { get; set; }
        private ModeState _state = ModeState.all;
        private enum ModeState
        {
            all,
            TrashBoxMode
        }
        //*/


        public ObjectStorageView(ObjectStorage target, VisualTreeAsset itemLayout)
        {
            _storage = target;
            _itemLayout = itemLayout;


            _subclasses = System.Reflection.Assembly
                .GetAssembly(typeof(BaseObject))
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(BaseObject)) && !x.IsAbstract)
                .ToList();
            _classAndSubclasses = new() { typeof(BaseObject) };
            _classAndSubclasses.AddRange(_subclasses);
        }


        // Title Section
        public void SetupTitleSection__Title(Label label)
        {
            label.text = _storage.name;
        }


        // Preference Section
        // [W.I.P.] –¢ŽÀ‘•‚Ìƒƒ\ƒbƒh‚Å‚·
        public void SetupPreferenceSection__TypeSelector(DropdownField typeSelector)
        {
        }


        // Object Section
        private bool _targetsTrashedObjects = false;
        private StorageItemViewFacade _itemViewFacade = new();
        private enum DisplayedListType
        {
            RecordList,
            TrashBox
        }
        public void SetupObjectSection__list(ListView objectList)
        {
            _storage.UpdateOrganizedListForEditor();
            

            _itemViewFacade.OnTrashButtonClicked = (obj) =>
            {
                if (_storage.Objects.Contains(obj))
                {
                    //Debug.Log($"{obj.name} is trashed");
                    _storage.DistinctForEditor();
                    _storage.DeleteObjectForEditor(obj);
                }
                _storage.UpdateOrganizedListForEditor(_targetsTrashedObjects);
                objectList.Rebuild();
                objectList.RefreshItems();
            };
            _itemViewFacade.OnRestoreButtonClicked = (obj) =>
            {
                if (_storage.TrashedObjects.Contains(obj))
                {
                    //Debug.Log($"{obj.name} is restored");
                    _storage.DistinctForEditor(true);
                    _storage.RestoreTrashedObjectForEditor(obj);
                }
                _storage.UpdateOrganizedListForEditor(_targetsTrashedObjects);
                objectList.Rebuild();
                objectList.RefreshItems();
            };
            _itemViewFacade.OnDeleteButtonClicked = (obj) =>
            {
                if (_storage.TrashedObjects.Contains(obj))
                {
                    //Debug.Log($"{obj.name} is deleted");
                    _storage.DistinctForEditor(true);
                    _storage.RemoveTrashedObjectForEditor(obj);
                }
                _storage.UpdateOrganizedListForEditor(_targetsTrashedObjects);
                objectList.Rebuild();
                objectList.RefreshItems();
            };


            objectList.bindingPath = "_organizerForEditor._organizedList";
            objectList.makeItem = () =>
            {
                var root = _itemLayout.Instantiate();
                _itemViewFacade.InitItem(root);
                return root;
            };
            objectList.bindItem = (element, index) =>
            {
                if(0 <= index && index < _storage.OrganizedListForEditor.Count)
                {
                    var obj = _storage.OrganizedListForEditor[index];
                    _itemViewFacade.UpdateItem(obj, _targetsTrashedObjects);
                }
            };
        }
        public void AssociateObjectSection__listSelectorWithList
            (EnumField listSelector, ListView objectList)
        {
            listSelector.RegisterValueChangedCallback(evt =>
            {
                switch (evt.newValue)
                {
                    case DisplayedListType.RecordList:
                        _targetsTrashedObjects = false;
                        break;
                    case DisplayedListType.TrashBox:
                        _targetsTrashedObjects = true;
                        break;
                    default:
                        throw new ArgumentException($"Unknown enum value: {evt.newValue}.");
                }

                _storage.UpdateOrganizedListForEditor(_targetsTrashedObjects);
                objectList.Rebuild();
                objectList.RefreshItems();
            });
        }
        public void PopulateObjectSection__listSelector(EnumField listSelector)
        {
            listSelector.Init(DisplayedListType.RecordList);
        }


        // Controller Section
        // Object Appender Section
        private Type _selectedType = default;
        public void SetupObjectAppenderSection__TypeSelector(DropdownField dropdown)
        {
            var folderPreferences = PackagePreferences.instance.FolderPreferences;


            dropdown.index = -1;

            dropdown.choices = _subclasses
                .Select(subclass => subclass.Name)
                .ToList();


            dropdown.RegisterValueChangedCallback(evt =>
            {
                var index = dropdown.index;
                if (index >= 0 && index < _subclasses.Count)
                {
                    _selectedType = _subclasses[index];
                }
                else
                {
                    dropdown.index = -1;
                }
            });
        }
        public void SetupObjectAppenderSection__ObjectCreater(Button button)
        {
            var folderPreferences = PackagePreferences.instance.FolderPreferences;
            var folderPath = folderPreferences.ItemFolder + $"/{_storage.name}";


            button.SetEnabled(false);

            button.clicked += () =>
            {
                folderPreferences.CreateDirectoryRecursively(folderPath);

                var path = EditorUtility.SaveFilePanelInProject(
                    "Save Record",
                    "DataObject",
                    "asset",
                    "",
                    folderPath
                );

                if (!string.IsNullOrEmpty(path))
                {
                    _storage.CreateObjectForEditor(_selectedType, path);
                }
            };
        }
        public void SetupObjectAppenderSection__ExistingObjectAppender(Button button)
        {
            var folderPreferences = PackagePreferences.instance.FolderPreferences;
            var folderPath = folderPreferences.ItemFolder + $"/{_storage.name}";


            button.clicked += () =>
            {
                folderPreferences.CreateDirectoryRecursively(folderPath);

                var path = EditorUtility.OpenFilePanel(
                    "Select Record",
                    folderPath,
                    "asset"
                );

                if (!string.IsNullOrEmpty(path))
                {
                    var relativePath = FileUtil.GetProjectRelativePath(path);
                    //Debug.Log($"Path : {relativePath}");
                    var obj = AssetDatabase.LoadAssetAtPath<BaseObject>(relativePath);

                    if(obj != null)
                    {
                        //Debug.Log($"Object Type : {obj.GetType()}\nName : {obj.name}");
                        //obj.Restore();
                        _storage.AppendObject(obj);
                    }
                }
            };
        }
        public void AssociateObjectAppenderSection__TypeSelectorWithObjectCreater
            (DropdownField dropdown, Button button)
        {
            dropdown.RegisterValueChangedCallback(evt =>
            {
                if (dropdown.index != -1)
                {
                    button.SetEnabled(true);
                }
            });
        }


        // Filter Section
        private NameFilterType _currentNameFilterType;
        private enum NameFilterType
        {
            Contains,
            StartWith,
            EndWith
        }
        public void SetupFilterSection__TypeFilter(DropdownField typeFilter)
        {
            typeFilter.choices = _classAndSubclasses
                .Select(type => type == typeof(BaseObject) ? "(All)" : type.Name)
                .ToList();


            typeFilter.index = 0;
            _storage.SetFilterForEditor(typeFilter: typeof(BaseObject));

        }
        public void SetupFilterSection__NameFilter(TextField nameFilter)
        {
            nameFilter.value = "";
            _storage.SetFilterForEditor(nameFilter: "");

        }
        public void SetupFilterSection__NameFilterType(EnumField nameFilterType)
        {
            nameFilterType.Init(NameFilterType.Contains);

            nameFilterType.RegisterValueChangedCallback(evt =>
            {
                _currentNameFilterType = (NameFilterType)evt.newValue;
            });
        }
        public void AssociateFilterSection__FilterCleanerWithTypeFilter
            (Button filterCleaner, DropdownField typeFilter)
        {
            filterCleaner.clicked += () =>
            {
                typeFilter.index = 0;
            };
        }
        public void AssociateFilterSection__FilterCleanerWithNameFilter
            (Button filterCleaner, TextField nameFilter)
        {
            filterCleaner.clicked += () =>
            {
                nameFilter.value = "";
            };
        }


        // List Organizer Section
        private SortType _currentSortType;
        private enum SortType
        {
            Name,
            Type,
            AssetName
        }
        public void SetupListOrganizerSection__Sort(Button sortButton)
        {
            sortButton.clicked += () =>
            {
                switch (_currentSortType)
                {
                    case SortType.Name:
                        _storage.SortByNameForEditor(_targetsTrashedObjects);
                        break;
                    case SortType.Type:
                        _storage.SortByTypeForEditor(_targetsTrashedObjects);
                        break;
                    case SortType.AssetName:
                        _storage.SortByAssetNameForEditor(_targetsTrashedObjects);
                        break;
                    default:
                        throw new System.ArgumentException($"Unknown enum value: {_currentSortType}.");
                }
            };
        }
        public void SetupListOrganizerSection__SortTypeSelector(EnumField sortTypeSelector)
        {
            sortTypeSelector.Init(SortType.Name);
            sortTypeSelector.RegisterValueChangedCallback(evt =>
            {
                _currentSortType = (SortType)evt.newValue;
            });
        }
        public void SetupListOrganizerSection__Distinct(Button distinctButton)
        {
            distinctButton.clicked += () =>
            {
                _storage.DistinctForEditor(_targetsTrashedObjects);
            };
        }


        // Associate Sections
        public void AssociateListOrganizerSectionWithObjectSection__ButtonWithList
            (Button button, ListView objectList)
        {
            button.clicked += () =>
            {
                _storage.UpdateOrganizedListForEditor(_targetsTrashedObjects);

                objectList.Rebuild();
                objectList.RefreshItems();
            };
        }
        public void AssociateFilterSectionWithObjectSection__TypeFilterWithList
            (DropdownField typeFilter, ListView objectList)
        {
            typeFilter.RegisterValueChangedCallback(evt =>
            {
                var index = typeFilter.index;
                if (index >= 0 && index < _classAndSubclasses.Count)
                {
                    var typeFilter = _classAndSubclasses[index];
                    _storage.SetFilterForEditor(typeFilter: typeFilter);
                }
                else
                {
                    _storage.SetFilterForEditor(typeFilter: typeof(BaseObject));
                }


                _storage.UpdateOrganizedListForEditor(_targetsTrashedObjects);
                objectList.Rebuild();
                objectList.RefreshItems();
            });
        }
        public void AssociateFilterSectionWithObjectSection__NameFilterWithList
            (TextField textField, ListView objectList)
        {
            textField.RegisterValueChangedCallback(evt =>
            {
                _storage.SetFilterForEditor(nameFilter: evt.newValue);


                _storage.UpdateOrganizedListForEditor(_targetsTrashedObjects);
                objectList.Rebuild();
                objectList.RefreshItems();
            });
        }
    }
}
