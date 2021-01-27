using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BST;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.IO;

namespace UnitTestBST
{
    [TestClass]
    public class UnitTest1
    {
        /* Тестируемое бинарное дерево поиска
                8
               / \
              3   10
             / \    \
            1   6    14
               / \   /
              4   7 13
         */

        readonly BinaryTree<int, int> tree =
            new BinaryTree<int, int>
        {
            {8, 8},
            {3, 3},
            {10, 10},
            {1, 1},
            {6, 6},
            {14, 14},
            {4, 4},
            {7, 7},
            {13, 13},
        };

        private List<int> GetActual()
        {
            var actual = new List<int>();
            var enumerator = tree.GetEnumerator();
            while (enumerator.MoveNext())
                actual.Add(enumerator.Current.Key);
            return actual;
        }

        [TestMethod]
        public void PreOrderTraversal()
        {
            var expected = new List<int>() { 8, 3, 1, 6, 4, 7, 10, 14, 13 };
            var actual = GetActual();
            Assert.Equals(expected, actual);
        }

        [TestMethod]
        public void RemoveOneLeaf()
        {
            tree.Remove(13);

            var expected = new List<int>() { 8, 3, 1, 6, 4, 7, 10, 14 };
            var actual = GetActual();
            Assert.Equals(expected, actual);
        }

        [TestMethod]
        public void RemoveNodeWithOneSubNode()
        {
            tree.Remove(14);

            var expected = new List<int>() { 8, 3, 1, 6, 4, 7, 10, 13 };
            var actual = GetActual();
            Assert.Equals(expected, actual);
        }

        [TestMethod]
        public void RemoveNodeWithTwoSubNode()
        {
            tree.Remove(3);

            var expected = new List<int>() { 8, 4, 1, 6, 7, 10, 14, 13 };
            var actual = GetActual();
            Assert.Equals(expected, actual);
        }

        [TestMethod]
        public void RemoveRootNode()
        {
            tree.Remove(8);

            var expected = new List<int>() { 10, 3, 1, 6, 4, 7, 14, 13 };
            var actual = GetActual();
            Assert.Equals(expected, actual);
        }

        [TestMethod]
        public void TryGetKeys()
        {
            var treeTest = new BinaryTree<int, int>()
            {
                {1, 2},
                {3, 4},
            };

            Assert.Equals(new List<int>() { 1, 3 }, treeTest.Keys);
        }

        [TestMethod]
        public void TryGetValues()
        {
            var treeTest = new BinaryTree<int, int>()
            {
                {1, 2},
                {3, 4},
            };

            Assert.Equals(new List<int>() { 2, 4 }, treeTest.Values);
        }

        [TestMethod]
        public void TryClearTree()
        {
            var treeTest = new BinaryTree<int, int>()
            {
                {1, 2},
                {3, 4},
            };

            treeTest.Clear();
            Assert.Equals(0, treeTest.Keys.Count);
        }

        [TestMethod]
        public void TryAddExistsKeyInTree()
        {
            var treeTest = new BinaryTree<int, int>() { { 1, 1 } };
            InsertDuplicateKeyException insertDuplicateKeyException = Assert.ThrowsException<InsertDuplicateKeyException>(() => treeTest.Add(1, 2));
            Assert.IsTrue(treeTest.Keys.Count == 1);
        }

        [TestMethod]
        public void TryContainsKeyIsTrue()
        {
            Assert.IsTrue(tree.ContainsKey(7));
        }

        [TestMethod]
        public void TryContainsKeyIsFalse()
        {
            Assert.IsFalse(tree.ContainsKey(999));
        }

        [TestMethod]
        public void TryContainsKeyValueIsTrue()
        {
            Assert.IsTrue(tree.Contains(new KeyValuePair<int, int>(7, 7)));
        }

        [TestMethod]
        public void TryContainsKeyValueIsFalse()
        {
            Assert.IsFalse(tree.Contains(new KeyValuePair<int, int>(7, 777)));
        }

        [TestMethod]
        public void TryGetValueToNodeExistsKey()
        {
            var treeTest = new BinaryTree<int, int>() { { 1, 12 } };
            Assert.Equals(12, treeTest[1]);
        }

        [TestMethod]
        public void TrySetValueToNodeExistsKey()
        {
            var treeTest = new BinaryTree<int, int>() { { 1, 12 } };
            treeTest[1] = 6;
            Assert.Equals(6, treeTest[1]);
        }

        [TestMethod]
        public void TryGetValueToNodeNotExistsKey()
        {
            Assert.ThrowsException<KeyNotFoundException>(() => new BinaryTree<int, int>()[2]);
        }

        [TestMethod]
        public void TrySetValueToNodeNotExistsKey()
        {
            var treeTest = new BinaryTree<int, int>();
            Assert.ThrowsException<KeyNotFoundException>(() => treeTest[2] = 6);
        }

        [TestMethod]
        public void TryGetValueMethodIsTrue()
        {
            var treeTest = new BinaryTree<int, int>() { { 1, 12 } };
            int value;
            var isTrySuccess = treeTest.TryGetValue(1, out value);
            Assert.IsTrue(isTrySuccess && value == 12);
        }

