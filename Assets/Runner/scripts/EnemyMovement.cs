using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;
    private float fixedX;

    void Start()
    {
        fixedX = transform.position.x;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.x = fixedX;   // lock X axis
        transform.position = pos;
    }
}