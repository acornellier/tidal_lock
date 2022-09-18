using FarrokhGames.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class InventoryOpener : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameObject _inventoryCanvas;
    [SerializeField] InventoryRenderer _inventoryRenderer;

    [Inject] Player _player;

    void OnEnable()
    {
        _player.controller.onBag += ToggleInventoryOpen;
    }

    void OnDisable()
    {
        _player.controller.onBag -= ToggleInventoryOpen;
    }

    void Start()
    {
        _inventoryRenderer.SetInventory(_player.inventory);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ToggleInventoryOpen();
    }

    void ToggleInventoryOpen()
    {
        _inventoryCanvas.gameObject.SetActive(!_inventoryCanvas.gameObject.activeSelf);
    }
}