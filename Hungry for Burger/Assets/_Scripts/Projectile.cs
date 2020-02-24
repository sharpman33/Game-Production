using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime;

    void Start()
    {
        StartCoroutine(WaitToDie());
    }

    IEnumerator WaitToDie()
    {
        while(true)
        {
            yield return new WaitForSeconds(lifetime);
            Destroy(this.gameObject);
        }
    }
}
