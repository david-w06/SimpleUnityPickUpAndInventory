using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject slotPrefab;
    public Transform slotParent;

    // The root panel to show/hide (assign the Panel GameObject in the Inspector)
    public GameObject panel;

    private List<InventorySlotUI> slotUIs = new List<InventorySlotUI>();

    // Layout constants — tweak these to taste
    private const int   Columns   = 5;
    private const float CellSize  = 80f;
    private const float CellGap   = 8f;
    private const float Padding   = 16f;

    void Start()
    {
        if (inventory == null || slotPrefab == null || slotParent == null || panel == null)
        {
            Debug.LogError("InventoryUI: missing references in the Inspector!");
            return;
        }

        SetupGrid();
        SetupPanel();

        for (int i = 0; i < inventory.size; i++)
        {
            GameObject obj = Instantiate(slotPrefab, slotParent);
            var slotUI = obj.GetComponent<InventorySlotUI>();
            if (slotUI == null)
            {
                Debug.LogError($"Slot prefab is missing the InventorySlotUI component! (slot {i})");
                return;
            }
            slotUI.index = i;
            slotUI.inventoryUI = this;
            slotUIs.Add(slotUI);
        }

        inventory.OnInventoryChanged += Refresh;

        panel.SetActive(false); // hidden by default
        Refresh();
    }

    // Adds (or reuses) a GridLayoutGroup that wraps at `Columns` columns
    void SetupGrid()
    {
        var grid = slotParent.GetComponent<GridLayoutGroup>();
        if (grid == null) grid = slotParent.gameObject.AddComponent<GridLayoutGroup>();

        grid.cellSize        = new Vector2(CellSize, CellSize);
        grid.spacing         = new Vector2(CellGap, CellGap);
        grid.padding         = new RectOffset((int)Padding, (int)Padding, (int)Padding, (int)Padding);
        grid.constraint      = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = Columns;
        grid.startAxis       = GridLayoutGroup.Axis.Horizontal;

        // Make slotParent exactly fit its content
        var fitter = slotParent.GetComponent<ContentSizeFitter>();
        if (fitter == null) fitter = slotParent.gameObject.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit   = ContentSizeFitter.FitMode.PreferredSize;
    }

    // Sizes the panel to wrap the grid and centres it on screen
    void SetupPanel()
    {
        int rows = Mathf.CeilToInt((float)inventory.size / Columns);
        float width  = Columns * CellSize + (Columns - 1) * CellGap + Padding * 2;
        float height = rows    * CellSize + (rows    - 1) * CellGap + Padding * 2;

        var rt = panel.GetComponent<RectTransform>();
        rt.anchorMin        = new Vector2(0.5f, 0.5f);
        rt.anchorMax        = new Vector2(0.5f, 0.5f);
        rt.pivot            = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;          // centred on screen
        rt.sizeDelta        = new Vector2(width, height);
    }

    void Update()
    {
        // Press I to toggle the inventory panel (new Input System)
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
        {
            panel.SetActive(!panel.activeSelf);
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < slotUIs.Count; i++)
        {
            slotUIs[i].Set(inventory.slots[i]);
        }
    }

    void OnDestroy()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= Refresh;
    }
}