using UnityEngine;

namespace SODatabase.DataObject
{
    public class IntObject : BaseObject
    {
        public int Value => _value;
        [SerializeField]
        private int _value = 0;
    }
}
