using System.Collections;
using UnityEngine;

public abstract class ShipComponent : MonoBehaviour
{
    protected ShipStructure attachedStructure;
    private Coroutine moveCoroutine;

    public void MoveTo(Vector2 localTarget, Vector2 localScale, float rotation, float period)
    {
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveToCoroutine(localTarget, localScale, rotation, period));
    }

    private IEnumerator MoveToCoroutine(Vector2 targetPosition, Vector2 targetScale, float targetRotationZ, float period)
    {
        Vector2 startPosition = transform.localPosition;
        Vector2 startScale = transform.localScale;
        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = Quaternion.AngleAxis(targetRotationZ, Vector3.forward);

        float start = Time.time;
        float end = start + period;

        while(Time.time <= end)
        {
            float t = Mathf.Min(Time.time, end);
            float f = Mathf.SmoothStep(0.0f, 1.0f, Mathf.InverseLerp(start, end, t));
            transform.localPosition = Vector2.Lerp(startPosition, targetPosition, f);
            transform.localScale = Vector2.Lerp(startScale, targetScale, f);
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, f);
            yield return null; // Wait until next frame
        }

        moveCoroutine = null;
    }

    public virtual bool Attach(ShipStructure structure)
    {
        if(DoAttach(structure))
        {
            attachedStructure = structure;
            return true;
        }
        return false;
    }

    protected abstract bool DoAttach(ShipStructure structure);

    public virtual bool Detach()
    {
        if(!attachedStructure)
        {
            return false;
        }

        return DoDetach(attachedStructure);
    }

    protected abstract bool DoDetach(ShipStructure structure);
}
