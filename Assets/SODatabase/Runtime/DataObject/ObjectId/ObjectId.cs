using System;
using UnityEngine;

namespace SODatabase.DataObject
{
    [Serializable]
    public struct ObjectId : IEquatable<ObjectId>
    {
        public string Name => _name;
        [SerializeField]
        private string _name;


        internal ObjectId(string name)
        {
            _name = name;
        }


#if UNITY_EDITOR
        internal void SetNameForEditor(string name)
        {
            _name = name;
        }
#endif


        public bool Equals(ObjectId other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is ObjectId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
