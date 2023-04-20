using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class EmployeeMovement : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] Animator animator = null;

    readonly int walkingVelocity = Animator.StringToHash("Velocity");

    private void Update()
    {
        animator.SetFloat(walkingVelocity, agent.velocity.magnitude);
    }

    private void Move(Vector3 point)
    {
        if (!NavMesh.SamplePosition(point, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        agent.SetDestination(hit.position);
    }
}
