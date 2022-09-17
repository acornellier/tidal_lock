using System;
using System.Linq;
using UnityEngine;

namespace FarrokhGames.Inventory
{
    public class InventoryManager : IInventoryManager
    {
        Vector2Int _size = Vector2Int.one;
        IInventoryProvider _provider;
        Rect _fullRect;

        public InventoryManager(IInventoryProvider provider, int width, int height)
        {
            _provider = provider;
            Rebuild();
            Resize(width, height);
        }

        /// <inheritdoc />
        public int width => _size.x;

        /// <inheritdoc />
        public int height => _size.y;

        /// <inheritdoc />
        public void Resize(int newWidth, int newHeight)
        {
            _size.x = newWidth;
            _size.y = newHeight;
            RebuildRect();
        }

        void RebuildRect()
        {
            _fullRect = new Rect(0, 0, _size.x, _size.y);
            HandleSizeChanged();
            onResized?.Invoke();
        }

        void HandleSizeChanged()
        {
            // Drop all items that no longer fit the inventory
            for (var i = 0; i < allItems.Length;)
            {
                var item = allItems[i];
                var shouldBeDropped = false;
                var padding = Vector2.one * 0.01f;

                if (!_fullRect.Contains(item.GetMinPoint() + padding) ||
                    !_fullRect.Contains(item.GetMaxPoint() - padding))
                    shouldBeDropped = true;

                if (shouldBeDropped)
                    TryDrop(item);
                else
                    i++;
            }
        }

        /// <inheritdoc />
        public void Rebuild()
        {
            Rebuild(false);
        }

        void Rebuild(bool silent)
        {
            allItems = new IInventoryItem[_provider.inventoryItemCount];
            for (var i = 0; i < _provider.inventoryItemCount; i++)
            {
                allItems[i] = _provider.GetInventoryItem(i);
            }

            if (!silent) onRebuilt?.Invoke();
        }

        public void Dispose()
        {
            _provider = null;
            allItems = null;
        }

        /// <inheritdoc />
        public bool isFull
        {
            get
            {
                if (_provider.isInventoryFull) return true;

                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        if (GetAtPoint(new Vector2Int(x, y)) == null)
                            return false;
                    }
                }

