﻿using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RecipePanel : MonoBehaviour
{
    [SerializeField] TMP_Text _title;
    [SerializeField] Image _icon;
    [SerializeField] GameObject _ingredientList;
    [SerializeField] IngredientIcon _ingredientPrefab;

    [Inject] Player _player;

    ItemObject _itemObject;

    void Awake()
    {
        ClearContents();
    }

    public void HandleCraft()
    {
        if (MissingIngredients())
        {
            print("Missing ingredients");
            return;
        }

        foreach (var ingredient in _itemObject.ingredients)
        {
            for (var i = 0; i < ingredient.quantity; ++i)
            {
                var itemToRemove =
                    _player.inventory.allItems.First(item => item.Name == ingredient.item.Name);
                if (!_player.inventory.TryRemove(itemToRemove))
                    throw new Exception($"Failed to remove {ingredient.item} while crafting");
            }
        }

        _player.inventory.TryAdd(_itemObject.CreateInstance());
    }

    bool MissingIngredients()
    {
        return _itemObject.ingredients.Any(
            ingredient =>
            {
                var amountInInventory =
                    _player.inventory.allItems.Count(item => item.Name == ingredient.item.Name);
                return amountInInventory < ingredient.quantity;
            }
        );
    }

    public void SetRecipe(ItemObject itemObject)
    {
        _itemObject = itemObject;

        if (_itemObject == null)
            ClearContents();
        else
            InitializeContents();
    }

    void ClearContents()
    {
        _title.text = "";
        _icon.enabled = false;

        foreach (Transform child in _ingredientList.transform)
        {
            Utilities.DestroyGameObject(child.gameObject);
        }
    }

    void InitializeContents()
    {
        _title.text = _itemObject.name;
        _icon.enabled = true;
        _icon.sprite = _itemObject.sprite;

        foreach (Transform child in _ingredientList.transform)
        {
            Utilities.DestroyGameObject(child.gameObject);
        }

        foreach (var ingredient in _itemObject.sortedIngredients)
        {
            var obj = Instantiate(_ingredientPrefab, _ingredientList.transform);
            obj.PostInstantiate(ingredient);
        }
    }
}