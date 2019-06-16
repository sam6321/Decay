using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private float maxHealthWidth;

    [SerializeField]
    private RectTransform bowHealth;

    [SerializeField]
    private RectTransform sternHealth;

    [SerializeField]
    private RectTransform plankHealthMain;

    [SerializeField]
    private RectTransform plankHealthCurrent;

    [SerializeField]
    private ShipStructure structure;

    private void Update()
    {
        SetWidth(bowHealth, structure.BowHealth / structure.BowMaxHealth);
        SetWidth(sternHealth, structure.SternHealth / structure.SternMaxHealth);

        float finalPlankHealth = structure.PlanksHealth % structure.HealthPerPlank;
        float mainPlankHealth = structure.PlanksHealth - finalPlankHealth;
        float maxPlankHealth = structure.Planks.Count * structure.HealthPerPlank;
        SetWidth(plankHealthCurrent, finalPlankHealth / maxPlankHealth);
        SetWidth(plankHealthMain, mainPlankHealth / maxPlankHealth);
    }

    private void SetWidth(RectTransform t, float width)
    {
        Vector2 sizeDelta = t.sizeDelta;
        sizeDelta.x = width * maxHealthWidth;
        t.sizeDelta = sizeDelta;
    }
}
