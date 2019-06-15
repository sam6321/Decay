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

    private void RecalculateLayout()
    {
        int width = Mathf.CeilToInt(Mathf.Sqrt(planks.Count));
        int height = Mathf.FloorToInt(Mathf.Sqrt(planks.Count) + 0.5f);

        float halfWidth = (Mathf.Max(0, width - 1)) / 2.0f;
        float halfHeight = (Mathf.Max(0, height - 1)) / 2.0f;

        RecalculatePlanks(width, height, halfWidth, halfHeight);
        RecalculateBow(width, height);
        RecalculateStern(width, height);
    }

    private void RecalculatePlanks(int width, int height, float halfWidth, float halfHeight)
    {
        int x = 0;
        int y = 0;

        int goal = 1;
        bool xDir = true;

        foreach(Plank plank in planks)
        {
            plank.MoveTo(
                new Vector2(x, y) * plankDimensions,
                Vector2.one,
                0,
                0.5f
            );

            if(xDir)
            {
                if(x != goal)
                {
                    x += goal > x ? 1 : -1;
                }
                else
                {
                    xDir = false;
                    goal = -goal;
                    y += goal > y ? 1 : -1;
                }
            }
            else
            {
                if(y != goal)
                {
                    y += goal > y ? 1 : -1;
                }
                else
                {
                    xDir = true;
                    goal = goal > 0 ? goal + 1 : goal;
                    x += goal > x ? 1 : -1;
                }
            }
        }
    }

    private void RecalculateBow(int width, int height)
    {
        if (bow)
        {
            if(height % 2 == 0)
            {
                height -= 1;
            }
            float newScale = (width * plankDimensions.x) / bowDimensions.x;
            bow.MoveTo(
                new Vector2((width % 2 == 0) ? 0.5f * plankDimensions.x : 0.0f, (float)height / 2.0f * plankDimensions.y + bowDimensions.y * newScale * 0.5f),
                new Vector2(newScale, newScale),
                0,
                0.5f
            );
        }
    }

    private void RecalculateStern(int width, int height)
    {
        if(stern)
        {
            float newScale = (width * plankDimensions.x) / sternDimensions.x;
            if(height % 2 == 0)
            {
                height += 1;
            }
            stern.MoveTo(
                new Vector2((width % 2 == 0) ? 0.5f * plankDimensions.x : 0.0f, (float)height / 2.0f * -plankDimensions.y - sternDimensions.y * newScale * 0.5f),
                new Vector2(newScale, newScale),
                0,
                0.5f
            );
        }
    }
}
