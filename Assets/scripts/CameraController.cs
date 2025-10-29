using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour 
{
    public Transform Player;
    public float CameraHeight = 12f;
    public float CameraAngle = 45f;
    public float CameraDistance = 8f; // Kamera mesafesi - İstediğin gibi ayarla!
    public float FollowSpeed = 5f;
    public float LookAtHeight = 1f;

    void Start() 
    {
        ApplyCameraSettings();
    }

    void Update() 
    {
        // Kamera pozisyonunu hesapla (Player'ın arkasında)
        Vector3 cameraOffset = new Vector3(0f, CameraHeight, -CameraDistance);
        Vector3 targetPosition = Player.position + cameraOffset;
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, FollowSpeed * Time.deltaTime);
        
        // Karaktere doğru bak
        Vector3 lookAtTarget = Player.position + Vector3.up * LookAtHeight;
        transform.LookAt(lookAtTarget);
    }

    void ApplyCameraSettings()
    {
        Vector3 cameraOffset = new Vector3(0f, CameraHeight, -CameraDistance);
        Vector3 startPos = Player.position + cameraOffset;
        transform.position = startPos;
        
        Vector3 lookAtTarget = Player.position + Vector3.up * LookAtHeight;
        transform.LookAt(lookAtTarget);
    }
}