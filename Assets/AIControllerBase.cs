
using UnityEngine;

public class AIControllerBase : MonoBehaviour
{

    [SerializeField] private AIState aiState;

    [SerializeField] private float decisionTime = 2f;
    [SerializeField] private float aggroRadius = 5f;
    [SerializeField] private float attackRange = 1f;

    [SerializeField] private LayerMask targetLayer;
    private float timer = 0f;



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
                    refController.Move(tmpTarget.position);
                    aiState = AIState.Move;
                }
                else
                {
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

                IDamagable tmpTargetToAttack = GetAttackTarget();
                if (tmpTargetToAttack != null)
                {
                    refController.Attack(tmpTargetToAttack);
                }
                aiState = AIState.Move;

                break;
            case AIState.Dead:
                break;
        }
    }


    private Transform GetMoveTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, aggroRadius, targetLayer);

        if (targets.Length > 0)
        {
            Debug.Log(targets[0].name);
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



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}