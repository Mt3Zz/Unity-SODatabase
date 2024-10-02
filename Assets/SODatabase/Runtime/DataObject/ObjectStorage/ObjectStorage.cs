using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SODatabase.DataObject
{
    public class ObjectStorage : ScriptableObject
    {
        public IList<BaseObject> Objects => _objects;
        [SerializeField]
        private List<BaseObject> _objects = new();
        private List<BaseObject> _trashedObjects = new();

        internal IList<BaseObject> DuplicatedObjects
        {
            get
            {
                var objectsById = new Dictionary<ObjectId, List<BaseObject>>();

                foreach (var obj in _objects)
                {
                    if (objectsById.ContainsKey(obj.Id))
                    {
                        objectsById[obj.Id].Add(obj); // 重複する場合はMyClassを追加
                    }
                    else
                    {
                        objectsById[obj.Id] = new() { obj }; // 初回はMyClassを新規リストに追加
                    }
                }

                // 重複している要素を抽出
                var duplicates = objectsById
                    .Where(kvp => kvp.Value.Count > 1) // 出現回数が2回以上のものをフィルタ
                    .SelectMany(kvp => kvp.Value) // 重複したBaseObjectを追加
                    .ToList();

                return duplicates;
            }
        }


        // Preferences
        internal StoragePreferences Preferences => _preferences;
        private StoragePreferences _preferences = new();


#if UNITY_EDITOR
        // Create a new object and add it to the storage
        public void CreateObjectForEditor(Type objectType, string path)
        {
            if (
                objectType != typeof(BaseObject) && 
                !typeof(BaseObject).IsAssignableFrom(objectType)
                )
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

        // Delete an object by its ID
        public bool DeleteObjectForEditor(ObjectId id)
        {
            var obj = ReadObject(id);
            if (obj != null)
            {
                obj.Delete();
                _objects.Remove(obj);
                _trashedObjects.Add(obj);
                return true;
            }
            return false;
        }
#endif

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

            var isUpdated = controller.Update(obj as T);

            if (isUpdated) return true;
            return false;
        }


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
