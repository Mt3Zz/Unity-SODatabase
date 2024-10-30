using UnityEngine;

namespace SODatabase
{
    using IntObject = DataObject.IntObject;

    internal sealed class IntObjectController : MonoBehaviour, DataObject.IObjectController<IntObject>
    {
        [SerializeField]
        private IntObject _target = default;


        public int NewValue
        {
            get => _newValue;
            set => _newValue = value;
        }
        [SerializeField]
        private int _newValue = default;


        public bool UpdateTarget()
        {
            return UpdateObject(_target);
        }
        public bool UpdateObject(IntObject obj)
        {
            if (obj == null) return false;

            obj.Value = NewValue;
            return true;
        }
    }
}
