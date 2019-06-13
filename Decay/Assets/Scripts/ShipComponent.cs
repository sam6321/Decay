using System.Collections;
using UnityEngine;

public abstract class ShipComponent : MonoBehaviour
{
    protected ShipStructure attachedStructure;
    private Coroutine moveCoroutine;

    public void MoveTo(Vector2 localTarget, float period)
    {
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveToCoroutine(localTarget, period));
    }

    private IEnumerator MoveToCoroutine(Vector2 target, float period)
    {
        Vector2 current = transform.localPosition;
        float startAngle = transform.localEulerAngles.z;
        float start = Time.time;
        float end = start + period;

        while(Time.time <= end)
        {
            float t = Mathf.Min(Time.time, end);
            float f = Mathf.SmoothStep(0.0f, 1.0f, Mathf.InverseLerp(start, end, t));
            transform.localPosition = Vector2.Lerp(current, target, f);
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(startAngle, 0, t));
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
