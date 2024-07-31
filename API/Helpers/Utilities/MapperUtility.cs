using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Query;

namespace API.Helpers.Utilities
{
    public static partial class MapperUtility
    {
        private static TDestination MapConfiguration<TSource, TDestination>(TSource source) where TSource : class, new() where TDestination : class, new()
        {
            var inPropDict = typeof(TSource).GetProperties()
                .Where(p => p.CanRead)
                .ToDictionary(p => p.Name);

            var outProps = typeof(TDestination).GetProperties()
                .Where(p => p.CanWrite);

            return ExecuteMap<TDestination>(source, inPropDict, outProps);
        }

        private static TDestination MapConfiguration<TDestination>(this object source) where TDestination : class, new()
        {
            var inPropDict = source.GetType().GetProperties()
                .Where(p => p.CanRead)
                .ToDictionary(p => p.Name);

            var outProps = typeof(TDestination).GetProperties()
                .Where(p => p.CanWrite);

            return ExecuteMap<TDestination>(source, inPropDict, outProps);
        }

        private static TDestination MapAttributeConfiguration<TSource, TDestination>(TSource source) where TSource : class, new() where TDestination : class, new()
        {
            var attrType = typeof(MapperAttribute);
            var inPropDict = typeof(TSource).GetProperties()
                .Where(p => p.CanRead)
                .Select(p => new MapperItem
                {
                    Name = p.Name,
                    Property = p,
                    Attribute = (MapperAttribute)p.GetCustomAttribute(attrType, false)
                }).ToList();

            var outProps = typeof(TDestination).GetProperties()
                .Where(p => p.CanWrite)
                .Select(p => new MapperItem
                {
                    Name = p.Name,
                    Property = p,
                    Attribute = (MapperAttribute)p.GetCustomAttribute(attrType, false)
                }).ToList();

            return ExecuteMapAttribute<TDestination>(source, inPropDict, outProps);
        }

        private static TDestination MapAttributeConfiguration<TDestination>(this object source) where TDestination : class, new()
        {
            var attrType = typeof(MapperAttribute);
            var inPropDict = source.GetType().GetProperties()
                .Where(p => p.CanRead)
                .Select(p => new MapperItem
                {
                    Name = p.Name,
                    Property = p,
                    Attribute = (MapperAttribute)p.GetCustomAttribute(attrType, false)
                }).ToList();

            var outProps = typeof(TDestination).GetProperties()
                .Where(p => p.CanWrite)
                .Select(p => new MapperItem
                {
                    Name = p.Name,
                    Property = p,
                    Attribute = (MapperAttribute)p.GetCustomAttribute(attrType, false)
                }).ToList();

            return ExecuteMapAttribute<TDestination>(source, inPropDict, outProps);
        }

        private static TDestination ExecuteMap<TDestination>(dynamic source, Dictionary<string, PropertyInfo> inPropDict, IEnumerable<PropertyInfo> outProps) where TDestination : class, new()
        {
            TDestination destination = new();

            foreach (var outProp in outProps)
            {
                if (inPropDict.TryGetValue(outProp.Name, out var inProp))
                {
                    object sourceValue = inProp.GetValue(source);
                    if (inProp.PropertyType.Name.Contains("ICollection"))
                    {
                        ConvertEntitySource(sourceValue, outProp, destination);
                        continue;
                    }
                    else if (inProp.PropertyType != outProp.PropertyType)
                    {
                        sourceValue = Convert.ChangeType(sourceValue, outProp.PropertyType);
                    }
                    outProp.SetValue(destination, sourceValue);
                }
                else
                {
                    string outName = RemoveSpecialCharacter().Replace(outProp.Name, "");
                    string[] outNameSplit = SplitUpperCharacter().Split(outName);
                    string name = string.Join("", outNameSplit.Take(outNameSplit.Length - 1).ToArray());
                    string property = outNameSplit[^1];

                    if (inPropDict.TryGetValue(name, out var entity))
                    {
                        object entitySource = entity.GetValue(source);
                        var inPropEntity = entitySource.GetType().GetProperties()
                            .FirstOrDefault(p => p.CanRead && p.Name == property);

                        object sourceValue = inPropEntity?.GetValue(entitySource);
                        if (inPropEntity?.PropertyType != outProp.PropertyType)
                        {
                            sourceValue = Convert.ChangeType(sourceValue, outProp.PropertyType);
                        }
                        outProp.SetValue(destination, sourceValue);
                    }
                }
            }
            return destination;
        }

