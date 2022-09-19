using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientIcon : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] TMP_Text _quantity;

    public void PostInstantiate(Ingredient ingredient)
    {
        _image.sprite = ingredient.item.sprite;
        _quantity.text = ingredient.quantity.ToString();
    }
}