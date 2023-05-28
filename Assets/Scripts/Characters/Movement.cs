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
    [SerializeField] Health health = null;

    [SerializeField] AnimationCurve movementCurve;

    public bool hasReachedDestination { get; private set; }

    readonly int walkingVelocity = Animator.StringToHash("Velocity");

    private void Update()
    {
        if (!health.IsAlive) { return; }

        animator.SetFloat(walkingVelocity, agent.velocity.magnitude);

        agent.acceleration = movementCurve.Evaluate(agent.velocity.magnitude);

        hasReachedDestination = (Mathf.Approximately(agent.remainingDistance, Mathf.Epsilon)) ? true : false;
    }

    public bool MoveToPoint(Vector3 point)
    {
        if (!NavMesh.SamplePosition(point, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return false; }

        agent.SetDestination(hit.position);
        agent.updateRotation = true;

        return true;
    }

    public void StopMoving()
    {
        agent.ResetPath();
        agent.updateRotation = false;
    }
}
