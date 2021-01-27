
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BST
{
    [Serializable]
    public class BinaryTree<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private Node _root;

        public TValue this[TKey key]
        {
            get
            {
                var node = Search(_root, key);
                if (node == null)
                    throw new KeyNotFoundException($"The given key '{key}' not exists");
                return node.Data.Value;
            }
            set
            {
                var node = Search(_root, key);
                if (node == null)
                    throw new KeyNotFoundException($"The given key '{key}' not exists");
                node.Data = new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        public ICollection<TKey> Keys => this.Select(x => x.Key).ToList();
        public ICollection<TValue> Values => this.Select(x => x.Value).ToList();
        public int Count => Keys.Count;
        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            var item = new KeyValuePair<TKey, TValue>(key, value);
            var node = new Node() { Data = item };

            if (_root == null)
            {
                _root = node;
                return;
            }

            Insert(_root, node);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        private Node Insert(Node parent, Node node)
        {
            if (parent == null)
                return node;

            if (Comparer<TKey>.Default.Compare(node.Data.Key, parent.Data.Key) == -1)
            {
                parent.Left = Insert(parent.Left, node);
                parent.Left.Parent = parent;
            }
            else if (Comparer<TKey>.Default.Compare(node.Data.Key, parent.Data.Key) == 1)
            {
                parent.Right = Insert(parent.Right, node);
                parent.Right.Parent = parent;
            }
            else
                throw new InsertDuplicateKeyException(node.Data.Key.ToString());

            return parent;
        }

        public void Clear()
        {
            _root = null;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var searchedNode = Search(_root, item.Key);
            return searchedNode != null && Comparer<TValue>.Default.Compare(searchedNode.Data.Value, item.Value) == 0;
        }

        public bool ContainsKey(TKey key)
        {
            return Search(_root, key) != null;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException($"Array is null");

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException($"Index is less than zero");

            if (Keys.Count > (array.Length - arrayIndex))
                throw new ArgumentException("The number of elements in the source collection is greater than " +
                                             "the available space from index to the end of the destination array");

            foreach (var keyValuePair in this)
            {
                array[arrayIndex] = keyValuePair;
                arrayIndex++;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            var node = Search(_root, key);
            if (node == null)
                return false;

            if (node.Left == null && node.Right == null)
                SetDeletedNodeChildToNewParent(node.Parent, node, null);

            if (node.Left == null && node.Right != null)
                SetDeletedNodeChildToNewParent(node.Parent, node, node.Right);
            if (node.Left != null & node.Right == null)
                SetDeletedNodeChildToNewParent(node.Parent, node, node.Left);

            if (node.Left != null && node.Right != null)
            {
                var successor = Next(key);
                node.Data = successor.Data;

                SetDeletedNodeChildToNewParent(successor.Parent, successor, successor.Right);
                SetDeletedNodeChildToNewParent(successor.Parent, successor, successor.Left);
            }

            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = Search(_root, key);
            value = node == null ? default(TValue) : this[key];
            return node != null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void PreOrderTraversal(List<KeyValuePair<TKey, TValue>> list, Node node)
        {
            if (node == null)
                return;

            list.Add(new KeyValuePair<TKey, TValue>(node.Data.Key, node.Data.Value));
            PreOrderTraversal(list, node.Left);
            PreOrderTraversal(list, node.Right);
        }

        private Node Search(Node parent, TKey searchKey)
        {
            if (parent == null || Comparer<TKey>.Default.Compare(searchKey, parent.Data.Key) == 0)
                return parent;

            if (Comparer<TKey>.Default.Compare(searchKey, parent.Data.Key) == -1)
                return Search(parent.Left, searchKey);
            else
                return Search(parent.Right, searchKey);
        }

        private Node Next(TKey key)
        {
            Node current = _root;
            Node successor = null;

            while (current != null)
            {
                if (Comparer<TKey>.Default.Compare(current.Data.Key, key) > 0)
                {
                    successor = current;
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }

            return successor;
        }

        private void SetDeletedNodeChildToNewParent(Node parent, Node deleted, Node child)
        {
            if (parent?.Left == deleted)
                parent.Left = child;
            if (parent?.Right == deleted)
                parent.Right = child;

            if (child != null && deleted != null)
                child.Parent = deleted.Parent;
        }



        [Serializable]
        private class Node
        {
            public KeyValuePair<TKey, TValue> Data { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public Node Parent { get; set; }
        }
    }
}
