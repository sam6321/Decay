using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Common;

public class ShipStructure : MonoBehaviour
{
    [System.Serializable]
    public class OnLoseEvent : UnityEvent<ShipStructure> { }

    [SerializeField]
    private GameObject onDestroyParticles;

    [SerializeField]
    private AudioGroup onDestroySounds;

    [SerializeField]
    private GameObject onDestroySoundPrefab;

    [SerializeField]
    private AudioGroup onDamagedSounds;

    [SerializeField]
    private ColourGroup randomColours;

    [SerializeField]
    private Color colour = new Color(1, 1, 1);

    [SerializeField]
    private Scoring scoring = null;

    [SerializeField]
    private Vector2 plankDimensions = new Vector2(0.6f, 2.06f);

    [SerializeField]
    private Vector2 bowDimensions = new Vector2(2.148438f, 1.734375f);

    [SerializeField]
    private Vector2 sternDimensions = new Vector2(5.46f, 1.26f);

    [SerializeField]
    private uint minPlanksRequiredForBow = 4;
    public uint MinPlanksRequiredForBow => minPlanksRequiredForBow;

    [SerializeField]
    private uint minPlanksRequiredForStern = 4;
    public uint MinPlanksRequiredForStern => minPlanksRequiredForStern;

    [SerializeField]
    private Bow bow;
    public Bow Bow => bow;

    [SerializeField]
    private Stern stern;
    public Stern Stern => stern;

    [SerializeField]
    private Weapon weapon;
    public Weapon Weapon => weapon;

    public float BowHealth { get; set; } = 0;
    public float SternHealth { get; set; } = 0;
    public float PlanksHealth { get; set; } = 0;

    [SerializeField]
    private float healthPerPlank = 20;
    public float HealthPerPlank => healthPerPlank;

    [SerializeField]
    private float bowMaxHealth = 100;
    public float BowMaxHealth => bowMaxHealth;

    [SerializeField]
    private float sternMaxHealth = 100;
    public float SternMaxHealth => sternMaxHealth;

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

    [SerializeField]
    private OnLoseEvent onLose = new OnLoseEvent();
    public OnLoseEvent OnLose => onLose;

    private AudioSource audioSource;
    private Rigidbody2D rigidbody2D;
    private Bounds bounds = new Bounds();
    private bool noRecalcLayout = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (randomColours)
        {
            colour = randomColours.GetRandom();
        }

