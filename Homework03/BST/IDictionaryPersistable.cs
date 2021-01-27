using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BST
{
    interface IDictionaryPersistable<TKey, TValue>
    {
        void Serialize(string path, IDictionary<TKey, TValue> tree);
        IDictionary<TKey, TValue> Deserialize(string path);
    }
}
