using UnityEngine;
using Zenject;

public class GuiManager : MonoBehaviour
{
    [SerializeField] GameObject _inventoryCanvas;
    [SerializeField] InventoryRenderer _inventoryRenderer;

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
        _inventoryRenderer.SetInventory(_player.inventory);
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