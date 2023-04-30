using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class Entity : MonoBehaviour
{
    public bool canBeSelected;
    public bool canMove;
    public GameObject selector;
    public NavMeshAgent agent;
    public void OnSelected()
    {
        Debug.Log(gameObject.name + " is Selected");
        if (canBeSelected) selector?.gameObject.SetActive(true);
    }
    public void OnDeselected()
    {
        selector?.gameObject.SetActive(false);
    }
    public void MoveTo(Vector3 movepos)
    {
        if (canMove) agent?.SetDestination(movepos);
    }
    public void InteractWith(GameObject g)
    {

    }
}
