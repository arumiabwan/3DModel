using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modelcontroller : MonoBehaviour {

    private CharacterController characterController;
    private Vector3 velocity;

    //歩行スピード（という名の走行スピード＝2.2f）
    [SerializeField]
    private float walkSpeed = 1.25f;

    //ジャンプ力ぅ……ですかねぇ……
    [SerializeField]
    private float jumpPower = 5f;


    //ジョイコン操作用
    [SerializeField]
    public string Horizontal = "Horizontal";
    [SerializeField]
    public string Vertical = "Vertical";
    [SerializeField]
    public string Jump = "Jump";


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

    //　現在キャラクターを操作出来るかどうか
    private bool isControl;

    //操作可能状態を外部からコントロールできる
    public void ChangeControl(bool controlFlag)
    {
        isControl = controlFlag;
    }

    //ダメージ処理関数(相手から呼ばれる)
    public void TakeDamage(Transform enemyTransform)
    {
        state = MyState.Damage;
        velocity = Vector3.zero;
        animator.SetTrigger("Damage");

        //攻撃を受けて移動させられる
        characterController.Move(enemyTransform.forward * 0.5f);

    }

    //アニメーションイベント---------------------------------------------

    void EndDamage()//アニメーションイベント
    {
        state = MyState.Normal;
    }

    void JumpStart()//アニメーションイベント
    {
        //velocity.y += jumpPower;
        //Debug.Log("jump");
        jumpflag = true;
    }

    void JumpFreezeIn()
    {
        state = MyState.Freeze;
    }

    void JumpFreezeOut()
    {
        state = MyState.Normal;
    }
    //-------------------------------------------------------------------

    bool jumpflag = false;


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

            if (isControl)
            {

                var input = new Vector3(Input.GetAxis(Horizontal), 0f, Input.GetAxis(Vertical));

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
                if (Input.GetButtonDown(Jump)
                    && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")//既にジャンプ中(着地アニメーション含む)でない
                    && !animator.IsInTransition(0)//アニメーション遷移中でない
                    )
                {
                    animator.SetBool("Jump", true);
                    //velocity.y += jumpPower; 
                }

            }

        }//ここまでNormalの動作
        else if (state == MyState.Freeze)
        {
            velocity.x = Vector3.zero.x;
            velocity.z = Vector3.zero.z;
        }

        if (jumpflag)
        {
            //velocity.y += jumpPower;
            jumpflag = false;
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);//算出した移動速度を用いて移動
    }


}
