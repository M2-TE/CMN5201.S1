using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDebugManager : MonoBehaviour {

    public GameObject Inventory;
    bool open;

    public void Start()
    {
        open = false;
        SwitchInventoryMode();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            open = !open;
            SwitchInventoryMode();
        }
    }

    public void SwitchInventoryMode()
    {
        Inventory.SetActive(open);
    }
}
