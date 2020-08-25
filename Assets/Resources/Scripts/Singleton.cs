using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//싱글턴이란 전역 변수를 사용하지 않고 객체를 하나만 생성 하도록 하며,
//생성된 객체를 어디에서든지 참조할 수 있도록 하는 패턴 이다.

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour 
{
    public static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            return instance;
        }
    }

}
