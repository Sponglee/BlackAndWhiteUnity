
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


    [SerializeField] private Transform currentTarget;

    private float timer = 0f;
    private float attackTimer = 0f;

    private bool IsOnCoolDown = false;

    private void Start()
    {
        decisionTime *= Random.Range(0.9f, 1.2f);
    }

    public void CheckBehaviour(EnemyController refController)
    {
        timer += Time.deltaTime;

        if (timer >= decisionTime)
        {
            ExecuteBehaviour(refController);
            timer = 0f;
        }
    }


    public void ExecuteBehaviour(EnemyController refController)
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


                IDamagable tmpAttackTarget = GetAttackTarget();
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
                    AttackSequence(refController);
                    DOVirtual.DelayedCall(decisionTime, () => { });
                    CoolDownSequence();
                }
                else
                {
                    aiState = AIState.Move;
                }

                break;
            case AIState.GotHit:
                timer = 0f;

                aiState = AIState.Idle;
                break;
            case AIState.Dead:
                refController.agent.enabled = false;
                break;
        }
    }

    private void AttackSequence(EnemyController refController)
    {
        refController.AttackAnim();
        // CheckTargetDistance();
        DOVirtual.DelayedCall(attackDelay, () =>
        {
            IDamagable tmpTargetToAttack = GetAttackTarget();
            if (tmpTargetToAttack != null)
            {
                refController.Attack(tmpTargetToAttack);
            }

            aiState = AIState.Move;
        });

    }

    private void CoolDownSequence()
    {
        IsOnCoolDown = true;
        DOVirtual.DelayedCall(attackCooldown, () => { IsOnCoolDown = false; });
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

    private IDamagable GetAttackTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, attackRange, targetLayer);

        if (targets.Length > 0)
        {
            if (targets[0].GetComponent<IDamagable>() != null)
                return targets[0].GetComponent<IDamagable>();
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
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}