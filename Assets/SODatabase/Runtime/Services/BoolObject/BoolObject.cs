using UnityEngine;

namespace SODatabase.DataObject
{
    public sealed class BoolObject : BaseObject
    {
        public bool Value
        {
            get { return _value; }
            internal set { _value = value; }
        }
        [SerializeField]
        private bool _value = false;
    }
}
