using UnityEngine;

namespace SODatabase.DataObject
{
    public sealed class StringObject : BaseObject
    {
        public string Value => _value;
        [SerializeField]
        private string _value = "";
    }
}
