using UnityEngine;

namespace SODatabase
{
    public sealed class StringObject : DataObject.BaseObject
    {
        public string Value
        {
            get => _value;
            set => _value = value;
        }
        [SerializeField]
        private string _value = "";


        public void Append(string suffix)
        {
            _value += suffix;
        }
    }
}