                return true;
            }
        }

        /// <inheritdoc />
        public IInventoryItem[] allItems { get; private set; }

        /// <inheritdoc />
        public Action onRebuilt { get; set; }

        /// <inheritdoc />
        public Action<IInventoryItem> onItemDropped { get; set; }

        /// <inheritdoc />
        public Action<IInventoryItem> onItemDroppedFailed { get; set; }

        /// <inheritdoc />
        public Action<IInventoryItem> onItemAdded { get; set; }

        /// <inheritdoc />
        public Action<IInventoryItem> onItemAddedFailed { get; set; }

        /// <inheritdoc />
        public Action<IInventoryItem> onItemRemoved { get; set; }

        /// <inheritdoc />
        public Action onResized { get; set; }

        /// <inheritdoc />
        public IInventoryItem GetAtPoint(Vector2Int point)
        {
            return allItems.FirstOrDefault(item => item.Contains(point));
        }

        /// <inheritdoc />
        public IInventoryItem[] GetAtPoint(Vector2Int point, Vector2Int size)
        {
            var posibleItems = new IInventoryItem[size.x * size.y];
            var c = 0;
            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    posibleItems[c] = GetAtPoint(point + new Vector2Int(x, y));
                    c++;
                }
            }

            return posibleItems.Distinct().Where(x => x != null).ToArray();
        }

        /// <inheritdoc />
        public bool TryRemove(IInventoryItem item)
        {
            if (!CanRemove(item)) return false;
            if (!_provider.RemoveInventoryItem(item)) return false;
            Rebuild(true);
            onItemRemoved?.Invoke(item);
            return true;
        }

        /// <inheritdoc />
        public bool TryDrop(IInventoryItem item)
        {
            if (!CanDrop(item) || !_provider.DropInventoryItem(item))
            {
                onItemDroppedFailed?.Invoke(item);
                return false;
            }

            Rebuild(true);
            onItemDropped?.Invoke(item);
            return true;
        }

        internal bool TryForceDrop(IInventoryItem item)
        {
            if (!item.canDrop)
            {
                onItemDroppedFailed?.Invoke(item);
                return false;
            }

            onItemDropped?.Invoke(item);
            return true;
        }

        /// <inheritdoc />
        public bool CanAddAt(IInventoryItem item, Vector2Int point)
        {
            if (!_provider.CanAddInventoryItem(item) || _provider.isInventoryFull)
                return false;

            var previousPoint = item.position;
            item.position = point;
            var padding = Vector2.one * 0.01f;

            // Check if item is outside of inventory
            if (!_fullRect.Contains(item.GetMinPoint() + padding) ||
                !_fullRect.Contains(item.GetMaxPoint() - padding))
            {
                item.position = previousPoint;
                return false;
            }

            // Check if item overlaps another item already in the inventory
            if (!allItems.Any(otherItem => item.Overlaps(otherItem)))
                return true; // Item can be added
            item.position = previousPoint;
            return false;
        }

        /// <inheritdoc />
        public bool TryAddAt(IInventoryItem item, Vector2Int point)
        {
            if (!CanAddAt(item, point) || !_provider.AddInventoryItem(item))
            {
                onItemAddedFailed?.Invoke(item);
                return false;
            }

            item.position = point;

            Rebuild(true);
            onItemAdded?.Invoke(item);
            return true;
        }

        /// <inheritdoc />
        public bool CanAdd(IInventoryItem item)
        {
            Vector2Int point;
            if (!Contains(item) && GetFirstPointThatFitsItem(item, out point))
                return CanAddAt(item, point);
            return false;
        }

        /// <inheritdoc />
        public bool TryAdd(IInventoryItem item)
        {
            if (!CanAdd(item)) return false;
            Vector2Int point;
            return GetFirstPointThatFitsItem(item, out point) && TryAddAt(item, point);
        }

        /// <inheritdoc />
        public bool CanSwap(IInventoryItem item)
        {
            return DoesItemFit(item) &&
                   _provider.CanAddInventoryItem(item);
        }

        /// <inheritdoc />
        public void DropAll()
        {
            var itemsToDrop = allItems.ToArray();
            foreach (var item in itemsToDrop)
            {
                TryDrop(item);
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            foreach (var item in allItems)
            {
                TryRemove(item);
            }
        }

        /// <inheritdoc />
        public bool Contains(IInventoryItem item)
        {
            return allItems.Contains(item);
        }

        /// <inheritdoc />
        public bool CanRemove(IInventoryItem item)
        {
            return Contains(item) && _provider.CanRemoveInventoryItem(item);
        }

        /// <inheritdoc />
        public bool CanDrop(IInventoryItem item)
        {
            return Contains(item) && _provider.CanDropInventoryItem(item) && item.canDrop;
        }

        /*
         * Get first free point that will fit the given item
         */
        bool GetFirstPointThatFitsItem(IInventoryItem item, out Vector2Int point)
        {
            if (DoesItemFit(item))
                for (var y = 0; y < height - (item.height - 1); y++)
                {
                    for (var x = 0; x < width - (item.width - 1); x++)
                    {
                        point = new Vector2Int(x, y);
                        if (CanAddAt(item, point)) return true;
                    }
                }

            point = Vector2Int.zero;
            return false;
        }

        /* 
         * Returns true if given items physically fits within this inventory
         */
        bool DoesItemFit(IInventoryItem item)
        {
            return item.width <= width && item.height <= height;
        }

        /*
         * Returns the center post position for a given item within this inventory
         */
        Vector2Int GetCenterPosition(IInventoryItem item)
        {
            return new Vector2Int(
                (_size.x - item.width) / 2,
                (_size.y - item.height) / 2
            );
        }
    }
}