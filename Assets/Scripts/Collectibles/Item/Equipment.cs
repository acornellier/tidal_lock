using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Item/Equipment", order = 0)]
public class Equipment : Item
{
    [SerializeField] float _maxOxygenIncrease;

    public override ItemSlot GetSlot()
    {
        return ItemSlot.Equipment;
    }

    public void Equip(Player player)
    {
        player.oxygen.maxOxygen += _maxOxygenIncrease;
    }

    public void Unequip(Player player)
    {
        player.oxygen.maxOxygen -= _maxOxygenIncrease;
    }
}