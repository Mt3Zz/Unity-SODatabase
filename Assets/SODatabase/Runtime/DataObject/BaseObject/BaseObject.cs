using System;
using UnityEngine;

namespace SODatabase.DataObject
{
    public abstract class BaseObject : ScriptableObject, IEquatable<BaseObject>
    {
        public ObjectId Id => _id;
        [SerializeField, HideInInspector]
        private ObjectId _id = default;


        internal Guid Uuid => Guid.Parse(_uuid);
        [SerializeField, HideInInspector]
        private string _uuid = Guid.NewGuid().ToString();

        internal DateTime CreatedAt => DateTime.Parse(_createdAt);
        [SerializeField, HideInInspector]
        private string _createdAt = DateTime.UtcNow.ToString();

        internal DateTime UpdatedAt => DateTime.Parse(_updatedAt);
        [SerializeField, HideInInspector]
        private string _updatedAt = DateTime.UtcNow.ToString();

        internal bool IsDeleted => _isDeleted;
        [SerializeField, HideInInspector]
        private bool _isDeleted = false;

        internal int Version => _version;
        [SerializeField, HideInInspector]
        private int _version = 1;


        protected void UpdateRecordInfo()
        {
            _updatedAt = DateTime.UtcNow.ToString();
            _version++;
        }


        internal void SetId(ObjectId id)
        {
            _id = id;
        }


        internal void Delete()
        {
            if (!IsDeleted)
            {
                _isDeleted = true;
                UpdateRecordInfo(); // 変更履歴を更新
            }
        }
        internal void Restore()
        {
            if (IsDeleted)
            {
                _isDeleted = false;
                UpdateRecordInfo(); // 変更履歴を更新
            }
        }


        internal void Save(ISaver saver)
        {
            saver.Save(this);
        }
        internal void Load (ISaver saver)
        {
            saver.Load(this);
        }


        public bool Equals(BaseObject other)
        {
            return Uuid == other.Uuid;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is BaseObject other) return Equals(other);
            return false;
        }
        public override int GetHashCode()
        {
            return Uuid.GetHashCode();
        }
    }
}
