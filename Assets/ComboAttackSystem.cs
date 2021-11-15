using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ComboAttackSystem : MonoBehaviour
{
    public GameObject swingEffectPref;

    public Animator refAnim;

    public UnityEvent OnAttack;
    public void Attack()
    {
        OnAttack.Invoke();
        refAnim.SetTrigger("Attack");
        Destroy(Instantiate(swingEffectPref, transform.position + Vector3.up, transform.rotation, transform), 3f);
    }




}
