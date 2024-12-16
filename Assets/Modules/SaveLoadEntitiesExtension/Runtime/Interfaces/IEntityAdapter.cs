namespace SaveLoadEntitiesExtension
{
    public interface IEntityAdapter
    {
        int GetId();
        string GetName();
        string GetEntityType();
        (float x, float y, float z) GetPosition();
        (float x, float y, float z) GetRotation();
        void SetPosition(float x, float y, float z);
        void SetRotation(float x, float y, float z);
        IComponentAdapter[] GetComponents();
    }
}