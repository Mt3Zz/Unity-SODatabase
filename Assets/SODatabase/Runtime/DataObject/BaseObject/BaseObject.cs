using System;
using UnityEngine;

namespace SODatabase
{
    internal abstract class BaseObject : ScriptableObject, IEquatable<BaseObject>
    {
        public Guid Uuid { get; } = Guid.NewGuid();

        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        public bool IsDeleted { get; private set; } = false;
        public int Version { get; private set; } = 1;


        protected void UpdateRecordInfo()
        {
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }


        public void Delete()
        {
            if (!IsDeleted)
            {
                IsDeleted = true;
                UpdateRecordInfo(); // 変更履歴を更新
            }
        }
        public void Restore()
        {
            if (IsDeleted)
            {
                IsDeleted = false;
                UpdateRecordInfo(); // 変更履歴を更新
            }
        }


        public void Save(ISaver saver)
        {
            saver.Save(this);
        }
        public void Load (ISaver saver)
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
