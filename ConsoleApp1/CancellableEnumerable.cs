using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class CancellableEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _enumerable;
        public bool IsCancelled { get; set; } = false;

        public CancellableEnumerable(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }
    }
}