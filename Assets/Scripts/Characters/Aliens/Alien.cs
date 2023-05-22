using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Alien : MonoBehaviour
{
    [SerializeField] Movement movement = null;
    [SerializeField] Animator animator = null;
    [SerializeField] Attacker attacker = null;
    [SerializeField] Health health = null;

    [Header("Settings")]
    [Tooltip("Minimum x- and z- coordinates alien can travel to")]
    [SerializeField] Vector2 minCoordinates;

    [Tooltip("Maximum x- and z- coordinates alien can travel to")]
    [SerializeField] Vector2 maxCoordinates;

    [Tooltip("Maximum distance alien can detect targets")]
    [SerializeField] float awarenessDistance = 1f;

    [Tooltip("Maximum distance alien can attack target")]
    [SerializeField] float attackDistance = 1f;

    Employee attackTarget = null;
    Building sabotageTarget = null;

    AlienState currentState;
    AlienState previousState;

    public enum AlienState
    {
        Wandering,
        Sabotaging,
        Attacking,
        Impact,
        Dying
    }

    readonly int attack = Animator.StringToHash("Attack1");
    readonly int impact = Animator.StringToHash("Impact");
    readonly int isDead = Animator.StringToHash("IsDead");

    private void OnEnable()
    {
        health.OnDamageTaken += Health_OnDamageTaken;
        health.OnDied += Health_OnDied;
    }

    private void OnDisable()
    {
        health.OnDamageTaken -= Health_OnDamageTaken;
        health.OnDied -= Health_OnDied;
    }

    private void Start()
    {
        SwitchState(AlienState.Wandering);
    }

    private void Update()
    {
        if (!health.IsAlive) { return; }

        ProcessState();
        CheckForTargets();

        if (attackTarget != null && currentState != AlienState.Attacking)
        {
            SwitchState(AlienState.Attacking);
        }
        else if (sabotageTarget != null && currentState != AlienState.Sabotaging)
        {
            SwitchState(AlienState.Sabotaging);
        }
        else if (attackTarget == null && sabotageTarget == null && currentState != AlienState.Wandering)
        {
            SwitchState(AlienState.Wandering);
        }
    }

    #region State Machine
    void SwitchState(AlienState newState)
    {
        ExitState();
        Debug.Log($"{gameObject.name} is switch to {newState} state");
        previousState = currentState;
        currentState = newState;

        EnterState();
    }

    void EnterState()
    {
        switch (currentState)
        {
            case AlienState.Wandering:
                ChooseRandomPlaceToWander();
                break;
            case AlienState.Sabotaging:
                SabotageTarget();
                break;
            case AlienState.Attacking:
                if (attackTarget == null)
                {
                    CheckForTargets();
                }
                ChaseTarget();
                break;
            case AlienState.Impact:
                animator.SetTrigger(impact);
                break;
            case AlienState.Dying:
                if (health.IsAlive) 
                {
                    SwitchState(AlienState.Wandering);
                    return;
                }
                animator.SetBool(isDead, true);
                break;
            default:
                break;
        }
    }

    private void ProcessState()
    {
        switch (currentState)
        {
            case AlienState.Wandering:
                if (!movement.hasReachedDestination) { return; }
                if (attackTarget != null || sabotageTarget != null) { return; }
                
                ChooseRandomPlaceToWander();
                break;
            case AlienState.Sabotaging:
                break;
            case AlienState.Attacking:
                if (attackTarget != null)
                {
                    ChaseTarget();
                }
                break;
            case AlienState.Impact:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Impact")) { return; }
                if (!health.IsAlive) { return; }
                SwitchState(AlienState.Attacking);
                break;
            case AlienState.Dying:
                break;
            default:
                break;
        }
    }

    void ExitState()
    {
        switch (currentState)
        {
            case AlienState.Wandering:
                break;
            case AlienState.Sabotaging:
                break;
            case AlienState.Attacking:
                animator.ResetTrigger(attack);
                break;
            case AlienState.Impact:
                animator.ResetTrigger(impact);
                break;
            case AlienState.Dying:
                break;
            default:
                break;
        }
    }

    #endregion

    #region Private Methods

    private void Health_OnDamageTaken()
    {
        SwitchState(AlienState.Impact);
    }

    private void Health_OnDied()
    {
        SwitchState(AlienState.Dying);
    }

    void CheckForTargets()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, awarenessDistance);

        foreach (Collider target in possibleTargets)
        {
            if (target.TryGetComponent<Employee>(out Employee employee))
            {
                attackTarget = employee;
                attacker.SetCurrentTarget(attackTarget.gameObject);

                if (currentState == AlienState.Attacking) { return; }

                SwitchState(AlienState.Attacking);
                return;
            }
            else
            {
                attackTarget = null;
            }

            if (target.TryGetComponent<Building>(out Building building))
            {
                sabotageTarget = building;
                attacker.SetCurrentTarget(sabotageTarget.gameObject);

                if (currentState == AlienState.Sabotaging) { return; }

                SwitchState(AlienState.Sabotaging);
                return;
            }
            else
            {
                sabotageTarget = null;
            }
        }
        
        if (currentState == AlienState.Wandering) { return; }

        SwitchState(AlienState.Wandering);
        return;
    }

    void ChooseRandomPlaceToWander()
    {
        Vector3 location = new Vector3(Random.Range(minCoordinates.x, maxCoordinates.x), 0f, Random.Range(minCoordinates.y, maxCoordinates.y));

        if (!NavMesh.SamplePosition(location, out NavMeshHit hit, 1f, NavMesh.AllAreas)) 
        { 
            ChooseRandomPlaceToWander(); 
        }

        movement.MoveToPoint(hit.position);
    }

    void ChaseTarget()
    {
        movement.MoveToPoint(attackTarget.transform.position);
        animator.ResetTrigger(attack);

        if (IsWithinAttackRange())
        {
            movement.StopMoving();
            AttackTarget();
        }
    }

    void AttackTarget()
    {
        transform.LookAt(attackTarget.transform);
        animator.SetTrigger(attack);

        if (!IsWithinAttackRange())
        {
            ChaseTarget();
        }
    }

    void SabotageTarget()
    {
        movement.MoveToPoint(sabotageTarget.transform.position);

        if (Vector3.Distance(transform.position, attackTarget.transform.position) <= attackDistance)
        {
            animator.SetTrigger(attack);
        }
    }

    bool IsWithinAttackRange()
    {
        return Vector3.Distance(transform.position, attackTarget.transform.position) <= attackDistance;
    }

    #endregion
}
