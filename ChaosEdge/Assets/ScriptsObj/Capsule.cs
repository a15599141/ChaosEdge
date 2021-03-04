using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Capsule : NetworkBehaviour
{
    // private Transform CapsuleTransform;
    // Start is called before the first frame update
    void Start()
    {
        //CapsuleTransform = gameObject.GetComponent<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer) MoveControl();
    }

    void MoveControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * 0.3f, Space.Self);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * 0.3f, Space.Self);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down * 2.0f, Space.Self);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * 2.0f, Space.Self);
        }

        if (Input.GetMouseButtonDown(0))
        {


        }

        if (Input.GetKeyDown(KeyCode.Escape) /*|| Input.GetKeyDown(KeyCode.Home)*/)
        {
            Quit();
        }
    }
    public void Quit()
    {
        //打包时不能使用
        //UnityEditor.EditorApplication.isPlaying = false;
        //测试时不能执行，打包后可以执行
        Application.Quit();
    }
}
