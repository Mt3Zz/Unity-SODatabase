using System;
using UnityEngine;

namespace SODatabase
{
    [Serializable]
    public sealed class BoolObjectController : DataObject.IObjectController<BoolObject>
    {
        public BoolObjectController(bool newValue)
        {
            _newValue = newValue;
        }


        public bool NewValue
        {
            get => _newValue;
            set => _newValue = value;
        }
        [SerializeField]
        private bool _newValue = false;


        public bool UpdateObject(BoolObject obj)
        {
            if (obj == null) return false;

            obj.Value = NewValue;
            return true;
        }
    }
}
