using System.Collections.Generic;

public class InventoryProvider : IInventoryProvider
{
    readonly List<IInventoryItem> _items = new();

    readonly ItemSlot _itemSlot;

    public InventoryProvider(ItemSlot itemSlot = ItemSlot.Any)
    {
        _itemSlot = itemSlot;
    }

    public int inventoryItemCount => _items.Count;

    public bool isInventoryFull => false;

    public bool AddInventoryItem(IInventoryItem item)
    {
        if (!_items.Contains(item))
        {
            _items.Add(item);
            return true;
        }

        return false;
    }

    public bool DropInventoryItem(IInventoryItem item)
    {
        return RemoveInventoryItem(item);
    }

    public IInventoryItem GetInventoryItem(int index)
    {
        return _items[index];
    }

    public bool CanAddInventoryItem(IInventoryItem item)
    {
        return _itemSlot == ItemSlot.Any || ((Item)item).GetSlot() == ItemSlot.Equipment;
    }

    public bool CanRemoveInventoryItem(IInventoryItem item)
    {
        return true;
    }

    public bool CanDropInventoryItem(IInventoryItem item)
    {
        return true;
    }

    public bool RemoveInventoryItem(IInventoryItem item)
    {
        return _items.Remove(item);
    }
}