namespace Modules.ComponentSerialization.Runtime
{
    public interface ITypeSerializer
    {
        object Serialize(object value);
        object Deserialize(object dto);
    }
    
    public interface ITypeSerializer<TSource, TDto> : ITypeSerializer
    {
        TDto Serialize(TSource value);
        TSource Deserialize(TDto dto);
        
        object ITypeSerializer.Serialize(object value) => Serialize((TSource)value);
        object ITypeSerializer.Deserialize(object dto) => Deserialize((TDto)dto);
    }
}