        RecalculateLayout();
        PlanksHealth = healthPerPlank * planks.Count;
    }

    private void Update()
    {
        if (stern)
        {
            DamageStern(2 * Time.deltaTime);
        }

        if(bow)
        {
            DamageBow(2 * Time.deltaTime);
        }

        if(planks.Count > 0)
        {
            float drain = planks.Count == 1 ? 0.0f : 5.0f;
            DamagePlanks(drain * Time.deltaTime, null);
        }
    }

    private Vector2 lastVelocity;

    void FixedUpdate()
    {
        lastVelocity = rigidbody2D.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.rigidbody)
        {
            return;
        }

        ShipStructure otherStructure = collision.rigidbody.GetComponent<ShipStructure>();
        if(!otherStructure)
        {
            return; // Not colliding with a ship
        }

        // Calculate the damage we should take on collision
        Vector2 direction = collision.otherRigidbody.position - collision.rigidbody.position;
        float orthogonalSpeed = Vector2.Dot((lastVelocity + collision.relativeVelocity), direction);
        if(orthogonalSpeed < 0)
        {
            orthogonalSpeed = 0;
        }
        float damageMultiplier = orthogonalSpeed * collision.rigidbody.mass;

        ApplyDamage(collision.otherCollider, new DamageInfo()
        {
            source = otherStructure,
            amount = damageMultiplier * 2,
            force = (collision.otherRigidbody.position - collision.rigidbody.position) * 100
        });
    }

    private void AddComponent(ShipComponent component)
    {
        component.SetOwnerColour(colour);
        component.GetComponent<SpriteRenderer>().sortingOrder = component is Oar ? 2 : 1;
        component.transform.parent = transform;
    }

    private void RemoveComponent(ShipComponent component)
    {
        component.ClearOwnerColour();
        component.GetComponent<SpriteRenderer>().sortingOrder = 0;
        component.transform.parent = null;
    }

    public void ApplyDamage(Collider2D collider, DamageInfo info)
    {
        if(!collider.transform.IsChildOf(transform))
        {
            // TODO: There's a bug here that I need to fix...
            Debug.LogError("Can't apply damage to object that we don't own");
            return;
        }

        if (collider.CompareTag("Plank") || collider.CompareTag("Oar"))
        {
            DamagePlanks(info.amount, info.source);
        }
        else if (collider.CompareTag("Bow") || collider.CompareTag("Weapon"))
        {
            DamageBow(info.amount);
        }
        else if (collider.CompareTag("Stern"))
        {
            DamageStern(info.amount);
        }

        if(scoring)
        {
            scoring.DamageTaken += (uint)info.amount;
        }

        onDamagedSounds.PlayRandomOneShot(audioSource);

        rigidbody2D.AddForce(info.force.normalized * 100);
    }

    public bool AddPlank(Plank plank)
    {
        if(!planks.Contains(plank))
        {
            planks.Add(plank);
            AddComponent(plank);
            PlanksHealth = Mathf.Min(planks.Count * healthPerPlank, PlanksHealth + 1.25f * healthPerPlank);
            RecalculateLayout();
            if (scoring)
            {
                scoring.PlanksPickedUp++;
            }
            return true;
        }
        return false;
    }

    public bool RemovePlank(Plank plank)
    {
        if(planks.Remove(plank))
        {
            RemoveComponent(plank);
            RecalculateLayout();
            if (scoring)
            {
                scoring.PlanksLost++;
            }
            return true;
        }
        return false;
    }

    public void DamagePlanks(float amount, ShipStructure attacker)
    {
        PlanksHealth -= amount; 
        while (PlanksHealth < (healthPerPlank * (planks.Count - 1)) && Planks.Count > 0)
        {
            if (!planks[planks.Count - 1].FloatingComponent.TryReturnToSpawner())
            {
                Debug.LogError("Should not happen");
            }
        }

        if (PlanksHealth <= 0)
        {
            PlanksHealth = 0;
            OnLose.Invoke(this);
            onDestroySounds.PlayRandomOneShot(Instantiate(onDestroySoundPrefab, transform.position, Quaternion.identity).GetComponent<AudioSource>());
            Instantiate(onDestroyParticles, transform.position, Quaternion.identity);

            if(attacker.CompareTag("Player"))
            {
                GameObject.Find("GameManager").GetComponent<Scoring>().ShipsSunk++;
            }
            Destroy(gameObject);
        }
    }

    public bool CanPickupStern => !stern && planks.Count >= minPlanksRequiredForStern;

    public bool AddStern(Stern stern)
    {
        if(CanPickupStern)
        {
            this.stern = stern;
            AddComponent(stern);
            SternHealth = 100f;
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public bool RemoveStern(Stern stern)
    {
        if(this.stern == stern)
        {
            RemoveComponent(stern);
            this.stern = null;
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public void DamageStern(float amount)
    {
        SternHealth -= amount;
        if (SternHealth <= 0)
        {
            SternHealth = 0;
            if (!stern.FloatingComponent.TryReturnToSpawner())
            {
                Debug.LogError("Should not happen");
            }
        }
    }

    public bool CanPickupBow => !bow && planks.Count >= minPlanksRequiredForBow;

    public bool AddBow(Bow bow)
    {
        if(CanPickupBow)
        {
            this.bow = bow;
            AddComponent(bow);
            BowHealth = 100f;
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public bool RemoveBow(Bow bow)
    {
        if(this.bow == bow)
        {
            RemoveComponent(bow);
            this.bow = null;
            if(weapon)
            {
                // Weapon comes off with the bow
                if(!weapon.FloatingComponent.TryReturnToSpawner())
                {
                    Debug.LogError("Should not happen");
                }
            }
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public void DamageBow(float amount)
    {
        BowHealth -= amount;
        if (BowHealth <= 0)
        {
            BowHealth = 0;
            if (!bow.FloatingComponent.TryReturnToSpawner())
            {
                Debug.LogError("Should not happen");
            }
        }
    }

    public bool CanPickupOar => oars.Count < Height * 2;

    public bool AddOar(Oar oar)
    {
        if(!oars.Contains(oar) && CanPickupOar)
        {
            oars.Add(oar);
            AddComponent(oar);
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public bool RemoveOar(Oar oar)
    {
        if(oars.Remove(oar))
        {
            RemoveComponent(oar);
            RecalculateLayout();
            return true;
        }
        return false;
    }

    public bool CanPickupWeapon => !weapon && bow;

    public bool AddWeapon(Weapon weapon)
    {
        if(CanPickupWeapon)
        {
            AddComponent(weapon);
            this.weapon = weapon;
            RecalculateLayout();
            return true;
        }

        return false;
    }

    public bool RemoveWeapon(Weapon weapon)
    {
        if(this.weapon == weapon)
        {
            RemoveComponent(weapon);
            this.weapon = null;
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
        return Vector2.Distance(closestOtherPoint, closestPoint);
    }

    private void RecalculateLayout()
    {
        if(noRecalcLayout)
        {
            return; // skip layout recalc for now
        }

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
            plank.MoveTo(0.5f, localTarget: new Vector2(x, y) * plankDimensions, rotation: 0);

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
            Vector2 bowPosition = new Vector2((width % 2 == 0) ? 0.5f * plankDimensions.x : 0.0f, (float)height / 2.0f * plankDimensions.y + bowDimensions.y * newScale * 0.5f);
            Vector2 bowScale = new Vector2(newScale, newScale);
            bow.MoveTo(0.5f, bowPosition, bowScale, 0);

            if(weapon)
            {
                weapon.MoveTo(0.5f, bowPosition, bowScale);
            }
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
            Vector2 sternPosition = new Vector2((width % 2 == 0) ? 0.5f * plankDimensions.x : 0.0f, (float)height / 2.0f * -plankDimensions.y - sternDimensions.y * newScale * 0.5f);
            stern.MoveTo(0.5f, sternPosition, new Vector2(newScale, newScale), 0);
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
                    0.5f,
                    localTarget: new Vector2(xPosition, yPosition),
                    rotation: i < 0 ? 45 : 135 // Left or right orientation
                );

                if (index == oars.Count)
                {
                    return; // Exhausted oars
                }
            }
        }

        // Some oars left over need dropping because we don't have the planks to support them
        noRecalcLayout = true; // pretty bad hack to prevent layout recalculation while we're removing oars
        for (int count = oars.Count; index < count; index++)
        {
            oars[oars.Count - 1].FloatingComponent.TryReturnToSpawner();
        }
        noRecalcLayout = false;
    }
}
