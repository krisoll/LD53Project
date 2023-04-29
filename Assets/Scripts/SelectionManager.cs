using System.Collections.Generic;
using UnityEngine;
public class SelectionManager
{
    private static SelectionManager _instance;
    public static SelectionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SelectionManager();
            }

            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    public HashSet<Entity> SelectedUnits = new HashSet<Entity>();
    public List<Entity> AvailableUnits = new List<Entity>();

    private SelectionManager() { }

    public void Select(Entity Unit)
    {
        SelectedUnits.Add(Unit);
        Unit.OnSelected();
        Debug.Log("Unit :" + Unit.gameObject.name);
    }

    public void Deselect(Entity Unit)
    {
        Unit.OnDeselected();
        SelectedUnits.Remove(Unit);
    }

    public void DeselectAll()
    {
        foreach (Entity unit in SelectedUnits)
        {
            unit.OnDeselected();
        }
        SelectedUnits.Clear();
    }

    public bool IsSelected(Entity Unit)
    {
        return SelectedUnits.Contains(Unit);
    }
}