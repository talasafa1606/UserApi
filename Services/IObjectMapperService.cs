public interface IObjectMapperService
{
    TDestination Map<TSource, TDestination>(TSource source) where TDestination : new();
}
