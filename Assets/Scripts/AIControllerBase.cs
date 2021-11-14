
using System;
using DG.Tweening;
using UnityEngine;

public class AIControllerBase : MonoBehaviour
{

    [SerializeField] private AIState aiState;
    [SerializeField] private float decisionTime = 0.2f;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float aggroRadius = 5f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private EnemyController refController;
    [SerializeField] private Transform currentTarget;

    private float timer = 0f;
    private float attackTimer = 0f;

    private bool IsOnCoolDown = false;


    private float currentDecisionTime;

    private void Start()
    {
        decisionTime *= UnityEngine.Random.Range(0.9f, 1.2f);
        currentDecisionTime = decisionTime;
        refController = GetComponent<EnemyController>();
    }

    public void CheckBehaviour()
    {
        timer += Time.deltaTime;

        if (timer >= currentDecisionTime)
        {
            ExecuteBehaviour();
            timer = 0f;
        }
    }


    public void ExecuteBehaviour()
    {
        switch (aiState)
        {
            case AIState.Idle:
                Transform tmpTarget = GetMoveTarget();
                if (tmpTarget != null)
                {

                    currentTarget = tmpTarget;
                    refController.Move(tmpTarget.position);
                    aiState = AIState.Move;
                }
                else
                {
                    currentTarget = null;
                    refController.Move(Vector3.zero);
                    aiState = AIState.Idle;
                }
                break;
            case AIState.Move:

                Transform tmpAttackTarget = GetAttackTarget();
                if (tmpAttackTarget != null)
                {
                    aiState = AIState.Attack;
                }
                else
                {
                    aiState = AIState.Idle;
                }

                break;
            case AIState.Attack:
                if (!IsOnCoolDown)
                {
                    AttackSequence();
                    DOVirtual.DelayedCall(decisionTime, () => { });
                    // ResetCooldonw();
                }
                break;
            case AIState.GotHit:
                timer = 0f;
                currentDecisionTime = decisionTime;
                aiState = AIState.Idle;
                break;
            case AIState.Dead:
                refController.agent.enabled = false;
                break;
        }
    }

    private void AttackSequence()
    {

        refController.AttackAnim();
        IsOnCoolDown = true;

        // CheckTargetDistance();
        DOVirtual.DelayedCall(attackDelay, () =>
        {
            // if (aiState != AIState.Attack)
            //     return;

            Transform tmpTargetToAttack = GetAttackTarget();
            refController.Attack(tmpTargetToAttack);
            ResetCooldown(() =>
            {
                aiState = AIState.Idle;
            });

        });

    }

    private void ResetCooldown(Action aCallback)
    {
        IsOnCoolDown = true;
        currentDecisionTime = attackCooldown;
        DOVirtual.DelayedCall(attackCooldown, () =>
        {
            IsOnCoolDown = false;
            currentDecisionTime = decisionTime;
            aCallback();
        });

    }

    private Transform GetMoveTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, aggroRadius, targetLayer);

        if (targets.Length > 0)
        {
            // Debug.Log(targets[0].name);
            return targets[0].transform;
        }

        return null;
    }

    private Transform GetAttackTarget()
    {
        Collider[] targets = Physics.OverlapSphere(refController.attackPoint.position, attackRange, targetLayer);

        if (targets.Length > 0)
        {
            if (targets[0].GetComponent<IDamagable>() != null)
                return targets[0].transform;
        }

        return null;
    }

    private bool CheckTargetDistance()
    {
        if (currentTarget != null)
        {
            if (Vector3.Distance(currentTarget.transform.position, transform.position) > 0.5f)
                return false;
            else
                return true;
        }
        else
            return false;
    }

    public void ChangeState(AIState targetState)
    {
        aiState = targetState;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(refController.attackPoint.position, attackRange);
    }
}