        [TestMethod]
        public void TryGetValueMethodIsFalse()
        {
            var treeTest = new BinaryTree<int, int>();
            int value = 1;
            var isTrySuccess = treeTest.TryGetValue(1, out value);
            Assert.IsTrue(!isTrySuccess && value == 0);
        }

        [TestMethod]
        public void TryTreeCopyToSuccess()
        {
            var expected = GetActual();
            var actual = new KeyValuePair<int, int>[tree.Count];
            tree.CopyTo(actual, 0);
            Assert.Equals(expected.Count, actual.Length);
        }

        [TestMethod]
        public void TryTreeCopyToFail()
        {
            var arr = new KeyValuePair<int, int>[8];
            Assert.ThrowsException<ArgumentException>(() => tree.CopyTo(arr, 0));
        }

        [TestMethod]
        public void TrySerializeAndDeserializeSuccess()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(Directory.GetCurrentDirectory());
            string path = Path.Combine(Directory.GetCurrentDirectory(), "binary_search_tree.dat");

            var serializer = new BinarySearchTreePersistable<int, int>(fileSystem);
            serializer.Serialize(path, tree);

            Assert.IsTrue(fileSystem.FileExists(path));

            var treeDeserialized = (BinaryTree<int, int>)serializer.Deserialize(path);
            Assert.IsTrue(treeDeserialized.ContainsKey(1) &&
                        treeDeserialized.ContainsKey(3) &&
                        treeDeserialized.Count == 9);

            var expected = new List<int>() { 8, 3, 1, 6, 4, 7, 10, 14, 13 };
            var actual = new List<int>();
            var enumerator = treeDeserialized.GetEnumerator();
            while (enumerator.MoveNext())
                actual.Add(enumerator.Current.Key);
            Assert.Equals(expected, actual);
        }

        [TestMethod]
        public void TrySerializeWithReplaceFileAndDeserializeSuccess()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(Directory.GetCurrentDirectory());
            string path = Path.Combine(Directory.GetCurrentDirectory(), "binary_search_tree.dat");

            var serializer = new BinarySearchTreePersistable<int, int>(fileSystem);
            serializer.Serialize(path, tree);
            Assert.IsTrue(fileSystem.FileExists(path));

            serializer.Serialize(path, new BinaryTree<int, int>() { { 1, 1 } });
            Assert.IsTrue(fileSystem.FileExists(path));

            var treeDeserialized = (BinaryTree<int, int>)serializer.Deserialize(path);
            Assert.IsTrue(treeDeserialized.ContainsKey(1) &&
                        treeDeserialized.Count == 1);

            var expected = new List<int>() { 1 };
            var actual = new List<int>();
            var enumerator = treeDeserialized.GetEnumerator();
            while (enumerator.MoveNext())
                actual.Add(enumerator.Current.Key);
            Assert.Equals(expected, actual);
        }

        [TestMethod]
        public void TrySerializeAndDeserializeEmptyTree()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(Directory.GetCurrentDirectory());
            string path = Path.Combine(Directory.GetCurrentDirectory(), "binary_search_tree.dat");

            var serializer = new BinarySearchTreePersistable<int, int>(fileSystem);
            serializer.Serialize(path, new BinaryTree<int, int>());

            Assert.IsTrue(fileSystem.FileExists(path));

            var treeDeserialized = (BinaryTree<int, int>)serializer.Deserialize(path);
            Assert.IsTrue(treeDeserialized.Count == 0);
        }

        [TestMethod]
        public void TrySerializeNullTree()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(Directory.GetCurrentDirectory());
            string path = Path.Combine(Directory.GetCurrentDirectory(), "binary_search_tree.dat");

            var serializer = new BinarySearchTreePersistable<int, int>(fileSystem);
            Assert.ThrowsException<ArgumentNullException>(() => serializer.Serialize(path, null));
        }

        [TestMethod]
        public void TrySerializeToNotExistsDirectory()
        {
            var fileSystem = new MockFileSystem();
            var serializer = new BinarySearchTreePersistable<int, int>(fileSystem);
            Assert.ThrowsException<DirectoryNotFoundException>(() => serializer.Serialize("binary_search_tree.dat", tree));
        }

        [TestMethod]
        public void TryDeserializeFromNotExistsDirectory()
        {
            var fileSystem = new MockFileSystem();
            var serializer = new BinarySearchTreePersistable<int, int>(fileSystem);
            Assert.ThrowsException<DirectoryNotFoundException>(() => serializer.Deserialize("binary_search_tree.dat"));
        }

        [TestMethod]
        public void TryDeserializeFromNotExistsFile()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(Directory.GetCurrentDirectory());
            string pathNotExists = Path.Combine(Directory.GetCurrentDirectory(), "file_not_exists.dat");

            var serializer = new BinarySearchTreePersistable<int, int>(fileSystem);
            Assert.ThrowsException<FileNotFoundException>(() => serializer.Deserialize(pathNotExists));
        }
    }
}
