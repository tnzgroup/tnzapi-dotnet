namespace TNZAPI.NET.Helpers
{
    public class Mapper
    {
        /// <summary>
        /// Maps one object to another
        /// </summary>
        /// <typeparam name="TDestination">Destination object type</typeparam>
        /// <param name="source">Source object</param>
        /// <returns>TDestination</returns>
        public static TDestination Map<TDestination>(object source)
        {
            var destination = Activator.CreateInstance<TDestination>();
            var sourceType = source.GetType();
            var destinationType = typeof(TDestination);

            foreach (var sourceProperty in sourceType.GetProperties())
            {
                var destinationProperty = destinationType.GetProperty(sourceProperty.Name);
                if (destinationProperty != null && destinationProperty.CanWrite)
                {
                    var value = sourceProperty.GetValue(source);
                    if (value != null)
                    {
                        destinationProperty.SetValue(destination, value);
                    }
                }
            }

            return destination;
        }

        /// <summary>
        /// Update source object from none-empty properties
        /// </summary>
        /// <typeparam name="TSource">Source oject type</typeparam>
        /// <typeparam name="TDest">Dest object type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="dest">Dest object</param>
        /// <returns>Source object</returns>
        public static TSource Map<TSource, TDest>(TSource source, TDest dest)
        {
            foreach (var sourceProperty in source.GetType().GetProperties())
            {
                foreach (var destProperty in dest.GetType().GetProperties())
                {
                    if (sourceProperty.Name == destProperty.Name)
                    {
                        if (destProperty.PropertyType == typeof(DateTime?) && destProperty.GetValue(dest) == null)
                        {
                            continue;
                        }
                        if (destProperty.PropertyType == typeof(int?) && destProperty.GetValue(dest) == null)
                        {
                            continue;
                        }
                        if (sourceProperty.GetValue(source) == null && destProperty.GetValue(dest) == null)
                        {
                            continue;
                        }

                        sourceProperty.SetValue(source, destProperty.GetValue(dest));
                    }
                }
            }

            return source;
        }

        /// <summary>
        /// Update source object from none-empty properties
        /// </summary>
        /// <typeparam name="TSource">Source oject type</typeparam>
        /// <typeparam name="TDest">Dest object type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="dest">Dest object</param>
        /// <returns>Source object</returns>
        public static TSource Update<TSource, TDest>(TSource source, TDest dest)
        {
            foreach (var sourceProperty in source.GetType().GetProperties())
            {
                foreach (var destProperty in dest.GetType().GetProperties())
                {
                    if (sourceProperty.Name == destProperty.Name)
                    {
                        if (destProperty.GetValue(dest) is not null)
                        {
                            sourceProperty.SetValue(source, destProperty.GetValue(dest));
                        }
                    }
                }
            }

            return source;
        }
    }
}
