using UnityEngine;

namespace SODatabase.DataObject
{
    public class FloatObject : BaseObject
    {
        public float Value => _value;
        [SerializeField]
        private float _value = 0;
    }
}
