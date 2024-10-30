using UnityEngine;

namespace SODatabase
{
    using BoolObject = DataObject.BoolObject;


    internal sealed class BoolObjectController : MonoBehaviour, DataObject.IObjectController<BoolObject>
    {
        [SerializeField]
        private BoolObject _target = default;


        public bool NewValue
        {
            get => _newValue;
            set => _newValue = value;
        }
        [SerializeField]
        private bool _newValue = false;


        public bool UpdateTarget()
        {
            return UpdateObject(_target);
        }
        public bool UpdateObject(BoolObject obj)
        {
            if (obj == null) return false;

            obj.Value = NewValue;
            return true;
        }
    }
}
