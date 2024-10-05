using UnityEngine;

namespace SODatabase.DataObject
{
    public sealed class FloatObject : BaseObject
    {
        public float Value
        {
            get { return _value; }
            internal set { _value = value; }
        }
        [SerializeField]
        private float _value = 0;
    }
}
