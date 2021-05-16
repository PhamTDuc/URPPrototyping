using UnityEngine;
using Guinea.Core;

namespace Guinea.Interactable
{
    public class Interactable : IInteractable
    {

        [SerializeField]
        private Stats stats;

        public override void Interact(ICanInteract canInteract)
        {
            canInteract.AddStats(stats);
        }
        void OnValidate()
        {
            Commons.Assert(stats != null, "Stats can't not be null^^!!");
        }


    }
}