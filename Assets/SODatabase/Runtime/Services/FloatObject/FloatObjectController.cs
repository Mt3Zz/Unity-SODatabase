using System;
using UnityEngine;

namespace SODatabase
{
    [Serializable]
    public sealed class FloatObjectController : DataObject.IObjectController<FloatObject>
    {
        public FloatObjectController(float newValue)
        {
            _newValue = newValue;
        }


        public float NewValue
        {
            get => _newValue;
            set => _newValue = value;
        }
        [SerializeField]
        private float _newValue = default;


        public bool UpdateObject(FloatObject obj)
        {
            if (obj == null) return false;

            obj.Value = NewValue;
            return true;
        }
    }
}
