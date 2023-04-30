using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Entity
{
    public Resource currentResource;
    public void Update()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Buildings"))
        {
            CityCenter center = GetComponent<CityCenter>();
            center?.AddResource(currentResource);
        }
    }
    IEnumerator ExtractResourceCoroutine()
    {
        yield return null;
    }
}
