using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Enables human interaction with an inventory renderer using Unity's event systems
/// </summary>
[RequireComponent(typeof(InventoryRenderer))]
public class InventoryController : MonoBehaviour,
    IPointerDownHandler, IBeginDragHandler, IDragHandler,
    IEndDragHandler, IPointerExitHandler, IPointerEnterHandler
{
    // The dragged item is static and shared by all controllers
    // This way items can be moved between controllers easily
    static InventoryDraggedItem _draggedItem;

    Canvas _canvas;
    internal InventoryRenderer inventoryRenderer;
    internal InventoryManager inventory => (InventoryManager)inventoryRenderer.inventory;

    IInventoryItem _itemToDrag;
    PointerEventData _currentEventData;
    IInventoryItem _lastHoveredItem;

    /*
     * Setup
     */
    void Awake()
    {
        inventoryRenderer = GetComponent<InventoryRenderer>();
        if (inventoryRenderer == null)
            throw new NullReferenceException("Could not find a renderer. This is not allowed!");

        // Find the canvas
        var canvases = GetComponentsInParent<Canvas>();
        if (canvases.Length == 0)
            throw new NullReferenceException("Could not find a canvas.");
        _canvas = canvases[^1];
    }

    /*
     * Grid was clicked (IPointerDownHandler)
     */
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_draggedItem != null) return;
        // Get which item to drag (item will be null of none were found)
        var grid = ScreenToGrid(eventData.position);
        _itemToDrag = inventory.GetAtPoint(grid);
    }

    /*
     * Dragging started (IBeginDragHandler)
     */
    public void OnBeginDrag(PointerEventData eventData)
    {
        inventoryRenderer.ClearSelection();

        if (_itemToDrag == null || _draggedItem != null) return;

        var localPosition = ScreenToLocalPositionInRenderer(eventData.position);
        var itemOffest = inventoryRenderer.GetItemOffset(_itemToDrag);
        var offset = itemOffest - localPosition;

        // Create a dragged item 
        _draggedItem = new InventoryDraggedItem(
            _canvas,
            this,
            _itemToDrag.position,
            _itemToDrag,
            offset
        );

        // Remove the item from inventory
        inventory.TryRemove(_itemToDrag);
    }

    /*
     * Dragging is continuing (IDragHandler)
     */
    public void OnDrag(PointerEventData eventData)
    {
        _currentEventData = eventData;
        if (_draggedItem != null)
        {
            // Update the items position
            //_draggedItem.Position = eventData.position;
        }
    }

    /*
     * Dragging stopped (IEndDragHandler)
     */
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_draggedItem == null) return;

        var mode = _draggedItem.Drop(eventData.position);

        switch (mode)
        {
            case InventoryDraggedItem.DropMode.Dropped:
                ClearHoveredItem();
                break;
            case InventoryDraggedItem.DropMode.Added:
            case InventoryDraggedItem.DropMode.Swapped:
            case InventoryDraggedItem.DropMode.Returned:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _draggedItem = null;
    }

    /*
     * Pointer left the inventory (IPointerExitHandler)
     */
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_draggedItem != null)
        {
            // Clear the item as it leaves its current controller
            _draggedItem.currentController = null;
            inventoryRenderer.ClearSelection();
        }
        else
        {
            ClearHoveredItem();
        }

        _currentEventData = null;
    }

    /*
     * Pointer entered the inventory (IPointerEnterHandler)
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_draggedItem != null)
            // Change which controller is in control of the dragged item
            _draggedItem.currentController = this;

        _currentEventData = eventData;
    }

    /*
     * Update loop
     */
    void Update()
    {
        if (_currentEventData == null) return;

        if (_draggedItem == null)
        {
            // Detect hover
            var grid = ScreenToGrid(_currentEventData.position);
            var item = inventory.GetAtPoint(grid);
            if (item == _lastHoveredItem) return;
            _lastHoveredItem = item;
        }
        else
        {
            // Update position while dragging
            _draggedItem.position = _currentEventData.position;
        }
    }

    /* 
     * 
     */
    void ClearHoveredItem()
    {
        _lastHoveredItem = null;
    }

    /*
     * Get a point on the grid from a given screen point
     */
    internal Vector2Int ScreenToGrid(Vector2 screenPoint)
    {
        var pos = ScreenToLocalPositionInRenderer(screenPoint);
        return new Vector2Int(
            Mathf.FloorToInt(pos.x / inventoryRenderer.cellSize.x),
            Mathf.FloorToInt(-pos.y / inventoryRenderer.cellSize.y)
        );
    }

    Vector2 ScreenToLocalPositionInRenderer(Vector2 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            inventoryRenderer.imageContainer,
            screenPosition,
            _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
            out var localPosition
        );
        return localPosition;
    }
}