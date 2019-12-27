using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    private GameObject target;
    private float speed = 5;

    Vector3 offset;

    void Start()
    {
        target = GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().GamePiece;
        speed = 5;
        offset = target.transform.position - transform.position;
    }

    void LateUpdate()
    {
        // Look
        var newRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, speed * Time.deltaTime);

        // Move
        Vector3 newPosition = target.transform.position + target.transform.forward * offset.z - target.transform.up * offset.y;
        transform.position = Vector3.Slerp(transform.position, newPosition, Time.deltaTime * speed);
    }
}
