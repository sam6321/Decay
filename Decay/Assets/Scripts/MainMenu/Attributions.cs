using UnityEngine;
using UnityEngine.EventSystems;

public class Attributions : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown (PointerEventData eventData) 
    {
        gameObject.SetActive(false);
    }
}
