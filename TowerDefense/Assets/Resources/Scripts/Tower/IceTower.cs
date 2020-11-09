using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : Tower //얼음 타워
{
    [SerializeField]
    private float slowingFactor; //둔화

    public float SlowingFactor //둔화 프로퍼티
    {
        get
        {
            return slowingFactor;
        }

    }

    private void Start()
    {
        ElementType = Element.ICE; //속성 타입
        Upgrades = new TowerUpgrade[] //업그레이드
        {
            // 가격,데미지,지속시간,확률,둔화율
            new TowerUpgrade(2,1,1,1,2),
            new TowerUpgrade(3,2,1,1,2),
            new TowerUpgrade(4,3,1,1,2),
            new TowerUpgrade(5,4,1,1,2),
            new TowerUpgrade(6,5,1,1,2)
        };
    }
    public override Debuff GetDebuff() //디버프
    {
        return new IceDebuff(SlowingFactor,DebuffDuration,Target);
    }
    public override string GetStats() //타워 스텟
    {
        if (NextUpgrade != null)
        {
            return string.Format("<color=#00ffffff>{0}</color>{1} \n둔화율: {2}% <color=#00ff00ff>+{3}</color>", "<size=15>얼음타워</size>", base.GetStats(), SlowingFactor, NextUpgrade.SlowingFactor);
        }
        return string.Format("<color=#00ffffff>{0}</color>{1} \n둔화율: {2}%", "<size=15>얼음타워</size>", base.GetStats(), SlowingFactor);
    }
    public override void Upgrade() //업그레이드 적용
    {
        this.slowingFactor += NextUpgrade.SlowingFactor;
        base.Upgrade();
    }
}
