using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CharacterContolloerをスクリプトからアタッチ
[RequireComponent(typeof(CharacterController))]

public class PlayerContolloerUseCC : MonoBehaviour {

    private Animator animator;
    private CharacterController characterController;
    private Vector3 velocity;

    //ジャンプ力ぅ……ですかねぇ…
    [SerializeField]
    private float jumpPower=5f;

    //レイを飛ばす体の位置
    [SerializeField]
    private Transform charaRay;

    //レイの長さ
    [SerializeField]
    private float charaRaylength=0.2f;

    //接地判定
    private bool isGround;

    private Vector3 input;

    //歩く速さ
    [SerializeField]
    private float walkSpeed = 1.5f;



	// Use this for initialization
	void Start () {
        
        //コンポーネントの取得
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        velocity = Vector3.zero;//ゼロベクトルで初期化

        isGround = false;

	}

    // Update is called once per frame
    void Update()
    {

        if (!characterController.isGrounded)//キャラコンで接地していないとき
        {
            if (Physics.Linecast(charaRay.position, (charaRay.position - transform.up * charaRaylength)))//レイを飛ばして接地判定
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }


            Debug.DrawLine(charaRay.position, (charaRay.position - transform.up * charaRaylength), Color.red);//レイを可視化
            Debug.Log(isGround);//接地判定
        }

        if (characterController.isGrounded || isGround)//レイまたはキャラコンが接地しているとき
        {
            if (characterController.isGrounded)//キャラコン接地時
            {
                //速度を常に初期化
                velocity = Vector3.zero;

                //ジャンプはしていない
                animator.SetBool("Jump", false);
            }
            else
            {
                //レイが接地判定しているときは重力は与えておく
                velocity = new Vector3(0f, velocity.y, 0f);
            }

            //移動処理
            input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));


            if (input.magnitude > 0f)//方向キーが多少押されている
            {
                animator.SetFloat("Speed", input.magnitude);
                transform.LookAt(transform.position + input);//入力方向を見るように？
                velocity += input.normalized * walkSpeed;//方向をとってwalkSpeedをかける
            }
            else
            {
                animator.SetFloat("Speed", 0f);//ほとんど押されていないときは移動しない
            }

            //ジャンプ処理
            if(Input.GetButtonDown("Jump")
                && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")//ジャンプ中にジャンプしない
                && !animator.IsInTransition(0)//遷移途中にジャンプしないようにする
                )
            {
                animator.SetBool("Jump", true);
                velocity.y += jumpPower;
            }
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);


    }

}
