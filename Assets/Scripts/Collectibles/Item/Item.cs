using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Ingredient
{
    public Item item;
    public int quantity;
}

[CreateAssetMenu(fileName = "Item", menuName = "Item/Item", order = 0)]
public class Item : ScriptableObject, IInventoryItem
{
    [SerializeField] Sprite _sprite;
    [SerializeField] InventoryShape _shape;
    [SerializeField] List<Ingredient> _ingredients;

    public string Name => name;
    public Sprite sprite => _sprite;
    public Vector2Int position { get; set; } = Vector2Int.zero;
    public int width => _shape.width;
    public int height => _shape.height;
    public bool canDrop => true;

    public IEnumerable<Ingredient> ingredients => _ingredients;

    public IEnumerable<Ingredient> sortedIngredients =>
        _ingredients.OrderBy(ingredient => ingredient.item.name);

    public bool IsPartOfShape(Vector2Int localPosition)
    {
        return _shape.IsPartOfShape(localPosition);
    }

    public virtual ItemSlot GetSlot()
    {
        return ItemSlot.Any;
    }

    public Item CreateInstance()
    {
        var clone = Instantiate(this);
        clone.name = clone.name[..^7]; // Remove (Clone) from name
        return clone;
    }
}