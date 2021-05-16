using UnityEngine;
using UnityEngine.EventSystems;
using Guinea.Core;

namespace Guinea
{
    [RequireComponent(typeof(Controller))]
    public class UserControl : MonoBehaviour
    {
        private IPlayable playable;
        void Awake()
        {
            playable = GetComponent<IPlayable>();
        }

        void Start()
        {
            playable.SetThrowObject(playable.Grenade);
        }

        void Update()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            playable.UpDown(Input.GetAxis("Mouse Y"));
            playable.Shoot(Input.GetMouseButton(0));

            if (Input.GetMouseButtonDown(1))
            {
                playable.Reload();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                playable.SetGrenade(ObjectType.GRENADE_02);
                playable.SetThrowObject(playable.Grenade);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                playable.Reset();
            }


            if (playable.isAvailable(playable.Grenade)) playable.Throw(Input.GetKey(KeyCode.R), () => playable.Use(playable.Grenade));


            if (Input.GetKeyUp(KeyCode.R))
            {
                if (playable.isAvailable(playable.Grenade))
                {
                    Commons.Log($"Available Bomb {playable.Grenade} . Remaining: {playable.GetStat(playable.Grenade).quantity}");
                }
                else
                {
                    Commons.Log($"Available Bomb {playable.Grenade} is Empty ^_^");
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
            {
                playable.ChangeWeapon(playable.CurrentIndex + 1);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
            {
                playable.ChangeWeapon(playable.CurrentIndex - 1);
            }
            playable.Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), Input.GetKey(KeyCode.Space));
        }

        void FixedUpdate()
        {
            playable.Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), Input.GetKey(KeyCode.Space));
        }
    }
}