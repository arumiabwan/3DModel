using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacter : MonoBehaviour {

    //操作中のキャラクター
    private int nowChara;

    [SerializeField]
    private List<GameObject> charaList;

    void CharacterChanger(int tempNowChara)
    {
        bool flag;//

        //次のキャラクターを指定（今の次の番号）
        int nextChara = tempNowChara+1;

        //指定した番号がリストより多い＝次がいないとき、最初にもどる
        if (nextChara >= charaList.Count)
        {
            nextChara = 0;
        }

        for (var i = 0; i < charaList.Count; i++)
        {
            if (i == nextChara)//操作キャラ番号の時のみ代入するflagはtrueになる
            {
                flag = true;
            }
            else
            {
                flag = false;
            }

            //キャラクターごとにオンオフを割り当てる
            charaList[i].GetComponent<Modelcontroller>().ChangeControl(flag);
            //アニメーションをSpeed0で初期化しておく
            charaList[i].GetComponent<Animator>().SetFloat("Speed", 0);
            
        }

        nowChara = nextChara;
    }


	// Use this for initialization
	void Start () {

        //最初の操作キャラをセット
        nowChara = charaList.Count;
        CharacterChanger(nowChara);

    }


	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CharacterChanger(nowChara);
        }

	}
}
