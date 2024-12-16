namespace SaveLoadEntitiesExtension
{
    public interface IWorld
    {
        IEntity SpawnEntity(string entityName, float x, float y, float z, float rx, float ry, float rz, int id = -1);
        IEntity[] GetAllEntities();
        void DestroyAllEntities();
    }
}