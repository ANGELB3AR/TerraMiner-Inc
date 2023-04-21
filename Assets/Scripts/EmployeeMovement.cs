using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class EmployeeMovement : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] Animator animator = null;
    [SerializeField] Outline outline = null;

    readonly int walkingVelocity = Animator.StringToHash("Velocity");

    private void OnEnable()
    {
        EmployeeSelection.OnEmployeeSelected += EmployeeSelection_OnEmployeeSelected;
        EmployeeSelection.OnEmployeeDeselected += EmployeeSelection_OnEmployeeDeselected;
    }

    private void Update()
    {
        animator.SetFloat(walkingVelocity, agent.velocity.magnitude);
    }

    private void OnDisable()
    {
        EmployeeSelection.OnEmployeeSelected -= EmployeeSelection_OnEmployeeSelected;
        EmployeeSelection.OnEmployeeDeselected -= EmployeeSelection_OnEmployeeDeselected;
    }

    public void Move(Vector3 point)
    {
        if (!NavMesh.SamplePosition(point, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        agent.SetDestination(hit.position);
    }
    private void EmployeeSelection_OnEmployeeSelected(EmployeeMovement employee)
    {
        if (employee != this) { return; }

        outline.enabled = true;
    }

    private void EmployeeSelection_OnEmployeeDeselected(EmployeeMovement employee)
    {
        if (employee != this) { return; }

        outline.enabled = false;
    }
}
