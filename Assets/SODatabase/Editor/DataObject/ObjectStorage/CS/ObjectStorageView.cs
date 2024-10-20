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
        private ObjectStorage _target;
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
            _target = target;
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
            label.text = _target.name;
        }


        // Preference Section
        // [W.I.P.] –¢ŽÀ‘•‚Ìƒƒ\ƒbƒh‚Å‚·
        public void SetupPreferenceSection__TypeSelector(DropdownField typeSelector)
        {
        }


        // Object Section
        private bool _targetsTrashedObjects = false;
        private DisplayedListType _listType;
        private enum DisplayedListType
        {
            RecordList,
            TrashBox
        }
        public void SetupObjectSection__list(ListView objectList)
        {
            _target.InitOrganizerForEditor();

            objectList.bindingPath = "_organizerForEditor._organizedList";
            objectList.makeItem = _itemLayout.Instantiate;

            RebuildObjectSection__list(objectList);
        }
        private void RebuildObjectSection__list(ListView objectList)
        {
            objectList.bindItem = (element, index) =>
            {
                var obj = _target.OrganizedListForEditor[index];

                var itemFacade = new StorageItemViewFacade();
                itemFacade.BindingObject = obj;
                itemFacade.IsTrashedItem = _targetsTrashedObjects;
                itemFacade.OnTrashButtonClicked = () =>
                {
                    if (_target.Objects.Contains(obj))
                    {
                        //Debug.Log($"{obj.name} is trashed");
                        _target.DistinctForEditor();
                        _target.DeleteObjectForEditor(obj);
                    }
                    _target.InitOrganizerForEditor(_targetsTrashedObjects);
                };
                itemFacade.OnRestoreButtonClicked = () =>
                {
                    if (_target.TrashedObjects.Contains(obj))
                    {
                        //Debug.Log($"{obj.name} is restored");
                        _target.DistinctForEditor(true);
                        _target.RestoreTrashedObjectForEditor(obj);
                    }
                    _target.InitOrganizerForEditor(_targetsTrashedObjects);
                };
                itemFacade.OnDeleteButtonClicked = () =>
                {
                    if (_target.TrashedObjects.Contains(obj))
                    {
                        //Debug.Log($"{obj.name} is deleted");
                        _target.DistinctForEditor(true);
                        _target.RemoveTrashedObjectForEditor(obj);
                    }
                    _target.InitOrganizerForEditor(_targetsTrashedObjects);
                };


                itemFacade.SetupItem(element);
            };


            objectList.Rebuild();
            objectList.RefreshItems();
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

                _target.InitOrganizerForEditor(_targetsTrashedObjects);
                RebuildObjectSection__list(objectList);
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
            var folderPath = folderPreferences.ItemFolder + $"/{_target.name}";


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
                    _target.CreateObjectForEditor(_selectedType, path);
                }
            };
        }
        public void SetupObjectAppenderSection__ExistingObjectAppender(Button button)
        {
            var folderPreferences = PackagePreferences.instance.FolderPreferences;
            var folderPath = folderPreferences.ItemFolder + $"/{_target.name}";


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
                        _target.Objects.Add(obj);
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
            _target.SetFilterForEditor(typeFilter: typeof(BaseObject));

        }
        public void SetupFilterSection__NameFilter(TextField nameFilter)
        {
            nameFilter.value = "";
            _target.SetFilterForEditor(nameFilter: "");

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
                        _target.SortByNameForEditor(_targetsTrashedObjects);
                        break;
                    case SortType.Type:
                        _target.SortByTypeForEditor(_targetsTrashedObjects);
                        break;
                    case SortType.AssetName:
                        _target.SortByAssetNameForEditor(_targetsTrashedObjects);
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
                _target.DistinctForEditor(_targetsTrashedObjects);
            };
        }


        // Associate Sections
        public void AssociateListOrganizerSectionWithObjectSection__ButtonWithList
            (Button button, ListView objectList)
        {
            button.clicked += () =>
            {
                _target.InitOrganizerForEditor(_targetsTrashedObjects);

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
                    _target.SetFilterForEditor(typeFilter: typeFilter);
                }
                else
                {
                    _target.SetFilterForEditor(typeFilter: typeof(BaseObject));
                }


                RebuildObjectSection__list(objectList);
            });
        }
        public void AssociateFilterSectionWithObjectSection__NameFilterWithList
            (TextField textField, ListView objectList)
        {
            textField.RegisterValueChangedCallback(evt =>
            {
                _target.SetFilterForEditor(nameFilter: evt.newValue);

                RebuildObjectSection__list(objectList);
            });
        }
    }
}
