using UnityEngine;
using cakeslice;

[RequireComponent(typeof(Outline))]
public class OutlineOnMouseOver : MonoBehaviour
{
    private Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
    }

    private void OnMouseEnter()
    {

    }

    private void OnMouseExit()
    {

    }
}
