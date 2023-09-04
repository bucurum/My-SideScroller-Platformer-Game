using UnityEngine;

public class TiltPlatformController : MonoBehaviour
{
    public float tiltAngle = 15.0f; // Eğilme açısı
    public float tiltSpeed = 1.0f; // Eğilme hızı

    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        // Eğilme açısını hesaplayın
        float tilt = Mathf.Sin(Time.time * tiltSpeed) * tiltAngle;

        // Yeni rotasyonu hesaplayın
        Quaternion newRotation = Quaternion.Euler(initialRotation.eulerAngles.x, initialRotation.eulerAngles.y, tilt);

        // Yeni rotasyonu atayın
        transform.rotation = newRotation;
    }
}
