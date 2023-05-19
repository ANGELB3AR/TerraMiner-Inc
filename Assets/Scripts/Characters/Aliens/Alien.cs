using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Alien : MonoBehaviour
{
    [SerializeField] Movement movement = null;
    [SerializeField] Animator animator = null;
    [SerializeField] Attacker attacker = null;

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

    readonly int attack = Animator.StringToHash("Attack1");

    private void Start()
    {
        ChooseRandomPlaceToWander();
    }

    private void Update()
    {
        if (attackTarget == null && sabotageTarget == null)
        {
            CheckForTargets();
        }
        
        if (attackTarget == null) { return; }

        AttackTarget();

        if (sabotageTarget == null) { return; }

        SabotageTarget();
    }

    void CheckForTargets()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, awarenessDistance);

        foreach (Collider target in possibleTargets)
        {
            if (target.TryGetComponent<Employee>(out Employee employee))
            {
                attackTarget = employee;
                return;
            }

            else if (target.TryGetComponent<Building>(out Building building))
            {
                sabotageTarget = building;
                return;
            }
        }
        return;
    }

    void ChooseRandomPlaceToWander()
    {
        Vector3 location = new Vector3(Random.Range(minCoordinates.x, maxCoordinates.x), 0f, Random.Range(minCoordinates.y, maxCoordinates.y));

        if (!NavMesh.SamplePosition(location, out NavMeshHit hit, 1f, NavMesh.AllAreas)) 
        { 
            ChooseRandomPlaceToWander(); 
        }

        movement.Move(hit.position);
    }

    void ChaseTarget()
    {
        movement.Move(attackTarget.transform.position);
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
        movement.Move(sabotageTarget.transform.position);

        if (Vector3.Distance(transform.position, attackTarget.transform.position) <= attackDistance)
        {
            animator.SetTrigger(attack);
        }
    }

    bool IsWithinAttackRange()
    {
        return Vector3.Distance(transform.position, attackTarget.transform.position) <= attackDistance;
    }

    /*
     WANDERING
        - IF COME ACROSS BUILDING THEN START DESTROYING IT
        - IF COME ACROSS EMPLOYEE THEN ATTACK
     FIGHTING
        - IF TARGET DIES THEN FIND NEW TARGET
            - IF NO NEW TARGETS THEN WANDER AGAIN
    SABOTAGING
        - IF ATTACKED THEN FIGHT BACK
        - IF EQUIPMENT DESTROYED THEN WANDER AGAIN
     */


}
