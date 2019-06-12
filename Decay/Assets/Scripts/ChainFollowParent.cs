using UnityEngine;

public class ChainFollowParent : MonoBehaviour
{
    public PositionFollow Next { get; set; }

    public void Add(PositionFollow next)
    {
        if(!Next)
        {
            next.Target = transform;
        }
        else
        {
            next.Target = Next.transform;
        }

        Next = next;
    }
}
