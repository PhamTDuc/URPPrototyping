using UnityEngine;
using Guinea.Core;

public class DemoWeapon : MonoBehaviour
{
    private IShootable weapon;
    // Start is called before the first frame update
    void Awake()
    {
        weapon = GetComponent<IShootable>();
    }

    // Update is called once per frame
    void Update()
    {
        weapon.Shoot(Input.GetMouseButton(0));
        if (Input.GetMouseButtonDown(1)) weapon.Reload();
    }
}
