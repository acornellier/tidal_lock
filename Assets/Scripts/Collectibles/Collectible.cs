using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour, IInteractable
{
    [SerializeField] Item _item;
    public Item item => _item;

    void Awake()
    {
        if (_item == null)
            Debug.LogError($"Missing item assignment {_item}");
    }

    public void Interact(Player player)
    {
        player.Collect(this);
        Utilities.DestroyGameObject(gameObject);
    }
}