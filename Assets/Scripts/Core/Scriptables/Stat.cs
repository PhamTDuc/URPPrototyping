using Guinea.Core;
namespace Guinea.Core
{
    [System.Serializable]
    public class Stat
    {
        public ObjectType type;
        public int quantity;
        // public ItemInfo info;

        public Stat(Stat stat)
        {
            type = stat.type;
            quantity = stat.quantity;
        }

        public Stat(ObjectType type, int quantity)
        {
            this.type = type;
            this.quantity = quantity;
            // this.info = info;
        }

        public override string ToString()
        {
            return $"Stat(type={type}, quantity={quantity})";
        }
    }
}