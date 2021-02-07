using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IDMenu : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] InputField id;
    [SerializeField] Button idBtn;
    // Start is called before the first frame update
    
    private void OnEnable()
    {
        scoreText.text = "Score : " + GameManager.instance.wave * 10;
    }
    
}
