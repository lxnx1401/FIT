using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Follow : MonoBehaviour
{
    [SerializeField] private Transform Target;          // Ziel, dem die Kamera folgt
    [SerializeField] private Vector3 Offset;            // Abstand zur Zielposition
    [SerializeField] private float Speed;               // Geschwindigkeit des Nachfolgens

    private void LateUpdate()
    {
        // 1. Gewünschte Position berechnen
        Vector3 Pos = Target.position + Offset;

        // 2. Aktuelle Position sanft zur Zielposition interpolieren
        Vector3 SmoothMove = Vector3.Lerp(transform.position, Pos, Speed * Time.deltaTime);
        transform.position = SmoothMove;

        // 3. Kamera zum Ziel ausrichten (Rotation)
        Vector3 LookAt = Target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(LookAt);
    }
}
