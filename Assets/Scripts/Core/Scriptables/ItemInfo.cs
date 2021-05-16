using UnityEngine;
using UnityEngine.UI;

namespace Guinea.Core
{
    [CreateAssetMenu(fileName="ItemInfo", menuName="Inventory/ItemInfo")]
    public class ItemInfo : ScriptableObject
    {
        [SerializeField]
        private string item_name;
        [SerializeField]
        private string description;
        [SerializeField]
        private Sprite sprite;
        public string Name {get => item_name;}
        public string Description { get => description;}
        public Sprite Sprite { get => sprite;}
    }

}