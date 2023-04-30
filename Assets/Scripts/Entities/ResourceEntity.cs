using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ResourceType
{
    NOTHING,
    WOOD,
    GOLD
}
public class ResourceEntity : Entity
{
    [SerializeField]
    private bool isBeingExtracted;
    public ResourceType type;
    public ResourceEntity StartExtracting()
    {
        return this;
    }
}
[System.Serializable]
public class Resource
{
    public ResourceType type;
    public int quantity;
}
