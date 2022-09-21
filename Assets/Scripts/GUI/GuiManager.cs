using UnityEngine;
using Zenject;

public class GuiManager : MonoBehaviour
{
    [SerializeField] GameObject _inventoryCanvas;
    [SerializeField] InventoryRenderer _bagRenderer;
    [SerializeField] InventoryRenderer _equipmentRenderer;

    [SerializeField] GameObject _craftingCanvas;

    [Inject] Player _player;

    void OnEnable()
    {
        _player.controller.onToggleBag += ToggleInventory;
        _player.controller.onToggleCrafting += ToggleCrafting;
    }

    void OnDisable()
    {
        _player.controller.onToggleBag -= ToggleInventory;
        _player.controller.onToggleCrafting -= ToggleCrafting;
    }

    void Start()
    {
        _bagRenderer.SetInventory(_player.inventory.bag);
        _equipmentRenderer.SetInventory(_player.inventory.equipment);
    }

    void ToggleInventory()
    {
        _inventoryCanvas.gameObject.SetActive(!_inventoryCanvas.gameObject.activeSelf);
    }

    void ToggleCrafting()
    {
        _craftingCanvas.gameObject.SetActive(!_craftingCanvas.gameObject.activeSelf);
    }
}