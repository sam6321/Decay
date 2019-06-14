using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStructure : MonoBehaviour
{
    [SerializeField]
    private Vector2 plankDimensions;

    [SerializeField]
    private Vector2 bowDimensions;

    [SerializeField]
    private uint minPlanksRequiredForBow = 2;

    [SerializeField]
    private Bow bow;

    [SerializeField]
    private Rudder rudder;

    [SerializeField]
    private List<Plank> planks = new List<Plank>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

    public bool AddRudder(Rudder rudder)
    {
        if(!this.rudder)
        {
            this.rudder = rudder;
            rudder.transform.parent = transform;
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public bool RemoveRudder(Rudder rudder)
    {
        if(this.rudder == rudder)
        {
            this.rudder.transform.parent = null;
            this.rudder = null;
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

    private void RecalculateLayout()
    {
        int width = Mathf.CeilToInt(Mathf.Sqrt(planks.Count));
        int height = Mathf.FloorToInt(Mathf.Sqrt(planks.Count) + 0.5f);

        float halfWidth = (Mathf.Max(0, width - 1)) / 2.0f;
        float halfHeight = (Mathf.Max(0, height - 1)) / 2.0f;

        RecalculatePlanks(width, height, halfWidth, halfHeight);
        RecalculateBow(width, halfHeight);
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

    private void RecalculateRudder(int width, int height, float halfWidth, float halfHeight)
    {
        if(rudder)
        {
            //rudder.MoveTo()
        }
    }
}
