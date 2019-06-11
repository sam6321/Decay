using UnityEngine;

[CreateAssetMenu(fileName = "Ship Component", menuName = "Ship Component", order = 4)]
class ShipComponent : ScriptableObject
{
    public string componentName;
    public Sprite[] waterSprites;
}
