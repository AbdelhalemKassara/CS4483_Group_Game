using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;


public class BuildDatabase : MonoBehaviour
{
    string conn;
    string sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    
    string DATABASE_NAME = "unitygame.db";

    void Start()
    {
        string dbPath = "URI=file:" + Path.Combine(Application.dataPath, "StreamingAssets", DATABASE_NAME);
        conn = dbPath;
        dbconn = new SqliteConnection(conn);
        CreateTable();
    }

    private void CreateTable()
    {
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open();
            dbcmd = dbconn.CreateCommand();
            string sqlCode = @"
                            BEGIN TRANSACTION;
                            CREATE TABLE IF NOT EXISTS players (
                                id    INTEGER NOT NULL UNIQUE,
                                username    TEXT NOT NULL UNIQUE,
                                PRIMARY KEY(id AUTOINCREMENT)
                            );
                            CREATE TABLE IF NOT EXISTS map_1 (
                                username    TEXT NOT NULL UNIQUE,
                                time_score    INTEGER NOT NULL,
                                FOREIGN KEY(username) REFERENCES players(username) ON DELETE CASCADE
                            );
                            CREATE TABLE IF NOT EXISTS map_2 (
                                username    TEXT NOT NULL UNIQUE,
                                time_score    INTEGER NOT NULL,
                                FOREIGN KEY(username) REFERENCES players(username) ON DELETE CASCADE
                            );
                            CREATE TABLE IF NOT EXISTS map_3 (
                                username    TEXT NOT NULL UNIQUE,
                                time_score    INTEGER NOT NULL,
                                FOREIGN KEY(username) REFERENCES players(username) ON DELETE CASCADE
                            );
                            CREATE TABLE IF NOT EXISTS map_4 (
                                username    TEXT NOT NULL UNIQUE,
                                time_score    INTEGER NOT NULL,
                                FOREIGN KEY(username) REFERENCES players(username) ON DELETE CASCADE
                            );
                            CREATE TABLE IF NOT EXISTS map_5 (
                                username    TEXT NOT NULL UNIQUE,
                                time_score    INTEGER NOT NULL,
                                FOREIGN KEY(username) REFERENCES players(username) ON DELETE CASCADE
                            );
                            INSERT INTO players VALUES (1,'bob');
                            INSERT INTO players VALUES (2,'Spongebob');
                            INSERT INTO players VALUES (3,'Patrick');
                            INSERT INTO players VALUES (4,'Squid');
                            INSERT INTO players VALUES (5,'Edwin');
                            INSERT INTO players VALUES (6,'Abdel');
                            INSERT INTO players VALUES (7,'Johnson');
                            INSERT INTO players VALUES (8,'Varnesh');
                            INSERT INTO players VALUES (9,'bobby');
                            INSERT INTO players VALUES (10,'newb');
                            INSERT INTO players VALUES (11,'asd');
                            INSERT INTO players VALUES (26,'asds');
                            INSERT INTO players VALUES (27,'asdassa');
                            INSERT INTO players VALUES (28,'varnesh');
                            INSERT INTO players VALUES (29,'update');
                            INSERT INTO map_1 VALUES ('bob',50);
                            INSERT INTO map_1 VALUES ('Spongebob',420);
                            INSERT INTO map_1 VALUES ('Patrick',262);
                            INSERT INTO map_1 VALUES ('Squid',298);
                            INSERT INTO map_1 VALUES ('Edwin',340);
                            INSERT INTO map_1 VALUES ('Abdel',454);
                            INSERT INTO map_1 VALUES ('Johnson',436);
                            INSERT INTO map_1 VALUES ('Varnesh',278);
                            INSERT INTO map_1 VALUES ('bobby',200);
                            INSERT INTO map_2 VALUES ('Spongebob',385);
                            INSERT INTO map_2 VALUES ('Patrick',383);
                            INSERT INTO map_2 VALUES ('Squid',574);
                            INSERT INTO map_2 VALUES ('Edwin',421);
                            INSERT INTO map_2 VALUES ('Abdel',236);
                            INSERT INTO map_2 VALUES ('Johnson',329);
                            INSERT INTO map_2 VALUES ('Varnesh',421);
                            INSERT INTO map_2 VALUES ('bob',315);
                            INSERT INTO map_2 VALUES ('update',18);
                            INSERT INTO map_3 VALUES ('bob',398);
                            INSERT INTO map_3 VALUES ('Spongebob',471);
                            INSERT INTO map_3 VALUES ('Patrick',505);
                            INSERT INTO map_3 VALUES ('Squid',386);
                            INSERT INTO map_3 VALUES ('Edwin',380);
                            INSERT INTO map_3 VALUES ('Abdel',364);
                            INSERT INTO map_3 VALUES ('Johnson',237);
                            INSERT INTO map_3 VALUES ('Varnesh',409);
                            INSERT INTO map_4 VALUES ('bob',502);
                            INSERT INTO map_4 VALUES ('Spongebob',343);
                            INSERT INTO map_4 VALUES ('Patrick',522);
                            INSERT INTO map_4 VALUES ('Squid',230);
                            INSERT INTO map_4 VALUES ('Edwin',406);
                            INSERT INTO map_4 VALUES ('Abdel',592);
                            INSERT INTO map_4 VALUES ('Johnson',478);
                            INSERT INTO map_4 VALUES ('Varnesh',343);
                            INSERT INTO map_5 VALUES ('bob',193);
                            INSERT INTO map_5 VALUES ('Spongebob',243);
                            INSERT INTO map_5 VALUES ('Patrick',521);
                            INSERT INTO map_5 VALUES ('Squid',321);
                            INSERT INTO map_5 VALUES ('Edwin',391);
                            INSERT INTO map_5 VALUES ('Abdel',433);
                            INSERT INTO map_5 VALUES ('Johnson',337);
                            INSERT INTO map_5 VALUES ('Varnesh',222);
                            COMMIT;
                            ";
            dbcmd.CommandText = sqlCode;
            dbcmd.ExecuteScalar();
            dbconn.Close();
        }
    }
}
