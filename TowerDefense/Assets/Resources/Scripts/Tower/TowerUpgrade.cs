using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade {

    public int Price { get; private set; } //가격
    public int Damage { get; private set; } //데미지
    public float DebuffDuration { get; private set; } //디버프 지속시간
    public float ProcChance { get; private set; } //확률
    public float SlowingFactor { get; private set; } //둔화
    public float TickTime { get; private set; } //지속간격
    public int SpecialDamage { get; private set; } //데미지

    //각 타워의 매개변수 별로 각 다른 타워 업그레이드 생성자를 불러옴
    public TowerUpgrade(int price, int damage, float debuffduration, float procChance)
    {
        this.Damage = damage;
        this.DebuffDuration = debuffduration;
        this.ProcChance = procChance;
        this.Price = price;
    }

    public TowerUpgrade(int price,int damage,float debuffduration,float procChance,float slowingFactor)
    {
        this.Damage = damage;
        this.DebuffDuration = debuffduration;
        this.ProcChance = procChance;
        this.SlowingFactor = slowingFactor;
        this.Price = price;
    }
    public TowerUpgrade(int price, int damage, float debuffduration, float procChance, float ticktime, int specialdamage)
    {
        this.Damage = damage;
        this.DebuffDuration = debuffduration;
        this.ProcChance = procChance;
        this.TickTime -= ticktime;
        this.SpecialDamage = specialdamage;
        this.Price = price;
    }
}
