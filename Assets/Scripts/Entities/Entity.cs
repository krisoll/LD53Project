using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class Entity : MonoBehaviour
{
    public bool canBeSelected;
    public bool canMove;
    public bool canBeInteractedWith;
    [SerializeField] private bool interacting;
    [SerializeField] protected Entity interactingEntity;
    public GameObject selector;
    public NavMeshAgent agent;
    public Action<GameObject> OnInteract;
    public void OnSelected()
    {
        Debug.Log(gameObject.name + " is Selected");
        if (canBeSelected) selector?.gameObject.SetActive(true);
    }
    public void OnDeselected()
    {
        selector?.gameObject.SetActive(false);
    }
    public virtual void MoveTo(Vector3 movepos)
    {
        if (canMove) agent?.SetDestination(movepos);
    }
    public virtual void InteractWith(GameObject g)
    {
        OnInteract?.Invoke(g);
        if (interactingEntity != null) interactingEntity = null;
        interactingEntity = g.GetComponent<Entity>();
    }
}
