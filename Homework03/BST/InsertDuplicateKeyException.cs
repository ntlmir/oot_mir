using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BST
{
    class InsertDuplicateKeyException : Exception
    {
        public InsertDuplicateKeyException() { }
        public InsertDuplicateKeyException(string key)
            : base($"Attempt insert to binary search tree a duplicate key: {key}") { }
    }
}
