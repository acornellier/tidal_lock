using UnityEngine;

public class PlayerOxygen : MonoBehaviour
{
    public float currentOxygen { get; private set; } = 100f;
    public float maxOxygen { get; set; } = _baseMaxOxygen;

    public float changeRate { get; set; }

    const float _baseMaxOxygen = 100f;

    void Update()
    {
        currentOxygen += changeRate * Time.deltaTime;
        currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
    }
}