using UnityEngine;
using UnityEngine.UI;

public class EnemyCountUI : MonoBehaviour
{
    [SerializeField]
    private Text shipsRemainingText;

    [SerializeField]
    private ShipManager shipManager;

    private int lastCount = 0;

    void Update()
    {
        if(shipManager.Ships.Count != lastCount)
        {
            shipsRemainingText.text = shipManager.Ships.Count.ToString();
            lastCount = shipManager.Ships.Count;
        }
    }
}
