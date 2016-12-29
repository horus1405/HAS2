using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HAS2.Core.Localization
{
    public class JsNamespaceCollection : ICollection<JsNamespace>
    {
        private readonly List<JsNamespace> _namespaces;

        public JsNamespaceCollection()
        {
            _namespaces= new List<JsNamespace>();
        }

        public JsNamespaceCollection(IEnumerable<JsNamespace> namespaces)
        {
            _namespaces = namespaces.ToList();
        }

        public IEnumerator<JsNamespace> GetEnumerator()
        {
            return _namespaces.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(JsNamespace item)
        {
            _namespaces.Add(item);
        }

        public void Clear()
        {
           _namespaces.Clear();
        }

        public bool Contains(JsNamespace item)
        {
            return _namespaces.Contains(item);
        }

        public void CopyTo(JsNamespace[] array, int arrayIndex)
        {
            _namespaces.CopyTo(array);
        }

        public bool Remove(JsNamespace item)
        {
           return _namespaces.Remove(item);
        }

        public int Count
        {
            get { return _namespaces.Count(); }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public string TryBeginNamespace(int currentIndex)
        {
            var currentNamespace = _namespaces.FirstOrDefault(x => x.BeginRange == currentIndex);
            return currentNamespace != null ? 
                currentNamespace.Namespace : 
                string.Empty;
        }

        public string TryEndNamespace(int currentIndex)
        {
            var currentNamespace = _namespaces.FirstOrDefault(x => x.EndRange == currentIndex);
            return currentNamespace != null ?
                currentNamespace.Namespace :
                string.Empty;
        }

    }
}