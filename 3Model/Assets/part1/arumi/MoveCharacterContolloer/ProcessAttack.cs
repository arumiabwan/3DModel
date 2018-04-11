using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessAttack : MonoBehaviour {

    private Enemy enemy;
    [SerializeField]
    private SphereCollider SphereColliderR;
    [SerializeField]
    private SphereCollider SphereColliderL;
    private Animator animator;

	// Use this for initialization
	void Start () {

        //コンポーネントの取得
        enemy = GetComponent<Enemy>();
        //SphereColliderL = GetComponentInChildren<SphereCollider>();
        //SphereColliderR = GetComponentInChildren<SphereCollider>();
        animator = GetComponent<Animator>();

	}

    void AttackStart()
    {
        SphereColliderL.enabled = true;//攻撃開始時に攻撃判定をオンにする
        SphereColliderR.enabled = true;//攻撃開始時に攻撃判定をオンにする
    }

    void AttackEnd()
    {
        SphereColliderL.enabled = false;//攻撃終了時に攻撃判定をオフにする
        SphereColliderR.enabled = false;//攻撃終了時に攻撃判定をオフにする
    }

    void StateEnd()
    {       
        enemy.SetState("freeze");
    }

	// Update is called once per frame
	void Update () {
		
	}
}
