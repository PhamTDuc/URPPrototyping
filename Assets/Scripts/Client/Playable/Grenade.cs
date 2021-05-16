using UnityEngine;
using System.Collections;
using Guinea.Core;

[RequireComponent(typeof(Rigidbody))]
public class Grenade : MonoBehaviour, IBeThrown
{
    [SerializeField]
    private BombProperties properties;
    private Rigidbody rb;

    private GameObject effect;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(properties.Delay);
        MasterManager.GetPoolManager().Spawn(properties.Effect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, properties.Radius);
        foreach (Collider collider in colliders)
        {
            IDestructible destroyable = collider.GetComponent<IDestructible>();
            destroyable?.GetDamage(CalculateDamage(collider));
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(properties.Force, transform.position, properties.Radius);
            }
        }
        MasterManager.GetPoolManager().DeactiveOrDestroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, properties.Radius);
    }

    public void Execute(float force, Transform local)
    {
        rb.AddForce(local.forward * force, ForceMode.VelocityChange);
        StartCoroutine(Explode());
    }

    private int CalculateDamage(Collider collider)
    {
        float ratio = 1.0f - Vector3.Distance(transform.position, collider.ClosestPoint(transform.position)) / properties.Radius;
        Commons.Log($"Damage to {collider.name}:  {(int)(ratio * properties.Damage)}");
        return (int)(ratio * properties.Damage);
    }
}
