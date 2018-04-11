using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchCharacter : MonoBehaviour {

    void OnTriggerStay(Collider col)
    {
        //Playerが感知範囲内にいるとき
        if (col.tag == "Player") {

            Enemy.EnemyState state = GetComponentInParent<Enemy>().GetState();
            if (state != Enemy.EnemyState.Freeze && state != Enemy.EnemyState.Attack)
            {
                if (state != Enemy.EnemyState.Chase)
                {
                    Debug.Log("追跡開始");
                    GetComponentInParent<Enemy>().SetState("chase", col.transform);
                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("見失う");
            GetComponentInParent<Enemy>().SetState("wait");
        }
    }


}
