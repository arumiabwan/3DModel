using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KizunaAIContolloerMk3 : MonoBehaviour {

    private CharacterController characterController;
    private Vector3 velocity;
    
    //歩行スピード
    [SerializeField]
    private float walkSpeed = 1.25f;

    //ジャンプ力ぅ……ですかねぇ……
    [SerializeField]
    private float jumpPower = 5f;


    private Animator animator;


    //状態の列挙
    public enum MyState
    {
        Normal,
        Damage,
        Freeze
    }

    //状態を入れる
    private MyState state;
    
    //ダメージ処理関数(相手から呼ばれる)
    public void TakeDamage(Transform enemyTransform)
    {
        state = MyState.Damage;
        velocity = Vector3.zero;
        animator.SetTrigger("Damage");

        //攻撃を受けて移動させられる
        characterController.Move(enemyTransform.forward * 0.5f);

    }

    void EndDamage()//アニメーションイベント
    {
        state = MyState.Normal;
    }

    void JumpStart()//アニメーションイベント
    {
        velocity.y += jumpPower;
    }

    void JumpFreezeIn()
    {
        state = MyState.Freeze;
    }

    void JumpFreezeOut()
    {
        state = MyState.Normal;
    }


    // Use this for initialization
    void Start()
    {
        //コンポーネント取得
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        //初期化
        velocity = Vector3.zero;

    }


    // Update is called once per frame
    void Update()
    {
        if (state == MyState.Normal)
        {

            if (characterController.isGrounded)
            {
                velocity = Vector3.zero;
            }
            var input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            //velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            if (input.magnitude > 0.1f)//方向キーが多少押されている（ベクトルの長さが一定以上）
            {
                //入力値から方向、移動アニメーションを計算
                animator.SetFloat("Speed", input.magnitude);
                transform.LookAt(transform.position + input);
                //velocity += input.normalized * walkSpeed;//実際の移動速度の計算
                velocity.x = input.normalized.x * walkSpeed;
                velocity.z = input.normalized.z * walkSpeed;
            }
            else
            {
                animator.SetFloat("Speed", 0f);
            }
            //}

            //ジャンプ処理
            if (Input.GetKeyDown(KeyCode.Space)
                && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")//既にジャンプ中(着地アニメーション含む)でない
                && !animator.IsInTransition(0)//アニメーション遷移中でない
                )
            {
                animator.SetBool("Jump", true);
                //velocity.y += jumpPower; 
            }

        }//ここまでNormalの動作
        else if (state == MyState.Freeze)
        {
            velocity = Vector3.zero;
        }
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);//算出した移動速度を用いて移動
    }


}
