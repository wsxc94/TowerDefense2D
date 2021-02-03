using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormTower : Tower    //번개 타워
{
    private void Start()
    {
        ElementType = Element.STORM; //속성 설정
        Upgrades = new TowerUpgrade[] //타워 업그레이드
        {
            //가격 , 데미지 , 지속시간 , 확률
            new TowerUpgrade(2,2,0.1f,1),
            new TowerUpgrade(5,5,0.1f,1),
            new TowerUpgrade(10,10,0.1f,1),
            new TowerUpgrade(10,10,0.1f,1),
            new TowerUpgrade(20,20,0.1f,1)
        };
    }
    public override Debuff GetDebuff()
    {
        return new StormDebuff(Target,DebuffDuration); //디버프
    }
    public override string GetStats() //타워 스텟
    {

        return string.Format("<color=#add8e6ff>{0}</color>{1}", "<size=1>번개타워</size>", base.GetStats());
        
    }
}
