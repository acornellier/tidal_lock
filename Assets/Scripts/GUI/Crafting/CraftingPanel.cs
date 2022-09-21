using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingPanel : MonoBehaviour
{
    [SerializeField] RecipePanel _recipePanel;
    [SerializeField] GameObject _recipeList;
    [SerializeField] RecipeButton _recipeButtonPrefab;
    [SerializeField] List<Item> _knownRecipes;

    void Awake()
    {
        foreach (Transform child in _recipeList.transform)
        {
            Utilities.DestroyGameObject(child.gameObject);
        }

        foreach (var item in _knownRecipes.OrderBy(item => item.name))
        {
            var obj = Instantiate(_recipeButtonPrefab, _recipeList.transform);
            obj.PostInstantiate(item);
            obj.OnSelect += _recipePanel.SetRecipe;
        }
    }
}