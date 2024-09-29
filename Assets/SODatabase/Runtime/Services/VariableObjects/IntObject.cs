using UnityEngine;

namespace SODatabase.DataObject
{
    public sealed class IntObject : BaseObject
    {
        public int Value => _value;
        [SerializeField]
        private int _value = 0;
    }
}