        private static TDestination ExecuteMapAttribute<TDestination>(dynamic source, List<MapperItem> inPropDict, List<MapperItem> outProps) where TDestination : class, new()
        {
            TDestination destination = new();

            foreach (var outProp in outProps)
            {
                var inProp = inPropDict.FirstOrDefault(
                        x => x.Name == outProp.Name
                          || (outProp.Attribute != null && x.Name == outProp.Attribute.Name)
                          || (x.Attribute != null && x.Attribute.Name == outProp.Name)
                          || (x.Attribute != null && outProp.Attribute != null && x.Attribute.Name == outProp.Attribute.Name));

                if (inProp is not null)
                {
                    object sourceValue = inProp.Property.GetValue(source);
                    if (inProp.Property.PropertyType.Name.Contains("ICollection"))
                    {
                        ConvertEntitySource(sourceValue, outProp.Property, destination);
                        continue;
                    }
                    else if (inProp.Property.PropertyType != outProp.Property.PropertyType)
                    {
                        sourceValue = Convert.ChangeType(sourceValue, outProp.Property.PropertyType);
                    }
                    outProp.Property.SetValue(destination, sourceValue);
                }
            }
            return destination;
        }

        private static void ConvertEntitySource<TDestination>(this object sources, PropertyInfo outPropInput, TDestination destination) where TDestination : class, new()
        {
            var baseClass = Type.GetType($"{outPropInput.ReflectedType.Namespace}.{outPropInput.Name}");

            var outProps = baseClass.GetProperties()
                .Where(p => p.CanWrite);

            var results = Activator.CreateInstance(outPropInput.PropertyType, false);

            foreach (object source in (dynamic)sources)
            {
                var inPropDict = source.GetType().GetProperties()
                    .Where(p => p.CanRead)
                    .ToDictionary(p => p.Name);

                var destinationBase = Activator.CreateInstance(baseClass, false);

                foreach (var outProp in outProps)
                {
                    if (inPropDict.TryGetValue(outProp.Name, out var inProp))
                    {
                        object sourceValue = inProp.GetValue(source);
                        if (inProp.PropertyType != outProp.PropertyType)
                        {
                            sourceValue = Convert.ChangeType(sourceValue, outProp.PropertyType);
                        }
                        outProp.SetValue(destinationBase, sourceValue);
                    }
                }

                var method = outPropInput.PropertyType.GetMethod("Add");
                method.Invoke(results, new object[] { destinationBase });
            }

            outPropInput.SetValue(destination, results);
        }

        public static TDestination MapOne<TSource, TDestination>(TSource source) where TSource : class, new() where TDestination : class, new()
        {
            return MapConfiguration<TSource, TDestination>(source);
        }

        public static TDestination MapOne<TDestination>(this object source) where TDestination : class, new()
        {
            return MapConfiguration<TDestination>((dynamic)source);
        }

        public static TDestination MapAttributeOne<TSource, TDestination>(TSource source) where TSource : class, new() where TDestination : class, new()
        {
            return MapAttributeConfiguration<TSource, TDestination>(source);
        }

        public static TDestination MapAttributeOne<TDestination>(this object source) where TDestination : class, new()
        {
            return MapAttributeConfiguration<TDestination>((dynamic)source);
        }

        public static IQueryable<TDestination> Map<TSource, TDestination>(IQueryable<TSource> sources) where TSource : class, new() where TDestination : class, new()
        {
            List<TDestination> destinations = new();
            foreach (var source in sources)
            {
                destinations.Add(MapConfiguration<TSource, TDestination>(source));
            }
            return destinations.AsAsyncQueryable();
        }

        public static IQueryable<TDestination> Map<TDestination>(this object sources) where TDestination : class, new()
        {
            List<TDestination> destinations = new();
            foreach (var source in (dynamic)sources)
            {
                destinations.Add(MapConfiguration<TDestination>(source));
            }
            return destinations.AsAsyncQueryable();
        }

        public static IQueryable<TDestination> MapAttribute<TSource, TDestination>(IQueryable<TSource> sources) where TSource : class, new() where TDestination : class, new()
        {
            List<TDestination> destinations = new();
            foreach (var source in sources)
            {
                destinations.Add(MapAttributeConfiguration<TSource, TDestination>(source));
            }
            return destinations.AsAsyncQueryable();
        }

        public static IQueryable<TDestination> MapAttribute<TDestination>(this object sources) where TDestination : class, new()
        {
            List<TDestination> destinations = new();
            foreach (var source in (dynamic)sources)
            {
                destinations.Add(MapAttributeConfiguration<TDestination>(source));
            }
            return destinations.AsAsyncQueryable();
        }

