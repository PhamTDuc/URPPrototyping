namespace Guinea.Core
{
    public interface ICanInteract
    {
        void AddStats(Stats stats);
        bool EquipWeapon(ObjectType type);
    }

}