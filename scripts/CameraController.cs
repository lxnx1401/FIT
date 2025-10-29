using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform player; // Karakterin transformu
    public Vector3 cameraOffset = new Vector3(0f, 8f, -4f); // Kamera pozisyonu
    public float cameraFollowSpeed = 5f; // Takip hızı
    public float rotationSpeed = 2f; // Dönüş hızı
    
    [Header("Input Settings")]
    public bool enableCameraRotation = true;
    public float rotationSensitivity = 2f;

    private float currentRotationY = 0f;

    void Start()
    {
        // Eğer player atanmamışsa bul
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
        // Başlangıç pozisyonunu ayarla
        transform.position = player.position + cameraOffset;
        transform.LookAt(player.position + Vector3.up * 1f); // Karakterin biraz üstüne bak
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Kamera rotasyonu
        if (enableCameraRotation && Mouse.current.rightButton.isPressed)
        {
            currentRotationY += Mouse.current.delta.x.ReadValue() * rotationSensitivity;
        }

        // Kamera pozisyonunu hesapla
        Quaternion rotation = Quaternion.Euler(0f, currentRotationY, 0f);
        Vector3 desiredPosition = player.position + rotation * cameraOffset;

        // Kamera pozisyonunu güncelle
        transform.position = Vector3.Lerp(transform.position, desiredPosition, cameraFollowSpeed * Time.deltaTime);
        
        // Kamerayı karaktere doğru çevir
        Vector3 lookTarget = player.position + Vector3.up * 1.5f; // Karakterin baş hizasına bak
        transform.LookAt(lookTarget);
    }

    // Kamera offset'ini ayarlamak için yardımcı metod
    public void SetCameraOffset(float height, float distance)
    {
        cameraOffset = new Vector3(0f, height, -distance);
    }
}
