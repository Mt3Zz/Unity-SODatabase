using UnityEngine;


namespace SODatabase.DataObject
{
    [CreateAssetMenu(fileName = "IntObject", menuName = "SODatabase/IntObject")]
    public class IntObject : BaseObject
    {
        public int Value => _value;
        [SerializeField]
        private int _value = 0;

    }
}
