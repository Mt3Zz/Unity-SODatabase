namespace SODatabase.DataObject
{
    internal interface ISaver
    {
        void Save(BaseObject baseObject);
        void Load(BaseObject baseObject);
    }
}
