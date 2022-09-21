using System;

public class PlayerInventory : IDisposable
{
    public readonly InventoryManager bag;
    public readonly InventoryManager equipment;

    readonly Player _player;

    public PlayerInventory(Player player)
    {
        _player = player;

        var bagProvider = new InventoryProvider();
        bag = new InventoryManager(bagProvider, 4, 4);

        var equipmentProvider = new InventoryProvider(ItemSlot.Equipment);
        equipment = new InventoryManager(equipmentProvider, 1, 4);

        equipment.onItemAdded += HandleEquip;
        equipment.onItemRemoved += HandleUnequip;
    }

    public void Dispose()
    {
        equipment.onItemAdded -= HandleEquip;
        equipment.onItemRemoved -= HandleUnequip;
    }

    void HandleEquip(IInventoryItem item)
    {
        ((Equipment)item).Equip(_player);
    }

    void HandleUnequip(IInventoryItem item)
    {
        ((Equipment)item).Unequip(_player);
    }
}