
using UnityEngine;
using Guinea.Core;
public class PoolObjectBase : MonoBehaviour
{
    [SerializeField]
    protected ObjectType objectType;
    public ObjectType Type
    {
        get { return objectType; }
    }
}