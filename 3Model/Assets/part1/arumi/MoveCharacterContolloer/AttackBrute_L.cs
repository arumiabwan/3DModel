using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBrute_L : MonoBehaviour {

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("Hit!!");

            if(col.GetComponent<PlayerContorollerScript>() !=null)col.GetComponent<PlayerContorollerScript>().TakeDamage(transform.root);
            //if (col.GetComponent<KizunaAIContolloerMk3>() != null) col.GetComponent<KizunaAIContolloerMk3>().TakeDamage(transform.root);
            if (col.GetComponent<Modelcontroller>() != null) col.GetComponent<Modelcontroller>().TakeDamage(transform.root);
            //Playerの移動スクリプトを取得してダメージ処理関数を発動させる

            //↑相手が対象のコンポーネントを持ってない場合にそのコンポーネントを呼んでしまわないようにする

        }
    }
}
