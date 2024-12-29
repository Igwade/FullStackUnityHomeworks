using App.SaveLoad.Entities.ComponentSerializers;

namespace App.SaveLoad.Entities
{
    // Просто тег для выборки
    public interface ISerializableComponent
    {
    }
    
    // Просто чтобы было видно сериализатор компонента из самого компонента
    // При изменение компонента можно быстро перейти к его сериализатору и поправить его при надобности
    public interface ISerializableComponent<TSerializer> : ISerializableComponent where TSerializer: IComponentSerializer
    {
    }
}