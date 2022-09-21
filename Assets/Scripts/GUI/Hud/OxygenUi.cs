using TMPro;
using UnityEngine;
using Zenject;

public class OxygenUi : MonoBehaviour
{
    [SerializeField] TMP_Text _text;

    [Inject] Player _player;

    void Update()
    {
        var cur = Mathf.Ceil(_player.oxygen.currentOxygen);
        _text.text = $"Oxygen: {cur}/{_player.oxygen.maxOxygen}";
    }
}