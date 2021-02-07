using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;
using UnityEngine.Networking;
using System;

public class DataBaseManager : Singleton<DataBaseManager>
{
    private string query = "Select * From Ranking"; public string Query { get { return query; } }

    private void Awake()
    {
        StartCoroutine(DBCreate());
    }

    public void DBConnectionCheck()
    {
        try
        {
            IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
            dbConnection.Open();

            if (dbConnection.State == ConnectionState.Open)
            {
                Debug.Log("데이터베이스 연결 성공");
            }
            else
            {
                Debug.Log("데이터베이스 연결 실패");
            }
        }
        catch (Exception e)
        {

            Debug.Log(e);
        }
    }
    public void DataRead(string query)
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open(); //DB 오픈
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query; // 쿼리 입력
        IDataReader dataReader = dbCommand.ExecuteReader(); // 쿼리 실행
        while (dataReader.Read()) // 들어온 레코드 읽기
        {
            Debug.Log(dataReader.GetInt32(0) + " , " + dataReader.GetString(1) + " , " + dataReader.GetInt32(2));
            //0 - 1 - 2 번 필드 읽기
        }
        dataReader.Dispose(); // 생성순서와 반대로 닫아줌
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close(); // DB에는 1개의 쓰레드만이 접근할 수 있고 동시에 접근시 에러
        dbConnection = null;

    }

    private string GetDBFilePath()
    {
        string path = string.Empty;

        if (Application.platform == RuntimePlatform.Android)
        {
            path = "URI=file:" + Application.persistentDataPath + "/TowerDefenseRank.db";
        }
        else
        {
            path = "URI=file:" + Application.dataPath + "/TowerDefenseRank.db";
        }
        return path;

    }

    private IEnumerator DBCreate()
    {
        string filepath = string.Empty;
        if (Application.platform == RuntimePlatform.Android)
        {
            filepath = Application.persistentDataPath + "/TowerDefenseRank.db";
            if (!File.Exists(filepath))
            {
                UnityWebRequest unityWebRequest = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/TowerDefenseRank.db");
                unityWebRequest.downloadedBytes.ToString();
                yield return unityWebRequest.SendWebRequest().isDone;
                File.WriteAllBytes(filepath, unityWebRequest.downloadHandler.data);
            }
        }
        else
        {
            filepath = Application.dataPath + "/TowerDefenseRank.db";
            if (!File.Exists(filepath))
            {
                File.Copy(Application.streamingAssetsPath + "/TowerDefenseRank.db", filepath);
            }
        }
        Debug.Log("DB 생성");
        DBConnectionCheck();
    }
    public void DataInsert(string query)
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();

        dbCommand.CommandText = query;
        dbCommand.ExecuteNonQuery();

        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }
}
