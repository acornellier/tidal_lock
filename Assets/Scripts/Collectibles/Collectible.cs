using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour, IInteractable
{
    [SerializeField] ItemObject _itemObject;
    public ItemObject itemObject => _itemObject;

    public void Interact(Player player)
    {
        player.Collect(this);
    }
}