using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour {

    private const float MAX_HEALTH = 100f;
    private const float MAX_FILL_AMOUNT = 1f;

    public float health;

    public Image healthBar;
    public Color startColor;
    public Color endColor;

    void Start () {
        health = MAX_HEALTH;

        startColor = ConvertColorFromRGBA(startColor);
        endColor = ConvertColorFromRGBA(endColor);

        healthBar.color = startColor;
    }

    public void ReduceHealth(float amountOfDamage)
    {
        health -= amountOfDamage;

        float value = Mathf.InverseLerp(0, MAX_HEALTH, health);
        float healthValue = Mathf.Lerp(0, MAX_FILL_AMOUNT, value);

        healthBar.fillAmount = healthValue;
        healthBar.color = Color.Lerp(endColor, startColor, healthValue);
    }

    public bool IsHealthBarEmpty()
    {
        return health <= 0;
    }

    // Recalculate color from [0-1] to [0-255]
    private Color32 ConvertColorFromRGBA(Color color) {
        return new Color(color.r, color.g, color.b);
    }

}
