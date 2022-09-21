using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class OxygenBubble : MonoBehaviour
{
    [Inject] Player _player;

    void OnTriggerEnter2D(Collider2D col)
    {
        _player.oxygen.changeRate = 10f;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        _player.oxygen.changeRate = -1f;
    }
}