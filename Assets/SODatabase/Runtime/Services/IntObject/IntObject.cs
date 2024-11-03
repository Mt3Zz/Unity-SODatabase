using UnityEngine;

namespace SODatabase.DataObject
{
    public sealed class IntObject : BaseObject
    {
        public int Value 
        { 
            get { return _value; }
            internal set {  _value = value; }
        }
        [SerializeField]
        private int _value = 0;
    }
}
