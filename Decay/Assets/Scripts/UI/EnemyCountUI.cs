using UnityEngine;
using UnityEngine.UI;

public class EnemyCountUI : MonoBehaviour
{
    [SerializeField]
    private Text enemiesRemainingText;

    [SerializeField]
    private ShipManager shipManager;

    private int lastCount = 0;

    void Update()
    {
        if(shipManager.Ships.Count != lastCount)
        {
            enemiesRemainingText.text = (shipManager.Ships.Count - 1).ToString();
            lastCount = shipManager.Ships.Count;
        }
    }
}
