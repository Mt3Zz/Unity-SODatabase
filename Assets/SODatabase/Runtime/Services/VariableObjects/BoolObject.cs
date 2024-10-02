using UnityEngine;

namespace SODatabase.DataObject
{
    public sealed class BoolObject : BaseObject
    {
        public bool Value => _value;
        [SerializeField]
        private bool _value = false;
    }
}
