using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    [SerializeField] Transform entryContatainer;
    [SerializeField] Transform entry;
    float entryHeight = 80f;

    private void OnEnable()
    {
        entry.gameObject.SetActive(false);
        GameManager.instance.DataBase.DataRead(GameManager.instance.DataBase.Query);
        GameManager.instance.DataBase.RankData.Sort(delegate(Item a , Item b)
        {
            if (a.score < b.score) return 1;
            else if (a.score > b.score) return -1;
            else return 0;
        });

        for (int i = 0; i < 9; i++)
        {
            if (GameManager.instance.DataBase.RankData.Count <= i) break;
            Transform entryTr = Instantiate(entry, entryContatainer);
            RectTransform entryRectTr = entryTr.GetComponent<RectTransform>();
            entryRectTr.anchoredPosition = new Vector2(0, -entryHeight * i);
            entryTr.gameObject.SetActive(true);

            entryTr.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            entryTr.GetChild(1).GetComponent<Text>().text = GameManager.instance.DataBase.RankData[i].name;
            entryTr.GetChild(2).GetComponent<Text>().text = GameManager.instance.DataBase.RankData[i].score.ToString();

        }

    }
}
