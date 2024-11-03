using UnityEngine;

namespace SODatabase
{
    using FloatObject = DataObject.FloatObject;


    internal sealed class FloatObjectController : MonoBehaviour, DataObject.IObjectController<FloatObject>
    {
        [SerializeField]
        private FloatObject _target = default;


        public float NewValue
        {
            get => _newValue;
            set => _newValue = value;
        }
        [SerializeField]
        private float _newValue = default;


        public bool UpdateTarget()
        {
            return UpdateObject(_target);
        }
        public bool UpdateObject(FloatObject obj)
        {
            if (obj == null) return false;

            obj.Value = NewValue;
            return true;
        }
    }
}
