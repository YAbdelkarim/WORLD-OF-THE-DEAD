using UnityEngine;

public class FacePlayer : MonoBehaviour
{

    private bool isRotating;
    private Quaternion targetRotation;
    private float rotationSpeed = 2f;

    private Transform player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RotateToPlayer();
    }

    void RotateToPlayer()
    {
        targetRotation = Quaternion.LookRotation(
            Vector3.ProjectOnPlane(player.position - transform.position, Vector3.up)
        );
        if (Quaternion.Angle(transform.rotation, targetRotation) >= 0.1f)
            isRotating = true;
        Rotate();
    }

    void Rotate()
    {
        // Smoothly interpolate to the target rotation
        if (isRotating)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Stop rotating when close enough to target
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }
}
