using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라 이동 함수

public class CameraMovement : MonoBehaviour {
    [SerializeField]
    private float cameraSpeed =0.0f;
    private float xMax;
    private float yMin;
        
	void Start () {
		
	}
		
	void Update () {
        GetInput();
	}
    private void GetInput()  //방향에 맞춰 이동
    {

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, xMax), Mathf.Clamp(transform.position.y, 0, yMin),-10);
    }
    public void SetLimits(Vector3 maxTile)
    {
        //카메라를 타일이 깔린 x축 y축에 맞춤
        Vector3 wp = Camera.main.ViewportToWorldPoint(new Vector3(1, 0));
        xMax = maxTile.x - wp.x;
        yMin = maxTile.y - wp.y;
    }
}
