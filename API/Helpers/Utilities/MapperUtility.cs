using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;

namespace API.Helpers.Utilities
{
    public static class MapperUtility
    {
        private static TDestination MapConfiguration<TSource, TDestination>(TSource source) where TSource : class, new() where TDestination : class, new()
        {
            var inPropDict = typeof(TSource).GetProperties()
                .Where(p => p.CanRead)
                .ToDictionary(p => p.Name);

            var outProps = typeof(TDestination).GetProperties()
                .Where(p => p.CanWrite);

            TDestination destination = new();

            foreach (var outProp in outProps)
            {
                if (inPropDict.TryGetValue(outProp.Name, out var inProp))
                {
                    object sourceValue = inProp.GetValue(source);
                    if (inProp.PropertyType != outProp.PropertyType)
                    {
                        sourceValue = Convert.ChangeType(sourceValue, outProp.PropertyType);
                    }
                    outProp.SetValue(destination, sourceValue);
                }
            }
            return destination;
        }

        private static TDestination MapConfiguration<TDestination>(this object source) where TDestination : class, new()
        {
            var inPropDict = source.GetType().GetProperties()
                .Where(p => p.CanRead)
                .ToDictionary(p => p.Name);

            var outProps = typeof(TDestination).GetProperties()
                .Where(p => p.CanWrite);

            TDestination destination = new();

            foreach (var outProp in outProps)
            {
                if (inPropDict.TryGetValue(outProp.Name, out var inProp))
                {
                    object sourceValue = inProp.GetValue(source);
                    if (inProp.PropertyType != outProp.PropertyType)
                    {
                        sourceValue = Convert.ChangeType(sourceValue, outProp.PropertyType);
                    }
                    outProp.SetValue(destination, sourceValue);
                }
            }
            return destination;
        }

        public static TDestination MapOne<TSource, TDestination>(TSource source) where TSource : class, new() where TDestination : class, new()
        {
            return MapConfiguration<TSource, TDestination>(source);
        }

        public static TDestination MapOne<TDestination>(this object source) where TDestination : class, new()
        {
            return MapConfiguration<TDestination>((dynamic)source);
        }

        public static IQueryable<TDestination> Map<TSource, TDestination>(IQueryable<TSource> sources) where TSource : class, new() where TDestination : class, new()
        {
            List<TDestination> destinations = new();
            foreach (var source in sources)
            {
                destinations.Add(MapConfiguration<TSource, TDestination>(source));
            }
            return destinations.AsQueryable();
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
}