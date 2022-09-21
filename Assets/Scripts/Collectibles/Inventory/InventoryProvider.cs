using System.Collections.Generic;

public class InventoryProvider : IInventoryProvider
{
    readonly List<ItemObject> _items = new();

    readonly bool _equipmentOnly;

    public InventoryProvider(bool equipmentOnly = false)
    {
        _equipmentOnly = equipmentOnly;
    }

    public int inventoryItemCount => _items.Count;

    public bool isInventoryFull => false;

    public bool AddInventoryItem(ItemObject item)
    {
        if (!_items.Contains(item))
        {
            _items.Add(item);
            return true;
        }

        return false;
    }

    public bool DropInventoryItem(ItemObject item)
    {
        return RemoveInventoryItem(item);
    }

    public ItemObject GetInventoryItem(int index)
    {
        return _items[index];
    }

    public bool CanAddInventoryItem(ItemObject item)
    {
        return !_equipmentOnly || item.isEquipment;
    }

    public bool CanRemoveInventoryItem(ItemObject item)
    {
        return true;
    }

    public bool CanDropInventoryItem(ItemObject item)
    {
        return true;
    }

    public bool RemoveInventoryItem(ItemObject item)
    {
        return _items.Remove(item);
    }
}