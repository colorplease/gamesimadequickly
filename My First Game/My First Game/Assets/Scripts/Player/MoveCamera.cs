using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = cameraPosition.position;
    }
}
