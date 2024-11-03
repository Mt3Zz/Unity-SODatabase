using System;
using UnityEngine;

namespace SODatabase
{
    [Serializable]
    public sealed class StringObjectController : DataObject.IObjectController<StringObject>
    {
        public StringObjectController(string newValue)
        {
            _newValue = newValue;
        }


        public string NewValue 
        { 
            get => _newValue; 
            set => _newValue = value; 
        }
        [SerializeField]
        private string _newValue = "";


        public bool UpdateObject(StringObject obj)
        {
            if (obj == null) return false;

            obj.Value = NewValue;
            return true;
        }
    }
}