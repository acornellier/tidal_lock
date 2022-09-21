using System;
using UnityEngine;

public interface IInventoryManager : IDisposable
{
    /// <summary>
    /// Invoked when an item is added to the inventory
    /// </summary>
    Action<ItemObject> onItemAdded { get; set; }

    /// <summary>
    /// Invoked when an item was not able to be added to the inventory
    /// </summary>
    Action<ItemObject> onItemAddedFailed { get; set; }

    /// <summary>
    /// Invoked when an item is removed to the inventory
    /// </summary>
    Action<ItemObject> onItemRemoved { get; set; }

    /// <summary>
    /// Invoked when an item is removed from the inventory and should be placed on the ground.
    /// </summary>
    Action<ItemObject> onItemDropped { get; set; }

    /// <summary>
    /// Invoked when an item was unable to be placed on the ground (most likely to its canDrop being set to false)
    /// </summary>
    Action<ItemObject> onItemDroppedFailed { get; set; }

    /// <summary>
    /// Invoked when the inventory is rebuilt from scratch
    /// </summary>
    Action onRebuilt { get; set; }

    /// <summary>
    /// Invoked when the inventory changes its size
    /// </summary>
    Action onResized { get; set; }

    /// <summary>
    /// The width of the inventory
    /// </summary>
    int width { get; }

    /// <summary>
    /// The height of the inventory
    /// </summary>
    int height { get; }

    /// <summary>
    /// Sets a new width and height of the inventory
    /// </summary>
    void Resize(int width, int height);

    /// <summary>
    /// Returns all items inside this inventory
    /// </summary>
    ItemObject[] allItems { get; }

    /// <summary>
    /// Returns true if given item is present in this inventory
    /// </summary>
    bool Contains(ItemObject item);

    /// <summary>
    /// Returns true if this inventory is full
    /// </summary>
    bool isFull { get; }

    /// <summary>
    /// Returns true if its possible to add given item
    /// </summary>
    bool CanAdd(ItemObject item);

    /// <summary>
    /// Add given item to the inventory. Returns true
    /// if successful
    /// </summary>
    bool TryAdd(ItemObject item);

    /// <summary>
    /// Returns true if its possible to add item at location
    /// </summary>
    bool CanAddAt(ItemObject item, Vector2Int point);

    /// <summary>
    /// Tries to add item att location and returns true if successful
    /// </summary>
    bool TryAddAt(ItemObject item, Vector2Int point);

    /// <summary>
    /// Returns true if its possible to remove this item
    /// </summary>
    bool CanRemove(ItemObject item);

    /// <summary>
    /// Returns true ifits possible to swap this item
    /// </summary>
    bool CanSwapAt(ItemObject item, Vector2Int position);

    /// <summary>
    /// Removes given item from this inventory. Returns
    /// true if successful.
    /// </summary>
    bool TryRemove(ItemObject item);

    /// <summary>
    /// Returns true if its possible to drop this item
    /// </summary>
    bool CanDrop(ItemObject item);

    /// <summary>
    /// Removes an item from this inventory. Returns true
    /// if successful.
    /// </summary>
    bool TryDrop(ItemObject item);

    /// <summary>
    /// Drops all items from this inventory
    /// </summary>
    void DropAll();

    /// <summary>
    /// Clears (destroys) all items in this inventory
    /// </summary>
    void Clear();

    /// <summary>
    /// Rebuilds the inventory
    /// </summary>
    void Rebuild();

    /// <summary>
    /// Get an item at given point within this inventory
    /// </summary>
    ItemObject GetAtPoint(Vector2Int point);

    /// <summary>
    /// Returns all items under given rectangle
    /// </summary>
    ItemObject[] GetAtPoint(Vector2Int point, Vector2Int size);
}