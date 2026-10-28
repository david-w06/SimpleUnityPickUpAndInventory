# Simple Pickup & Inventory System for Unity

A clean, modular pickup and inventory system for Unity 3D. Drop it into any project and have collectible items feeding a grid-based 2D overlay inventory in minutes — no extra dependencies beyond **TextMeshPro** and the **Input System** package.

---

## Features

- **Trigger-based pickup** — any 3D object with a Collider (Is Trigger ✅) and a `Collectible` script is auto-collected when the player walks through it
- **Auto-destroy** — collectibles remove themselves from the scene on pickup
- **Stackable items** — items stack up to a configurable max stack size
- **Grid inventory UI** — a 2D screen-space overlay that snaps into a clean rectangle automatically
- **Toggle with `I`** — press `I` at runtime to show/hide the inventory panel
- **ScriptableObject items** — each item type is a lightweight data asset; no prefab duplication needed


---

## Quick Setup (new project)

### 1 — Import required packages
In **Window → Package Manager** make sure these are installed:
- **Input System**
- **TextMeshPro** (usually pre-installed)

### 2 — Create Item assets
Right-click in the Project window → **Create → Inventory → Item**

Fill in:
| Field | Description |
|-------|-------------|
| Item Name | Display name shown in the UI |
| Stackable | Whether multiples stack in one slot |
| Max Stack | Max quantity per slot (e.g. 99) |
| Icon | Any `Sprite` to show in the inventory grid |

### 3 — Scene objects

| GameObject | Component(s) to add | Notes |
|------------|---------------------|-------|
| Player | `Inventory` | Set **Size** (default 10) |
| CollectionManager (empty) | `CollectionManager` | Drag the Player into the **Inventory** field |
| Any 3D mesh (pickup) | `Collectible` + Collider (**Is Trigger** ✅) | Drag an ItemData asset into **Item Data** |
| Player | Collider + Rigidbody | Needed for `OnTriggerEnter` to fire |

### 4 — Canvas / UI

1. **UI → Canvas** (Render Mode: `Screen Space - Overlay`)
2. Inside Canvas → **UI → Panel** — name it `InventoryPanel`
3. Inside InventoryPanel → create an empty child → name it `SlotGrid`
4. Create a **Slot prefab**:
   - UI → Image (name it `Slot`)
   - Add child Image named `Icon`
   - Add child TextMeshPro named `QuantityText`
   - Add `InventorySlotUI` script → assign `Icon` and `QuantityText`
   - Drag into Project to make it a prefab
5. Create an empty GameObject → name it `InventoryUI` → add `InventoryUI` script:

| Field | Assign |
|-------|--------|
| Inventory | Player GameObject (has Inventory component) |
| Slot Prefab | Your Slot prefab |
| Slot Parent | SlotGrid Transform |
| Panel | InventoryPanel GameObject |

Hit **Play** — the grid layout and panel sizing are configured automatically at runtime.

---

## How it works (flow)

```
Player collides with Collectible
  → CollectionManager.CollectItem(itemData, amount)
    → Inventory.AddItem()       ← stacks or opens a new slot
      → OnInventoryChanged      ← event fires
        → InventoryUI.Refresh() ← all slot icons update
  → Destroy(collectible)        ← pickup disappears

Press [I] → Inventory panel toggles on/off
```

---

## Customisation tips

- **Grid columns / cell size** — edit the constants at the top of `InventoryUI.cs`:
  ```csharp
  private const int   Columns  = 5;
  private const float CellSize = 80f;
  private const float CellGap  = 8f;
  private const float Padding  = 16f;
  ```
- **Inventory size** — change `size` on the `Inventory` component in the Inspector
- **New item types** — just create another `ItemData` asset; no code changes needed
- **Non-stackable items** — uncheck `Stackable` on the ItemData asset; each pickup takes its own slot

---

## Requirements

- Unity **6** or later (URP template)
- **Input System** package
- **TextMeshPro** package

---
