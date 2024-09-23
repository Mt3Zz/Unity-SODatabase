using System;
using UnityEngine;

namespace SODatabase.DataObject
{
    public abstract class BaseObject : ScriptableObject, IEquatable<BaseObject>
    {
        public ObjectId Id => _id;
        [SerializeField]
        private ObjectId _id;

        internal Guid Uuid { get; } = Guid.NewGuid();

        internal DateTime CreatedAt { get; } = DateTime.UtcNow;
        internal DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        internal bool IsDeleted { get; private set; } = false;
        internal int Version { get; private set; } = 1;


        protected void UpdateRecordInfo()
        {
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }


        internal void SetId(ObjectId id)
        {
            _id = id;
        }


        internal void Delete()
        {
            if (!IsDeleted)
            {
                IsDeleted = true;
                UpdateRecordInfo(); // 変更履歴を更新
            }
        }
        internal void Restore()
        {
            if (IsDeleted)
            {
                IsDeleted = false;
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
