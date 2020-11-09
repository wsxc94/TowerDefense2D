using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormDebuff : Debuff {

    private float minSpeed = 0.5f; //몬스터의 최소 속도

    private bool applied;

    public StormDebuff(Monster target,float duration) : base(target,duration) //디버프
    { 
        if (target != null)
        {
            if (!applied)
            {
                applied = true;
                if (target.Speed > minSpeed) //현재 몬스터의 속도가 최소속도보다 크다면

                {
                    target.Speed = 0f; //속도를 0으로 만듬(스턴효과)
                }
            }
            
        }
        base.Updata();
    }

    public override void Remove() //디버프 삭제
    {
        if (target != null)
        {
            
            target.Speed = target.MaxSpeed;
            base.Remove();
        }
            
    }

}
