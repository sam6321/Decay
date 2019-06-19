using System.Collections;
using UnityEngine;

public abstract class ShipComponent : MonoBehaviour
{
    protected ShipStructure attachedStructure;
    public ShipStructure AttachedStructure => attachedStructure;

    private Coroutine moveCoroutine;

    private FloatingComponent floatingComponent;
    public FloatingComponent FloatingComponent => floatingComponent;

    private SpriteRenderer spriteRenderer;

    private Color? colour;

    private bool movingOntoShip = false;

    private void Start()
    {
        floatingComponent = GetComponent<FloatingComponent>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (colour.HasValue)
        {
            spriteRenderer.material.SetColor("_Colour", colour.Value);
            spriteRenderer.material.SetFloat("_Amount", 1.0f);
        }
    }

    public void SetOwnerColour(Color colour)
    {
        if(!spriteRenderer)
        {
            this.colour = colour;
        }
        else
        {
            spriteRenderer.material.SetColor("_Colour", colour);
            spriteRenderer.material.SetFloat("_Amount", 1.0f);
        }
    }

    public void ClearOwnerColour()
    {
        if (!spriteRenderer)
        {
            colour = null;
        }
        else
        {
            spriteRenderer.material.SetFloat("_Amount", 0.0f);
        }
    }

    public void MoveTo(float period, Vector2? localTarget=null, Vector2? localScale=null, float? rotation=null)
    {
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveToCoroutine(localTarget, localScale, rotation, period));
    }

    private IEnumerator MoveToCoroutine(Vector2? targetPosition, Vector2? targetScale, float? targetRotationZ, float period)
    {
        // Who doesn't love optionals?
        Vector2? startPosition = targetPosition.HasValue ? transform.localPosition : (Vector2?)null;
        Vector2? startScale = targetScale.HasValue ? transform.localScale : (Vector2?)null;
        Quaternion? startRotation = targetRotationZ.HasValue ? transform.localRotation : (Quaternion?)null;
        Quaternion? targetRotation = targetRotationZ.HasValue ? Quaternion.AngleAxis(targetRotationZ.Value, Vector3.forward) : (Quaternion?)null;

        float start = Time.time;
        float end = start + period;

        while(Time.time <= end)
        {
            float t = Mathf.Min(Time.time, end);
            float f = Mathf.SmoothStep(0.0f, 1.0f, Mathf.InverseLerp(start, end, t));

            if(targetPosition.HasValue)
            {
                transform.localPosition = Vector2.Lerp(startPosition.Value, targetPosition.Value, f);
            }
            
            if(targetScale.HasValue)
            {
                transform.localScale = Vector2.Lerp(startScale.Value, targetScale.Value, f);
            }

            if(targetRotationZ.HasValue)
            {
                transform.localRotation = Quaternion.Slerp(startRotation.Value, targetRotation.Value, f);
            }
            
            yield return null; // Wait until next frame
        }

        // Set here so that it does not collide with other ships 
        // while moving into position
        gameObject.layer = LayerMask.NameToLayer("ShipComponent");
        moveCoroutine = null;
    }

    public bool Attach(ShipStructure structure)
    {
        if(DoAttach(structure))
        {
            attachedStructure = structure;
            movingOntoShip = true;
            return true;
        }
        return false;
    }

    protected abstract bool DoAttach(ShipStructure structure);

    public bool Detach()
    {
        if(!attachedStructure)
        {
            return false;
        }

        if(DoDetach(attachedStructure))
        {
            // Successfully detached, so go back to the floating component layer here
            gameObject.layer = LayerMask.NameToLayer("FloatingComponent");
            return true;
        }
        return false;
    }

    protected abstract bool DoDetach(ShipStructure structure);
}
