using UnityEngine;

public class EnemyShatterer : MonoBehaviour
{
    void OnEnable()
    {
        var model = GetComponentInParent<MeshRenderer>();

        Destroy(gameObject, 3f);
        Destroy(model.gameObject, 3f);

        model.enabled = false;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().AddExplosionForce(1, transform.position, 1, 0, ForceMode.Impulse);
        }
    }
}