        public static List<TDestination> MapTo<TSource, TDestination>(List<TSource> sources) where TSource : class, new() where TDestination : class, new()
        {
            List<TDestination> destinations = new();
            foreach (var source in sources)
            {
                destinations.Add(MapConfiguration<TSource, TDestination>(source));
            }
            return destinations;
        }

        public static List<TDestination> MapTo<TDestination>(this object sources) where TDestination : class, new()
        {
            List<TDestination> destinations = new();
            foreach (var source in (dynamic)sources)
            {
                destinations.Add(MapConfiguration<TDestination>(source));
            }
            return destinations;
        }

        public static List<TDestination> MapAttributeTo<TSource, TDestination>(List<TSource> sources) where TSource : class, new() where TDestination : class, new()
        {
            List<TDestination> destinations = new();
            foreach (var source in sources)
            {
                destinations.Add(MapAttributeConfiguration<TSource, TDestination>(source));
            }
            return destinations;
        }

        public static List<TDestination> MapAttributeTo<TDestination>(this object sources) where TDestination : class, new()
        {
            List<TDestination> destinations = new();
            foreach (var source in (dynamic)sources)
            {
                destinations.Add(MapAttributeConfiguration<TDestination>(source));
            }
            return destinations;
        }

        [GeneratedRegex("[^a-zA-Z0-9]")]
        private static partial Regex RemoveSpecialCharacter();
        [GeneratedRegex("(?<!^)(?=[A-Z])")]
        private static partial Regex SplitUpperCharacter();
    }

    public static class AsyncQueryable
    {
        /// <summary>
        /// Returns the input typed as IQueryable that can be queried asynchronously
        /// </summary>
        /// <typeparam name="TEntity">The item type</typeparam>
        /// <param name="source">The input</param>
        public static IQueryable<TEntity> AsAsyncQueryable<TEntity>(this IEnumerable<TEntity> source)
            => new AsyncQueryable<TEntity>(source ?? throw new ArgumentNullException(nameof(source)));
    }

    public class AsyncQueryable<TEntity> : EnumerableQuery<TEntity>, IAsyncEnumerable<TEntity>, IQueryable<TEntity>
    {
        public AsyncQueryable(IEnumerable<TEntity> enumerable) : base(enumerable) { }
        public AsyncQueryable(Expression expression) : base(expression) { }
        public IAsyncEnumerator<TEntity> GetEnumerator() => new AsyncEnumerator(this.AsEnumerable().GetEnumerator());
        public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new AsyncEnumerator(this.AsEnumerable().GetEnumerator());
        IQueryProvider IQueryable.Provider => new AsyncQueryProvider(this);

        class AsyncEnumerator : IAsyncEnumerator<TEntity>
        {
            private readonly IEnumerator<TEntity> inner;
            public AsyncEnumerator(IEnumerator<TEntity> inner) => this.inner = inner;
            public void Dispose() => inner.Dispose();
            public TEntity Current => inner.Current;
            public ValueTask<bool> MoveNextAsync() => new(inner.MoveNext());
#pragma warning disable CS1998 // Nothing to await
            public async ValueTask DisposeAsync() => inner.Dispose();
#pragma warning restore CS1998
        }

        class AsyncQueryProvider : IAsyncQueryProvider
        {
            private readonly IQueryProvider inner;
            internal AsyncQueryProvider(IQueryProvider inner) => this.inner = inner;
            public IQueryable CreateQuery(Expression expression) => new AsyncQueryable<TEntity>(expression);
            public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new AsyncQueryable<TElement>(expression);
            public object Execute(Expression expression) => inner.Execute(expression);
            public TResult Execute<TResult>(Expression expression) => inner.Execute<TResult>(expression);
            public static IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression) => new AsyncQueryable<TResult>(expression);
            TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
            {
                // TResult is Task<...> here!
                var genericArg = typeof(TResult).GetGenericArguments()[0];
                return (TResult)typeof(Task)
                    .GetMethod(nameof(Task.FromResult), BindingFlags.Static | BindingFlags.Public)!
                    .MakeGenericMethod(genericArg)
                    .Invoke(null, new[] { Execute(expression) })!;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MapperAttribute : Attribute
    {
        readonly string _name;

        public MapperAttribute(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }
    }

    public class MapperItem
    {
        public string Name { get; set; }
        public PropertyInfo Property { get; set; }
        public MapperAttribute Attribute { get; set; }
    }
}