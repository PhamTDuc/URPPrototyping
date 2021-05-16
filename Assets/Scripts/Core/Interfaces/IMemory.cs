using UnityEngine;
namespace Guinea.Core
{
    public interface IMemory
    {
        MemoryInfo[] Memories { get; }
        bool NotInMemory(GameObject obj);
        bool InMemory(string layerName = null, string tagName = null);
        bool InMemory(string layerName, string tagName, out MemoryInfo info);
    }
}
