using System.Collections.Generic;
using UnityEngine;

namespace SODatabase.DataObject
{
    [CreateAssetMenu(fileName ="ObjectStorage", menuName ="SODatabase/ObjectStorage", order = 1)]
    public class ObjectStorage : ScriptableObject
    {
        public IList<BaseObject> Objects => _objects;
        [SerializeField]
        private List<BaseObject> _objects = new();
    }
}
