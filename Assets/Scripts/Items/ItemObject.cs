using System.Collections.Generic;
using FarrokhGames.Inventory;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemObject", menuName = "Inventory/ItemObject", order = 0)]
public class ItemObject : ScriptableObject, IInventoryItem
{
    [SerializeField] Sprite _sprite;
    [SerializeField] InventoryShape _shape;
    [SerializeField] List<ItemObject> _craftingIngredients;

    public Sprite sprite => _sprite;

    public Vector2Int position { get; set; } = Vector2Int.zero;

    public int width => _shape.width;

    public int height => _shape.height;

    public bool canDrop => true;

    public List<ItemObject> craftingIngredients => _craftingIngredients;

    public bool IsPartOfShape(Vector2Int localPosition)
    {
        return _shape.IsPartOfShape(localPosition);
    }

    public IInventoryItem CreateInstance()
    {
        var clone = Instantiate(this);
        clone.name = clone.name[..^7]; // Remove (Clone) from name
        return clone;
    }
}