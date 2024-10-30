using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif


namespace SODatabase.DataObject
{
    public class ObjectStorage : ScriptableObject
    {
        public ReadOnlyCollection<BaseObject> Objects => _objects.AsReadOnly();
        [SerializeField]
        private List<BaseObject> _objects = new();
        internal IReadOnlyList<BaseObject> TrashedObjects => _trashedObjects;
        [SerializeField]
        private List<BaseObject> _trashedObjects = new();

        /*/
        internal IList<BaseObject> DuplicatedObjects
        {
            get
            {
                return _objects
                    .GroupBy(obj => obj.Id)            // Idでグループ化
                    .Where(group => group.Count() > 1) // 重複があるグループをフィルタ
                    .SelectMany(group => group)        // 重複したグループを展開
                    .ToList();
            }
        }
        //*/


        // Preferences
        internal StoragePreferences Preferences => _preferences;
        private StoragePreferences _preferences = new();


        // Add an object
        internal void AppendObject(BaseObject obj)
        {
            if (obj == null) return;
            _objects.Add(obj);
        }
        // Read an object by its ID
        public BaseObject ReadObject(ObjectId id)
        {
            return _objects.FirstOrDefault(obj => obj.Id.Equals(id));
        }
        // Update an existing object
        public bool UpdateObject<T>(ObjectId id, IObjectController<T> controller)
            where T : BaseObject
        {
            var obj = ReadObject(id);

            if (obj == null) return false;
            if (obj.GetType() != typeof(T)) return false;

            return controller.UpdateObject(obj as T);
        }
        

#if UNITY_EDITOR
        // Create a new object and add it to the storage
        internal void CreateObjectForEditor(Type objectType, string path)
        {
            if (objectType != typeof(BaseObject) &&
                !typeof(BaseObject).IsAssignableFrom(objectType))
            {
                throw new ArgumentException($"Type {objectType} is not BaseObject or a subclass of BaseObject.");
            }
            var obj = CreateInstance(objectType) as BaseObject;
            if (obj != null)
            {
                AssetDatabase.CreateAsset(obj, path);
                obj.SetId(new ObjectId(obj.name));
                _objects.Add(obj);
            }
        }
        // Remove an object from storage
        internal bool DeleteObjectForEditor(BaseObject obj)
        {
            if (obj == null) return false;
            if (!_objects.Contains(obj)) return false;

            //obj.Delete();
            _objects.Remove(obj);
            _trashedObjects.Add(obj);
            return true;
        }
        // Restore a trashed object
        internal bool RestoreTrashedObjectForEditor(BaseObject obj)
        {
            if (obj == null) return false;
            if (!_trashedObjects.Contains(obj)) return false;

            //obj.Restore();
            _trashedObjects.Remove(obj);
            _objects.Add(obj);
            return true;
        }
        // Remove a trashed object
        internal bool RemoveTrashedObjectForEditor(BaseObject obj)
        {
            if (obj == null) return false;
            if (!_trashedObjects.Contains(obj)) return false;

            _trashedObjects.Remove(obj);
            return true;
        }


        internal IReadOnlyList<BaseObject> OrganizedListForEditor => _organizerForEditor.OrganizedList;
        [SerializeField]
        private StorageOrganizer _organizerForEditor = new();
        [Serializable]
        private class StorageOrganizer
        {
            internal IReadOnlyList<BaseObject> OrganizedList => _organizedList;
            [SerializeField]
            private List<BaseObject> _organizedList = new();
            internal void UpdateOrganizedList(List<BaseObject> source = null)
            {
                if (source == null) source = _organizedList;
                _organizedList = source
                    .Where(obj => TypeFilter == typeof(BaseObject) || obj.GetType() == TypeFilter)
                    .Where(obj => string.IsNullOrEmpty(NameFilter) || obj.Id.Name.Contains(NameFilter))
                    .ToList();
            }


            internal string NameFilter { get; set; } ="";
            internal Type TypeFilter { get; set; } = typeof(BaseObject);


            internal List<BaseObject> Distinct(List<BaseObject> source)
            {
                var result = source
                    .Distinct()
                    .ToList();
                UpdateOrganizedList(result);
                return result;
            }
            internal List<BaseObject> SortByName(List<BaseObject> source)
            {
                var result = source
                    .Distinct()
                    .OrderBy(obj => obj.Id.Name)
                    .ToList();
                UpdateOrganizedList(result);
                return result;
            }
            internal List<BaseObject> SortByType(List<BaseObject> source)
            {
                var result = source
                    .Distinct()
                    .GroupBy(obj => obj.GetType())
                    .OrderBy(group => group.Key.Name)
                    .SelectMany(group => group)
                    .ToList();
                UpdateOrganizedList(result);
                return result;
            }
            internal List<BaseObject> SortByAssetName(List<BaseObject> source)
            {
                var result = source
                    .Distinct()
                    .OrderBy(obj => obj.name)
                    .ToList();
                UpdateOrganizedList(result);
                return result;
            }
        }
        internal void UpdateOrganizedListForEditor(bool targetsTrashedObjects = false)
        {
            if (targetsTrashedObjects)
            {
                _organizerForEditor.UpdateOrganizedList(_trashedObjects);
            }
            else
            {
                _organizerForEditor.UpdateOrganizedList(_objects);
            }
        }

        internal void DistinctForEditor(bool targetsTrashedObjects = false)
        {
            if (targetsTrashedObjects)
            {
                _trashedObjects = _organizerForEditor.Distinct(_trashedObjects);
            }
            else
            {
                _objects = _organizerForEditor.Distinct(_objects);
            }
        }
        internal void SortByNameForEditor(bool targetsTrashedObjects = false)
        {
            if (targetsTrashedObjects)
            {
                _trashedObjects = _organizerForEditor.SortByName(_trashedObjects);
            }
            else
            {
                _objects = _organizerForEditor.SortByName(_objects);
            }
        }
        internal void SortByTypeForEditor(bool targetsTrashedObjects = false)
        {
            if (targetsTrashedObjects)
            {
                _trashedObjects = _organizerForEditor.SortByType(_trashedObjects);
            }
            else
            {
                _objects = _organizerForEditor.SortByType(_objects);
            }
        }
        internal void SortByAssetNameForEditor(bool targetsTrashedObjects = false)
        {
            if (targetsTrashedObjects)
            {
                _trashedObjects = _organizerForEditor.SortByAssetName(_trashedObjects);
            }
            else
            {
                _objects = _organizerForEditor.SortByAssetName(_objects);
            }
        }

        internal void InitFilterForEditor(bool targetsTrashedObjects = false)
        {
            SetFilterForEditor(string.Empty, typeof(BaseObject), targetsTrashedObjects);
        }
        internal void SetFilterForEditor(string nameFilter = null, Type typeFilter = null, bool targetsTrashedObjects = false)
        {
            if (nameFilter != null)
            {
                _organizerForEditor.NameFilter = nameFilter;
            }
            if (typeFilter != null)
            {
                _organizerForEditor.TypeFilter = typeFilter;
            }

            var target = targetsTrashedObjects ? _trashedObjects : _objects;
            _organizerForEditor.UpdateOrganizedList(target);
        }
#endif
    }
    internal class StoragePreferences
    {
        internal Type ObjectType => _objectType;
        private Type _objectType = typeof(BaseObject);


#if UNITY_EDITOR
        internal void SetObjectTypeForEditor(Type storageType)
        {
            _objectType = storageType;
        }
#endif
    }
}
