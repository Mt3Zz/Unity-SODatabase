using System;
using UnityEngine;

namespace SODatabase
{
    [Serializable]
    public sealed class IntObjectController : DataObject.IObjectController<IntObject>
    {
        public IntObjectController(int newValue)
        {
            _newValue = newValue;
        }


        public int NewValue
        {
            get => _newValue;
            set => _newValue = value;
        }
        [SerializeField]
        private int _newValue = default;


        public bool UpdateObject(IntObject obj)
        {
            if (obj == null) return false;

            obj.Value = NewValue;
            return true;
        }
    }
}
