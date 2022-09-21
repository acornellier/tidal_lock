public class PlayerInventory
{
    public readonly InventoryManager bag;
    public readonly InventoryManager equipment;

    public PlayerInventory()
    {
        var bagProvider = new InventoryProvider();
        bag = new InventoryManager(bagProvider, 4, 4);

        var equipmentProvider = new InventoryProvider();
        equipment = new InventoryManager(equipmentProvider, 1, 4);
    }
}