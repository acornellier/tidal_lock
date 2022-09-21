using UnityEngine;

public class PlayerOxygen : MonoBehaviour
{
    public float currentOxygen { get; private set; } = 100f;
    public float maxOxygen = 100f;

    public float changeRate { get; set; }

    void Update()
    {
        currentOxygen += changeRate * Time.deltaTime;
        currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
    }
}