using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStructure : MonoBehaviour
{
    [SerializeField]
    private Vector2 plankDimensions;

    private Bow bow;
    private Rudder rudder;
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
            plank.transform.SetParent(transform, true);
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public bool RemovePlank(Plank plank)
    {
        if(planks.Remove(plank))
        {
            plank.transform.SetParent(null, true);
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
            rudder.transform.SetParent(transform, true);
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public bool RemoveRudder(Rudder rudder)
    {
        if(this.rudder == rudder)
        {
            this.rudder.transform.SetParent(null, true);
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
            this.bow = bow;
            bow.transform.SetParent(transform, true);
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public bool RemoveBow(Bow bow)
    {
        if(this.bow == bow)
        {
            this.bow.transform.SetParent(null, true);
            this.bow = null;
            RecalculateLayout();
            return true;
        }
        return false;
    }

    private void RecalculateLayout()
    {
        RecalculatePlanks();
    }

    private void RecalculatePlanks()
    {
        // Work out the plank structure from the planks we have, then set each plank to move to the correct position
        int width = Mathf.CeilToInt(Mathf.Sqrt(planks.Count));
        int height = Mathf.FloorToInt(Mathf.Sqrt(planks.Count) + 0.5f);
        int i = 0;
        float halfWidth = width / 2.0f;
        float halfHeight = height / 2.0f;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Plank plank = planks[i++];
                plank.MoveTo(new Vector2(x, y) * plankDimensions, 0.5f);
                
                if(i == planks.Count)
                {
                    return; // Consumed all planks
                }
            }
        }
    }
}
