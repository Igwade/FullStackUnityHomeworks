namespace SaveLoadEntitiesExtension
{
    public interface IWorldAdapter
    {
        IEntityAdapter SpawnEntity(string entityName, float x, float y, float z, float rx, float ry, float rz, int id = -1);
        IEntityAdapter[] GetAllEntities();
        void DestroyAllEntities();
    }
}