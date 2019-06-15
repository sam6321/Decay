using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStructure : MonoBehaviour
{
    [SerializeField]
    private Vector2 plankDimensions = new Vector2(0.6f, 3.1875f);

    [SerializeField]
    private Vector2 bowDimensions = new Vector2(2.148438f, 1.734375f);

    [SerializeField]
    private Vector2 sternDimensions = new Vector2(5.46f, 1.26f);

    [SerializeField]
    private uint minPlanksRequiredForBow = 4;

    [SerializeField]
    private Bow bow;
    public Bow Bow => bow;

    [SerializeField]
    private Stern stern;
    public Stern Stern => stern;

    [SerializeField]
    private List<Plank> planks = new List<Plank>();
    public IReadOnlyList<Plank> Planks => planks;

    private Bounds bounds = new Bounds();

    void Start()
    {
        RecalculateLayout();
    }

    public bool AddPlank(Plank plank)
    {
        if(!planks.Contains(plank))
        {
            // TODO: Max planks check
            planks.Add(plank);
            plank.transform.parent = transform;
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public bool RemovePlank(Plank plank)
    {
        if(planks.Remove(plank))
        {
            plank.transform.parent = null;
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public bool AddStern(Stern stern)
    {
        if(!this.stern)
        {
            this.stern = stern;
            stern.transform.parent = transform;
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public bool RemoveStern(Stern stern)
    {
        if(this.stern == stern)
        {
            this.stern.transform.parent = null;
            this.stern = null;
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public bool AddBow(Bow bow)
    {
        if(!this.bow)
        {
            if(planks.Count < minPlanksRequiredForBow)
            {
                // Player hasn't met required planks
                // TODO: popup text here maybe saying "can't grab this"
                return false;
            }

            this.bow = bow;
            bow.transform.parent = transform;
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public bool RemoveBow(Bow bow)
    {
        if(this.bow == bow)
        {
            this.bow.transform.parent = null;
            this.bow = null;
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public Vector2 ClosestPoint(Vector2 point)
    {
        Vector2 local = transform.InverseTransformPoint(point);
        // Closest point on local bounds
        local = bounds.ClosestPoint(local);
        return transform.TransformPoint(local);
    }

    public float DistanceTo(ShipStructure structure)
    {
        Vector2 closestOtherPoint = structure.ClosestPoint(transform.position);
        Vector2 closestPoint = ClosestPoint(closestOtherPoint);
        Debug.DrawLine(closestOtherPoint, closestPoint, Color.red, 1);
        return Vector2.Distance(closestOtherPoint, closestPoint);
    }

    private void RecalculateLayout()
    {
        int width = Mathf.CeilToInt(Mathf.Sqrt(planks.Count));
        int height = Mathf.FloorToInt(Mathf.Sqrt(planks.Count) + 0.5f);

        float halfWidth = (Mathf.Max(0, width - 1)) / 2.0f;
        float halfHeight = (Mathf.Max(0, height - 1)) / 2.0f;

        RecalculatePlanks(width, height, halfWidth, halfHeight);
        RecalculateBow(width, halfHeight);
        RecalculateStern(width, height, halfWidth, halfHeight);
        bounds.extents = new Vector3(
            width * plankDimensions.x, 
            height * plankDimensions.y + 
                (bow ? bowDimensions.y * bow.transform.localScale.y : 0) + 
                (stern ? sternDimensions.y * stern.transform.localScale.y : 0), 
            2.0f
        ) * 0.5f;
    }

    private void RecalculatePlanks(int width, int height, float halfWidth, float halfHeight)
    {
        // Work out the plank structure from the planks we have, then set each plank to move to the correct position
        int i = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Plank plank = planks[i++];
                plank.MoveTo(
                    new Vector2(x - halfWidth, y - halfHeight) * plankDimensions,
                    Vector2.one,
                    0,
                    0.5f
                );
                
                if(i == planks.Count)
                {
                    return; // Consumed all planks
                }
            }
        }
    }

    private void RecalculateBow(int width, float halfHeight)
    {
        if (bow)
        {
            float newScale = (width * plankDimensions.x) / bowDimensions.x;
            bow.MoveTo(
                new Vector2(0, (halfHeight + 0.5f) * plankDimensions.y + bowDimensions.y * newScale * 0.5f),
                new Vector2(newScale, newScale),
                0,
                0.5f
            );
        }
    }

    private void RecalculateStern(int width, int height, float halfWidth, float halfHeight)
    {
        if(stern)
        {
            float newScale = (width * plankDimensions.x) / sternDimensions.x;
            stern.MoveTo(
                new Vector2(0, (halfHeight + 0.5f) * -plankDimensions.y - sternDimensions.y * newScale * 0.5f),
                new Vector2(newScale, newScale),
                0,
                0.5f
            );
        }
    }
}
