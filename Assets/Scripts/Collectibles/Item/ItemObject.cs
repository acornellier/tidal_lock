using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Ingredient
{
    public ItemObject item;
    public int quantity;
}

[CreateAssetMenu(fileName = "ItemObject", menuName = "Inventory/ItemObject", order = 0)]
public class ItemObject : ScriptableObject, IInventoryItem
{
    [SerializeField] Sprite _sprite;
    [SerializeField] InventoryShape _shape;
    [SerializeField] List<Ingredient> _ingredients;
    [SerializeField] bool _isEquipment;

    public string Name => name;
    public Sprite sprite => _sprite;
    public Vector2Int position { get; set; } = Vector2Int.zero;
    public int width => _shape.width;
    public int height => _shape.height;
    public bool isEquipment => _isEquipment;
    public bool canDrop => true;

    public IEnumerable<Ingredient> ingredients => _ingredients;

    public IEnumerable<Ingredient> sortedIngredients =>
        _ingredients.OrderBy(ingredient => ingredient.item.name);

    public bool IsPartOfShape(Vector2Int localPosition)
    {
        return _shape.IsPartOfShape(localPosition);
    }

    public ItemObject CreateInstance()
    {
        var clone = Instantiate(this);
        clone.name = clone.name[..^7]; // Remove (Clone) from name
        return clone;
    }
}