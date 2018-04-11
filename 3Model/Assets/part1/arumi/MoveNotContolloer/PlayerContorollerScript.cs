using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContorollerScript : MonoBehaviour {

    //移動スピード
    [SerializeField]
    private float speed = 5f;

    //回転速度
    [SerializeField]
    private float rotateSpeed = 120f;

    //入力を受け付ける（移動値）
    private float h,v;

    //
    //private Vector3 velocity;

    public enum MyState{
        Normal,
        Damage
    }

    public MyState state;

    // Use this for initialization
    void Start () {
        state = MyState.Normal;
    }

	// Update is called once per frame
	void Update () {

        //float h = Input.GetAxis("Horizontal_Joy1");
        //float v = Input.GetAxis("Vertical_Joy1");

	}

    public void TakeDamage(Transform enemyTransform)
    {
        state = MyState.Damage;
        h = 0;
        v = 0;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().AddForce(enemyTransform.forward * 10f,ForceMode.Impulse);

        Debug.Log("Damaged!!");
        
    }

    void EndDamage()
    {
        state = MyState.Normal;
    }


    void FixedUpdate()
    {
        if (state == MyState.Normal)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 velocity = new Vector3(0, 0, v);

            velocity = transform.TransformDirection(velocity);

            // キャラクターの移動
            transform.localPosition += velocity * speed * Time.fixedDeltaTime;

            // キャラクターの回転
            transform.Rotate(0, h * rotateSpeed * Time.fixedDeltaTime, 0);
        }
    }



	
}
