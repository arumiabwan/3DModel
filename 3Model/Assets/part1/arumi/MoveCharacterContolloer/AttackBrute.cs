using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBrute : MonoBehaviour {

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("Hit!!");
            col.GetComponent<PlayerContorollerScript>().TakeDamage(transform.root);
            //Playerの移動スクリプトを取得してダメージ処理関数を発動させる
        }
    }
}
