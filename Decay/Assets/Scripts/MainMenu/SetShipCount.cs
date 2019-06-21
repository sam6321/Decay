using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetShipCount : MonoBehaviour
{
    public static int ShipCount { get; private set; } = 1;

    [SerializeField]
    private Text count;

    [SerializeField]
    private GameObject[] disable;

    void OnEnable()
    {
        foreach(GameObject go in disable)
        {
            go.SetActive(false);
        }
    }

    void OnDisable()
    {
        foreach(GameObject go in disable)
        {
            go.SetActive(true);
        }
    }

    public void OnUp()
    {
        ShipCount = Mathf.Clamp(ShipCount + 1, 1, 30);
        count.text = ShipCount.ToString();
    }

    public void OnDown()
    {
        ShipCount = Mathf.Clamp(ShipCount - 1, 1, 30);
        count.text = ShipCount.ToString();
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnExit()
    {
        gameObject.SetActive(false);
    }
}
