using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;
using System;

public class DatabaseManager
{
    public static DatabaseManager dbm = new DatabaseManager();

    // sql 인스턴스 변수
    IDbConnection dbc;
    IDbCommand dbcm;        //SQL문 작동 개체
    IDataReader dbr;        //반환된 값 읽어주는 객체
    
    /// <summary>
    /// SQL 실행문
    /// </summary>
    /// <param name="sql">SQL 명령어</param>
    public void UpdateData(string sql)
    {
        string path;

        if (Application.platform != RuntimePlatform.Android)
        {
            path = Application.dataPath + "/StreamingAssets/character.db";
        }
        else
        {
            path = Application.persistentDataPath + "/character.db";
            if (!File.Exists(path))
            {
                WWW load = new WWW("jar:file://" + Application.dataPath + "!/assets/" + "character.db");
                while (!load.isDone) { }
                File.WriteAllBytes(path, load.bytes);
            }
        }

        string constr = "URI=file:" + path;

        Debug.Log(sql);

        try
        {
            dbc = new SqliteConnection(constr);
            dbc.Open();
            dbcm = dbc.CreateCommand();

            dbcm.CommandText = sql;
            dbcm.ExecuteNonQuery();

            dbc.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        Disconnect();
    }

    /// <summary>
    /// SQL 검색
    /// </summary>
    /// <param name="sql">SQL 명령어</param>
    /// <returns>검색된 데이터의 객체</returns>
    public IDataReader SelectData(string sql)
    {
        string path;

        if (Application.platform != RuntimePlatform.Android)
        {
            path = Application.dataPath + "/StreamingAssets/character.db";
        }
        else
        {
            path = Application.persistentDataPath + "/character.db";
            if (!File.Exists(path))
            {
                WWW load = new WWW("jar:file://" + Application.dataPath + "!/assets/" + "character.db");
                while (!load.isDone) { }
                File.WriteAllBytes(path, load.bytes);
            }
        }
        string constr = "URI=file:" + path;

        Debug.Log(sql);

        try
        {
            dbc = new SqliteConnection(constr);
            dbc.Open();
            dbcm = dbc.CreateCommand();

            dbcm.CommandText = sql;
            dbr = dbcm.ExecuteReader();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        return dbr;
    }

    /// <summary>
    /// DB 연결 해제
    /// </summary>
    public void Disconnect()
    {
        if (dbr != null)
        {
            dbr.Close();
            dbr = null;
        }
        if (dbcm != null)
        {
            dbcm.Dispose();
            dbcm = null;
        }
        if (dbc != null)
        {
            dbc.Dispose();
            dbc = null;
        }
    }
}
