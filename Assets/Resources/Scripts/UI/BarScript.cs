using System;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    
    [SerializeField]
    private bool lerpColors=true; //체력바 막대기 색상 변경 여부


    [SerializeField]
    private Image content=null; //체력바 막대기


    [SerializeField]
    private Text valueText; //체력바 텍스트


    [SerializeField]
    private float lerpSpeed=0.0f; //체력바 이동속도


    private float fillAmount; //체력바가 채워진 양


    [SerializeField]
    private Color fullColor = Color.white; //체력바 색깔


    [SerializeField]
    private Color lowColor=Color.white; //체력바가 낮을때 사용할 색

  
    public float MaxValue { get; set; } //체력바의 최대값


    public float Value //체력바 상수 프로퍼티
    {
        set
        {
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }

    void Start()
    {
        if (lerpColors) //체력바 색 설정
        {
            content.color = fullColor;
        }
    }

  
    void Update ()
    {
      
        HandleBar();

    }


    private void HandleBar()
    {
        //채울양이 있으면 체력바 업데이트
        if (fillAmount != content.fillAmount) 
        {
            //선형보간함수를 이용해 체력바 감소 효과를 부드럽게 만듬
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);

            if (lerpColors) //색설정
            {   
                //색상을 전체색에서 낮음으로 늘림
                content.color = Color.Lerp(lowColor, fullColor, fillAmount);
            }
           
        }
    }
    public void Reset()
    {
        Value = MaxValue;
        content.fillAmount = 1;
        
    }
//체력바 매핑
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
