using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {

    private CharacterController enemyController;
    private Animator animator;

    //　目的地
    private Vector3 destination;
    //　歩くスピード
    private float walkSpeed = 1.0f;
    //　速度
    private Vector3 velocity;
    //　移動方向
    private Vector3 direction;
    //　到着フラグ
    private bool arrived;
    //　スタート位置
    //private Vector3 startPosition;

    //SetPosition
    private SetPosition setPosition;
    //　待ち時間
    [SerializeField]
    private float waitTime = 5f;
    //　経過時間
    private float elapsedTime;

    //　敵の状態
    private EnemyState state;

    public enum EnemyState
    {
        Walk,
        Wait,
        Chase
    }

    //　追いかけるキャラクター
    private Transform playerTransform;


    //敵の状態を管理
    public void SetState(string mode, Transform obj = null)
    {
        if (mode == "walk")
        {
            arrived = false;
            elapsedTime = 0f;
            state = EnemyState.Walk;
            setPosition.CreateRandomPosition();
        }
        else if (mode == "chase")
        {
            state = EnemyState.Chase;
            //待機から追いかける場合もあるので、とりあえずarrivedはオフにする
            arrived = false;
            //追いかける対象をセットする
            playerTransform = obj;
        }
        else if (mode == "wait")
        {
            elapsedTime = 0f;
            state = EnemyState.Wait;
            arrived = true;//到着状態＝wait
            velocity = Vector3.zero;
            animator.SetBool("Moving", false);
        }


    }

    public EnemyState GetState()
    {
        return state;
    }


    public void FootR() { }
    public void FootL() { }


    // Use this for initialization
    void Start()
    {

        //コンポーネント取得
        enemyController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        setPosition = GetComponent<SetPosition>();

        //目的地設定
        //var randDestination = Random.insideUnitCircle * 8;
        //destination = startPosition + new Vector3(randDestination.x, 0, randDestination.y);
        //destination = new Vector3(25f, 0f, 25f);
        setPosition.CreateRandomPosition();

        //速度初期化
        velocity = Vector3.zero;

        //その他初期化作業
        arrived = false;
        elapsedTime = 0f;

        SetState("wait");

    }

    // Update is called once per frame
    void Update()
    {

        //見回り状態or追跡状態
        if (state == EnemyState.Walk || state == EnemyState.Chase)
        {
            if (state == EnemyState.Chase)//追跡中
            {
                setPosition.SetDestination(playerTransform.position);//追跡キャラの座標を目的地に設定
            }

            if (enemyController.isGrounded)//接地時
            {
                velocity = Vector3.zero;
                animator.SetBool("Moving", true);
                direction = (setPosition.GetDestination() - transform.position).normalized;
                transform.LookAt(new Vector3(setPosition.GetDestination().x, transform.position.y, setPosition.GetDestination().z));
                velocity = direction * walkSpeed;
            }

            //目的地に到着したかどうか
            if (Vector3.Distance(transform.position, setPosition.GetDestination()) < 0.7f)
            {
                SetState("wait");
                animator.SetBool("Moving", false);
            }
        }
        else if (state == EnemyState.Wait)
        {
            elapsedTime += Time.deltaTime;
            //待ち時間を超えたら
            if (elapsedTime > waitTime)
            {
                SetState("walk");
            }
        }

        //重力・移動処理
        velocity.y += Physics.gravity.y * Time.deltaTime;
        enemyController.Move(velocity * Time.deltaTime);

    }
}


    //    if (!arrived)
    //    {
    //        if (enemyController.isGrounded)
    //        {
    //            velocity = Vector3.zero;
    //            //animator.SetFloat("Speed", 2.0f);
    //            animator.SetBool("Moving", true);

    //            //移動先へ向かうように各値を設定する
    //            direction = (destination - transform.position).normalized;
    //            transform.LookAt(new Vector3(destination.x, transform.position.y, destination.z));
    //            velocity = direction * walkSpeed;

    //            Debug.Log(destination);
    //        }

    //        //重力・移動処理
    //        velocity.y += Physics.gravity.y * Time.deltaTime;
    //        enemyController.Move(velocity * Time.deltaTime);

    //        //　目的地に到着したかどうかの判定
    //        if (Vector3.Distance(transform.position, destination) < 0.5f)
    //        {
    //            arrived = true;
    //            Debug.Log(arrived);
    //            animator.SetBool("Moving", false);
    //        }
    //    }
    //    else //到着していたら
    //    {
    //        elapsedTime += Time.deltaTime;
    //    }




    //    //待ち時間を超えたら次の目的地を設定してまた動き始める
    //    if (elapsedTime > waitTime)
    //    {
    //        setPosition.CreateRandomPosition();
    //        destination = setPosition.GetDestination();
    //        arrived = false;
    //        elapsedTime = 0f;
    //    }
    //    Debug.Log(elapsedTime);
    //}
//}
