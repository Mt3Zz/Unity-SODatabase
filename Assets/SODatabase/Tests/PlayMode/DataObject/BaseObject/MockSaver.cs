using UnityEngine;

namespace SODatabase.Tests.PlayMode
{
    internal class MockSaver : ISaver
    {
        public bool IsSaved { get; private set; } = false;
        public bool IsLoaded { get; private set; } = false;


        public void Save(BaseObject baseObject)
        {
            IsSaved = true;
        }

        public void Load(BaseObject baseObject)
        {
            IsLoaded = true;
        }
    }
}
