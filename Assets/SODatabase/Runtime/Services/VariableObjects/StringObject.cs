using UnityEngine;

namespace SODatabase.DataObject
{
    public sealed class StringObject : BaseObject
    {
        public string Value
        {
            get { return _value; }
            internal set { _value = value; }
        }
        [SerializeField]
        private string _value = "";
    }
}
