using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    //as of RftS Alpha, there's only one weapon type which is a homing projectile. but i'm using this
    //as an excuse to practice making scalable systems, so i'm making a more dynamic weapon system instead
    //of hardcoding it.
    public virtual void Attack()
    {
        Debug.Log("this enemy doesn't have a weapon equipped!");
    }
}
