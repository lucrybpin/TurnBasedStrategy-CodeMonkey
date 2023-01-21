using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityActionSystem : MonoBehaviour
{
    public static UnityActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;

    [SerializeField] Unit selectedUnity;
    [SerializeField] LayerMask unitLayerMask;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnityActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;
            selectedUnity.Move(MouserWorld.GetPosition());
        }
    }

    bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
        {
            if(raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }

    void SetSelectedUnit(Unit unit)
    {
        selectedUnity = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnity;
    }
}