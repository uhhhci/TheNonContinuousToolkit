using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAnimationTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SwordAvatar")
        {
            Animator anim = other.GetComponent<Animator>();
            AnimatorStateInfo state =anim.GetCurrentAnimatorStateInfo(0);
            //state.fullPathHash;
            anim.Play("SwordAttack", 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 1, 0), Vector3.one * 2f);
    }
}
