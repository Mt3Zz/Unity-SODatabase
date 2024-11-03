using UnityEngine;

namespace SODatabase
{
    public sealed class BoolObject : DataObject.BaseObject
    {
        public bool Value
        {
            get => _value;
            set => _value = value;
        }
        [SerializeField]
        private bool _value = false;


        public void Toggle()
        {
            _value = !_value;
        }
    }
}
