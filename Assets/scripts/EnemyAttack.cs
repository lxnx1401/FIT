using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(other.gameObject);
        }
        else if(other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

    }

}

