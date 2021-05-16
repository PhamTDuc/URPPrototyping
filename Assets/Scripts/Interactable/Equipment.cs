using UnityEngine;
using Guinea.Core;

public class Equipment : IInteractable
{
    [SerializeField]
    private ObjectType type;

    public override void Interact(ICanInteract canInteract)
    {
        canInteract.EquipWeapon(type);
    }

    void OnValidate()
    {
        type = Utility.Validate(type, ObjectType.CANNON_00, ObjectType.CANNON_COUNT);
    }
}
