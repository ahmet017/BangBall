using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void Hit(bool perfect)
    {
        anim.SetTrigger("Hit");
        if (perfect)
            anim.SetTrigger("Perfect");
    }
}
