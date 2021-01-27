using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BST
{
    public class BinarySearchTreePersistable<TKey, TValue> : IDictionaryPersistable<TKey, TValue>
    {
        private readonly IFileSystem _fileSystem;

        public BinarySearchTreePersistable(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IDictionary<TKey, TValue> Deserialize(string path)
        {
            if (!_fileSystem.Directory.Exists(_fileSystem.Path.GetDirectoryName(path)))
                throw new DirectoryNotFoundException("Source directory not exists");

            if (!_fileSystem.File.Exists(path))
                throw new FileNotFoundException("Source file not exists");

            var formatter = new BinaryFormatter();
            using (var fs = _fileSystem.FileStream.Create(path, FileMode.OpenOrCreate))
            {
                return (IDictionary<TKey, TValue>)formatter.Deserialize(fs);
            }
        }

        public void Serialize(string path, IDictionary<TKey, TValue> tree)
        {
            if (!_fileSystem.Directory.Exists(_fileSystem.Path.GetDirectoryName(path)))
                throw new DirectoryNotFoundException("Target directory not exists");

            var formatter = new BinaryFormatter();
            using (var fs = _fileSystem.FileStream.Create(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, tree);
            }
        }
    }
}
