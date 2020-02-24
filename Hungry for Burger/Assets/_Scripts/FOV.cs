using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    [Header("View Radius and Angle")]
    [Range(0, 1000)]
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    [Header("Masks")]
    public LayerMask targetMask;
    public LayerMask staticMask;
    public LayerMask obstacleMask;
    public LayerMask baitMask;



    public void Update()
    {
        FindTargetInView();
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool isGlobal)
    {
        if(!isGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    

    void FindTargetInView()
    {
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        Collider[] baitsInView = Physics.OverlapSphere(transform.position, viewRadius, baitMask);

        for (int i = 0; i < baitsInView.Length; i++)
        {
            Transform target = baitsInView[i].transform;
            Vector3 dirOfTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirOfTarget) < viewAngle / 2)
            {
                float distance = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirOfTarget, distance, staticMask) && !Physics.Raycast(transform.position, dirOfTarget, distance, obstacleMask))
                {
                    if (gameObject.GetComponent<EnemyAI>().state != EnemyStates.Following)
                        gameObject.GetComponent<EnemyAI>().state = EnemyStates.Jebaited;
                }
            }
        }

        for (int i = 0; i < targetsInView.Length; i++)
        {
            Transform target = targetsInView[i].transform;
            Vector3 dirOfTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirOfTarget) < viewAngle/2)
            {
                float distance = Vector3.Distance(transform.position, target.position);
                
                if (!Physics.Raycast(transform.position, dirOfTarget, distance, staticMask) && !Physics.Raycast(transform.position, dirOfTarget, distance, obstacleMask))
                {
                    if (target.gameObject.GetComponent<Player>().isSneaking)
                    {
                        return;
                    }                    
                    gameObject.GetComponent<EnemyAI>().state = EnemyStates.Following;
                    
                }
            }
        }

        
    }
}
