using App.SaveLoad.Entities.ComponentSerializers;

namespace App.SaveLoad.Entities
{
    // Просто тег для выборки и чтобы не забывать что компонент сериализуется
    public interface ISerializableComponent
    {
    }
    
    // Чтобы было видно сериализатор компонента из самого компонента
    // При изменение компонента можно быстро перейти к его сериализатору и поправить его при надобности.
    public interface ISerializableComponent<TSerializer> : ISerializableComponent where TSerializer: IComponentSerializer
    {
    }
}