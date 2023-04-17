using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.AI;

public class EmployeeMovement : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] Animator animator = null;

    Camera mainCamera;

    readonly int walkingVelocity = Animator.StringToHash("Velocity");

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        animator.SetFloat(walkingVelocity, agent.velocity.magnitude);

        if (!Mouse.current.leftButton.wasPressedThisFrame) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }

        Move(hit.point);
    }

    private void Move(Vector3 point)
    {
        if (!NavMesh.SamplePosition(point, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        agent.SetDestination(hit.position);
    }
}
