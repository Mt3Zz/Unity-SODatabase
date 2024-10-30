using UnityEngine;

namespace SODatabase
{
    using StringObject = DataObject.StringObject;


    internal sealed class StringObjectController : MonoBehaviour, DataObject.IObjectController<StringObject>
    {
        [SerializeField]
        private StringObject _target = default;


        public string NewValue 
        { 
            get => _newValue; 
            set => _newValue = value; 
        }
        [SerializeField]
        private string _newValue = "";


        public bool UpdateTarget()
        {
            return UpdateObject(_target);
        }
        public bool UpdateObject(StringObject obj)
        {
            if (obj == null) return false;

            obj.Value = NewValue;
            return true;
        }
    }
}