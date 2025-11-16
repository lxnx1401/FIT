using UnityEngine;

public class watchCamera : MonoBehaviour
{
   public GameObject Player; // Start is called once before the first execution of Update after the MonoBehaviour is created
private void LateUpdate(){
transform.position=new Vector3(Player.transform.position.x,40,Player.transform.position.z);
}
}
