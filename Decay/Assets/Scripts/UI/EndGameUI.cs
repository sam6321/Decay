using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    [SerializeField]
    private Scoring scoring;

    [SerializeField]
    private Text shipsSunk;

    [SerializeField]
    private Text damageDealt;

    [SerializeField]
    private Text damageTaken;

    [SerializeField]
    private Text planksPickedUp;

    [SerializeField]
    private Text planksLost;

    [SerializeField]
    private Text arrowsFired;

    private void OnEnable()
    {
        shipsSunk.text = scoring.ShipsSunk.ToString();
        damageDealt.text = scoring.DamageDealt.ToString();
        damageTaken.text = scoring.DamageTaken.ToString();
        planksPickedUp.text = scoring.PlanksPickedUp.ToString();
        planksLost.text = scoring.PlanksLost.ToString();
        arrowsFired.text = scoring.ArrowsFired.ToString();
    }

    public void OnPlayAgain()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnQuit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
