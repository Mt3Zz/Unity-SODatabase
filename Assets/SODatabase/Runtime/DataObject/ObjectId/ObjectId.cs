using System;

namespace SODatabase.DataObject
{
    public struct ObjectId : IEquatable<ObjectId>
    {
        public int Id => _id;
        private int _id;

        public string Name => _name;
        private string _name;


        internal ObjectId(int id, string name)
        {
            _id = id;
            _name = name;
        }


#if UNITY_EDITOR
        internal void SetIdForEditor(int id)
        {
            _id = id;
        }
        internal void SetNameForEditor(string name)
        {
            _name = name;
        }
#endif


        public bool Equals(ObjectId other)
        {
            return Id == other.Id && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is ObjectId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}
