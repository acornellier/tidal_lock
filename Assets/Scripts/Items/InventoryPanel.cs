using FarrokhGames.Inventory;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(InventoryRenderer))]
public class InventoryPanel : MonoBehaviour
{
    [Inject] Player _player;

    void Start()
    {
        GetComponent<InventoryRenderer>().SetInventory(_player.inventory);
    }
}