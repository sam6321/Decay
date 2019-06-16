using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStructure : MonoBehaviour
{
    class PlankInfo
    {
        public Plank plank;
        public Vector2 position;
    }

    [SerializeField]
    private Vector2 plankDimensions = new Vector2(0.6f, 2.06f);

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

    /*[SerializeField]
    private Weapon weapon;
    public Weapon Weapon => weapon; */

    [SerializeField]
    private List<Plank> planks = new List<Plank>();
    public IReadOnlyList<Plank> Planks => planks;
    public int Width => Mathf.CeilToInt(Mathf.Sqrt(planks.Count));
    public int Height => Mathf.FloorToInt(Mathf.Sqrt(planks.Count) + 0.5f);

    [SerializeField]
    private List<Oar> oars = new List<Oar>();
    public IReadOnlyList<Oar> Oars => oars;

    private Bounds bounds = new Bounds();

    void Start()
    {
        RecalculateLayout();
    }

    private void OnAdd(GameObject gameObject)
    {
        gameObject.GetComponent<FloatingComponent>().enabled = false;
        var body = gameObject.GetComponent<Rigidbody2D>();
        body.isKinematic = true;
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        body.velocity = new Vector2(0,0);
    }

    private void OnRemove(GameObject gameObject)
    {
        var body = gameObject.GetComponent<Rigidbody2D>();
        body.isKinematic = false;
        body.constraints = RigidbodyConstraints2D.None;
    }

    public bool AddPlank(Plank plank)
    {
        if(!planks.Contains(plank))
        {
            // TODO: Max planks check
            OnAdd(plank.gameObject);
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
            OnRemove(plank.gameObject);
            return true;
        }
        return false;
    }

    public bool AddStern(Stern stern)
    {
        if(!this.stern)
        {
            OnAdd(stern.gameObject);
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
            OnRemove(stern.gameObject);
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

            OnAdd(bow.gameObject);
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
            OnRemove(bow.gameObject);
            return true;
        }
        return false;
    }

    public bool AddOar(Oar oar)
    {
        if(!oars.Contains(oar) && oars.Count < Height * 2)
        {
            // TODO: Check if oar can be added based on planks count
            OnAdd(oar.gameObject);
            oars.Add(oar);
            oar.transform.parent = transform;
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public bool RemoveOar(Oar oar)
    {
        if(oars.Remove(oar))
        {
            oar.transform.parent = null;
            RecalculateLayout();
            OnRemove(oar.gameObject);
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
        int width = Width;
        int height = Height;

        RecalculatePlanks(width, height);
        RecalculateBow(width, height);
        RecalculateStern(width, height);
        RecalculateOars(width, height);
        bounds.extents = new Vector3(
            width * plankDimensions.x, 
            height * plankDimensions.y + 
                (bow ? bowDimensions.y * bow.transform.localScale.y : 0) + 
                (stern ? sternDimensions.y * stern.transform.localScale.y : 0), 
            2.0f
        ) * 0.5f;
    }

    private void RecalculatePlanks(int width, int height)
    {
        int x = 0;
        int y = 0;

        int goal = 1;
        bool xDir = true;

        foreach (Plank plank in planks)
        {
            plank.MoveTo(
                new Vector2(x, y) * plankDimensions,
                Vector2.one,
                0,
                0.5f
            );

            if (xDir)
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

    private void RecalculateOars(int width, int height)
    {
        if(oars.Count == 0)
        {
            return;
        }

        float xOffset = (width % 2 == 0 ? plankDimensions.x * 0.5f : 0.0f);
        float yOffset = (height % 2 == 0 ? plankDimensions.y * 0.5f : 0.0f) + plankDimensions.y * 0.5f;
        float halfWidth = width / 2.0f;
        float halfHeight = height / 2.0f;

        // Alternate oars on left and right side
        int index = 0;
        for (int y = 0; y < height; y++)
        {
            for(int i = -1; i <= 1; i += 2)
            {
                // This is an edge, take an oar and position it here
                float xPosition = i * halfWidth * plankDimensions.x + xOffset;
                float yPosition = halfHeight * plankDimensions.y - (yOffset + y * plankDimensions.y);
                oars[index++].MoveTo(
                    new Vector2(xPosition, yPosition),
                    Vector2.one,
                    i < 0 ? 45 : 135, // Left or right orientation
                    0.5f
                );

                if (index == oars.Count)
                {
                    return; // Exhausted oars
                }
            }
        }
    }
}
