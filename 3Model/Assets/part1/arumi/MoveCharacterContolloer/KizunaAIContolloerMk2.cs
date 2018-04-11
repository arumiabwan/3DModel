using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KizunaAIContolloerMk2 : MonoBehaviour {

    private CharacterController characterController;
    private Vector3 velocity;
    [SerializeField]
    private float walkSpeed = 1.25f;
    private Animator animator;

    public enum MyState {
        Normal,
        Damage
    }

    private MyState state;

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

    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (state == MyState.Normal)
        {

            if (characterController.isGrounded)
            {
                velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
                if (velocity.magnitude > 0.1f)
                {
                    animator.SetFloat("Speed", velocity.magnitude);
                    transform.LookAt(transform.position + velocity);

                }
                else
                {
                    animator.SetFloat("Speed", 0f);
                }
            }
        }//通常状態の動作
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }


}
