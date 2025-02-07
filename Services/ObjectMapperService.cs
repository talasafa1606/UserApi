using UserApi.Exceptions;
namespace UserApi.Services
{
    public class ObjectMapperService : IObjectMapperService
    {
        private readonly ILogger<ObjectMapperService> _logger;

        public ObjectMapperService(ILogger<ObjectMapperService> logger)
        {
            _logger = logger;
        }

        public TDestination Map<TSource, TDestination>(TSource source) where TDestination : new()
        {
            try
            {
                if (source == null)
                    throw UserApiException.NotFound(nameof(source));

                var destination = new TDestination();
                var sourceProps = typeof(TSource).GetProperties();
                var destProps = typeof(TDestination).GetProperties();

                foreach (var sourceProp in sourceProps)
                {
                    var destProp = destProps.FirstOrDefault(p =>
                        p.Name.Equals(sourceProp.Name, StringComparison.OrdinalIgnoreCase) &&
                        p.CanWrite);

                    if (destProp != null)
                    {
                        var value = sourceProp.GetValue(source);
                        if (value != null)
                        {
                            if (destProp.PropertyType == sourceProp.PropertyType)
                            {
                                destProp.SetValue(destination, value);
                            }
                            else
                            {
                                var convertedValue = Convert.ChangeType(value, destProp.PropertyType);
                                destProp.SetValue(destination, convertedValue);
                            }
                        }
                    }
                }

                return destination;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping object from {SourceType} to {DestType}",
                    typeof(TSource).Name, typeof(TDestination).Name);
                throw UserApiException.InternalServerError($"Error mapping object: {ex.Message}");
            }
        }
    }
}

