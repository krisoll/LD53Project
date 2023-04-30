using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityCenter : MonoBehaviour
{
    public List<Resource> resources = new List<Resource>();
    private void Start()
    {
        GameManager.Instance.cityCenter = this;
    }
    public void AddResource(Resource resource)
    {
        Resource res = null;
        foreach(Resource reso in resources)
        {
            if (reso.type == resource.type)
            {
                reso.quantity += resource.quantity;
                res = reso;
            }
        }
        if (res == null)
        {
            resources.Add(resource);
        }
        UIManager.Instance.AddResource(resource);
    }
}
