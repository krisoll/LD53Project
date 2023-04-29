using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private RectTransform SelectionBox;
    [SerializeField]
    private LayerMask UnitLayers;
    [SerializeField]
    private LayerMask FloorLayers;
    [SerializeField]
    private float DragDelay = 0.1f;

    private float MouseDownTime;
    private Vector2 StartMousePosition;

    private HashSet<Entity> newlySelectedUnits = new HashSet<Entity>();
    private HashSet<Entity> deselectedUnits = new HashSet<Entity>();

    private void Update()
    {
        HandleSelectionInputs();
        HandleMovementInputs();
    }

    private void HandleMovementInputs()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1) && SelectionManager.Instance.SelectedUnits.Count > 0)
        {
            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit Hit, FloorLayers))
            {
                foreach (Entity unit in SelectionManager.Instance.SelectedUnits)
                {
                    unit.MoveTo(Hit.point);
                }
            }
        }
    }

    private void HandleSelectionInputs()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SelectionBox.sizeDelta = Vector2.zero;
            SelectionBox.gameObject.SetActive(true);
            StartMousePosition = Input.mousePosition;
            MouseDownTime = Time.time;
        }
        else if (Input.GetKey(KeyCode.Mouse0) && MouseDownTime + DragDelay < Time.time)
        {
            ResizeSelectionBox();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            SelectionBox.sizeDelta = Vector2.zero;
            SelectionBox.gameObject.SetActive(false);

            foreach (Entity newUnit in newlySelectedUnits)
            {
                SelectionManager.Instance.Select(newUnit);
            }
            foreach (Entity deselectedUnit in deselectedUnits)
            {
                SelectionManager.Instance.Deselect(deselectedUnit);
            }

            newlySelectedUnits.Clear();
            deselectedUnits.Clear();

            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, UnitLayers)
                && hit.collider.TryGetComponent<Entity>(out Entity unit))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (SelectionManager.Instance.IsSelected(unit))
                    {
                        SelectionManager.Instance.Deselect(unit);
                    }
                    else
                    {
                        SelectionManager.Instance.Select(unit);
                    }
                }
                else
                {
                    SelectionManager.Instance.DeselectAll();
                    SelectionManager.Instance.Select(unit);
                }
            }
            else if (MouseDownTime + DragDelay > Time.time)
            {
                SelectionManager.Instance.DeselectAll();
            }

            MouseDownTime = 0;
        }
    }

    private void ResizeSelectionBox()
    {
        float width = Input.mousePosition.x - StartMousePosition.x;
        float height = Input.mousePosition.y - StartMousePosition.y;

        SelectionBox.anchoredPosition = StartMousePosition + new Vector2(width / 2, height / 2);
        SelectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        Bounds bounds = new Bounds(SelectionBox.anchoredPosition, SelectionBox.sizeDelta);

        for (int i = 0; i < SelectionManager.Instance.AvailableUnits.Count; i++)
        {
            if (UnitIsInSelectionBox(Camera.WorldToScreenPoint(SelectionManager.Instance.AvailableUnits[i].transform.position), bounds))
            {
                if (!SelectionManager.Instance.IsSelected(SelectionManager.Instance.AvailableUnits[i]))
                {
                    newlySelectedUnits.Add(SelectionManager.Instance.AvailableUnits[i]);
                }
                deselectedUnits.Remove(SelectionManager.Instance.AvailableUnits[i]);
            }
            else
            {
                deselectedUnits.Add(SelectionManager.Instance.AvailableUnits[i]);
                newlySelectedUnits.Remove(SelectionManager.Instance.AvailableUnits[i]);
            }
        }
    }

    private bool UnitIsInSelectionBox(Vector2 Position, Bounds Bounds)
    {
        return Position.x > Bounds.min.x && Position.x < Bounds.max.x
            && Position.y > Bounds.min.y && Position.y < Bounds.max.y;
    }
}
