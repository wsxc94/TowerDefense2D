using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
class Stat
{
 
    [SerializeField]
    private BarScript bar=null; //막대기 참조


    [SerializeField]
    private float maxVal; //최대값


    [SerializeField]
    private float currentVal; //현재값

    public float CurrentValue //현재값 프로퍼티
    {
        get
        {
            return currentVal;
        }
        set
        {
            //현재값을 0부터 최대값 사이로 만듬
            this.currentVal = Mathf.Clamp(value, 0, MaxVal);

           
            Bar.Value = currentVal;
        }
    }

  
    public float MaxVal //최대값 프로퍼티
    {
        get
        {
            return maxVal;
        }
        set
        {
            
            Bar.MaxValue = value;

            this.maxVal = value;
        }
    }

    public BarScript Bar
    {
        get
        {
            return bar;
        }

    }

    public void Initialize() //초기화
    {

        this.MaxVal = maxVal;
        this.CurrentValue = currentVal;
    }
}

