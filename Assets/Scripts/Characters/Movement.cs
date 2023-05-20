using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] Animator animator = null;

    public bool hasReachedDestination { get; private set; }

    readonly int walkingVelocity = Animator.StringToHash("Velocity");

    private void Update()
    {
        animator.SetFloat(walkingVelocity, agent.velocity.magnitude);

        hasReachedDestination = (Mathf.Approximately(agent.remainingDistance, 0)) ? true : false;
    }

    public void Move(Vector3 point)
    {
        if (!NavMesh.SamplePosition(point, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        agent.SetDestination(hit.position);
    }

    public void StopMoving()
    {
        agent.ResetPath();
    }
}
