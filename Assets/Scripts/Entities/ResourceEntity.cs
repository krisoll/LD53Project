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
    public ResourceEntity StartExtracting()
    {
        return this;
    }
}
public class Resource
{
    public ResourceType type;
    public int quantity;
}
