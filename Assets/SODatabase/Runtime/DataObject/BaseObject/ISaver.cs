using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SODatabase
{
    internal interface ISaver
    {
        void Save(BaseObject baseObject);
        void Load(BaseObject baseObject);
    }
}
