using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityCenter : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.cityCenter = this;
    }
    public void AddResource(Resource resource)
    {

    }
}
