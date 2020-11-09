using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSplash : MonoBehaviour //표식
{
    public int Damage { get; set; } //데미지
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Monster")
        {
            //몬스터가 표식을 밟으면 데미지를 입음
            other.GetComponent<Monster>().TakeDamage(Damage, Element.POISON);
            Destroy(gameObject);
        }
    }

}
