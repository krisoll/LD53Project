using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text gold;
    [SerializeField] private TMP_Text wood;
    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
    public void AddResource(Resource res)
    {
        if(res.type == ResourceType.WOOD)
        {
            int woodC = int.Parse(wood.text);
            woodC += res.quantity;
            wood.SetText(woodC.ToString());
        }
        if (res.type == ResourceType.GOLD)
        {
            int goldC = int.Parse(gold.text);
            goldC += res.quantity;
            wood.SetText(goldC.ToString());
        }
    }
}
