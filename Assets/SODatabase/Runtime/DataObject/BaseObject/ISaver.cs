namespace SODatabase.DataObject
{
    public interface ISaver
    {
        void Save(BaseObject baseObject);
        void Load(BaseObject baseObject);
    }
}
