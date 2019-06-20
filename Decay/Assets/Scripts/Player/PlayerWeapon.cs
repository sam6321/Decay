using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField]
    private Scoring scoring;

    private ShipStructure structure;

    // Start is called before the first frame update
    void Start()
    {
        structure = GetComponent<ShipStructure>();
    }

    // Update is called once per frame
    void Update()
    {
        if(structure.Weapon)
        {
            structure.Weapon.Aim(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if(Input.GetMouseButtonDown(1))
            {
                if(structure.Weapon.Fire())
                {
                    scoring.ArrowsFired++;
                }
            }
        }
    }
}
