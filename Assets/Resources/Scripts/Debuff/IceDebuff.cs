using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDebuff : Debuff {
    private float slowingFactor; //둔화
    private float minSpeed = 0.5f; //몬스터 최소 속도
    private bool applied; 


    public IceDebuff(float slowingFactor, float duration, Monster target) : base(target, duration) //디버프
    {
        this.slowingFactor = slowingFactor;
    }
    public override void Updata() 
    {
        if (target != null)  //타겟이 공격당하면 둔화효과 적용 (이동속도가 느려짐)
        {
            if (!applied)
            {              
                applied = true;
                if (target.Speed > minSpeed)
                {
                    target.Speed -= (target.MaxSpeed * slowingFactor) / 100f;
                   
                }
                
            }
        }
        base.Updata();
    }
    public override void Remove() //몬스터가 사망한경우 초기화
    {
        if (target != null)
        {
            
            target.Speed = target.MaxSpeed;

            base.Remove();
        }
        
    }

}
