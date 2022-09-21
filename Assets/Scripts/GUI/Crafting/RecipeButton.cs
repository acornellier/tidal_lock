using System;
using TMPro;
using UnityEngine;

public class RecipeButton : MonoBehaviour
{
    [SerializeField] TMP_Text _text;

    public event Action<Item> OnSelect;

    Item _item;

    void Awake()
    {
        _text.text = "";
    }

    public void PostInstantiate(Item item)
    {
        _item = item;
        _text.text = item.name;
    }

    public void HandleClick()
    {
        OnSelect?.Invoke(_item);
    }
}