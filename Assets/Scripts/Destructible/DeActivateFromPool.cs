using System.Collections;
using UnityEngine;
using Guinea.Core;

public class DeActivateFromPool : MonoBehaviour
{
    [SerializeField]
    private float timeToDeactivate;

    void OnEnable()
    {
        StartCoroutine(DeActivate());
    }

    IEnumerator DeActivate()
    {
        yield return new WaitForSeconds(timeToDeactivate);
        MasterManager.GetPoolManager().DeactiveOrDestroy(gameObject);
    }
}