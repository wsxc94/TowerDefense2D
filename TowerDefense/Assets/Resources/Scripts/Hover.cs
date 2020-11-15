using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover> {

    private SpriteRenderer spriteRenderer; 

    private SpriteRenderer rangeSpriteRenderer;

    public bool IsVisible { get; private set; }
                                       // Use this for initialization
    void Start () {
        //타워 이미지와 타워범위(range) 이미지 컴포넌트 취득
        spriteRenderer = GetComponent<SpriteRenderer>();

        rangeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
        FollowMouse();
	}
    private void FollowMouse()  //타워 버튼을 누르면 타워 이미지가 마우스포인터를 따라옴
    {
        if (spriteRenderer.enabled) //타워 이미지가 있다면
        {
            //메인카메라의 스크린포인트를 마우스포인터기준으로 따라감
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //현재 좌표는 이 자표의 x,y축을 따라감
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        
    }
    public void Activate(Sprite sprite)  //타워 선택 o
    {
        this.spriteRenderer.sprite = sprite;

        spriteRenderer.enabled = true;

        rangeSpriteRenderer.enabled = true;

        IsVisible = true;
    }
    public void Deactivate() //타워 선택 x
    {
        spriteRenderer.enabled = false;

        rangeSpriteRenderer.enabled = false;

        GameManager.Instance.ClickedBtn = null;

        IsVisible = false;

    }
}
