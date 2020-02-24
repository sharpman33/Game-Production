using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;
    public GameObject parent;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (parent.GetComponent<EnemyAI>().walking)
            animator.SetBool("Moving", true);
        else
            animator.SetBool("Moving", false);
    }
}
