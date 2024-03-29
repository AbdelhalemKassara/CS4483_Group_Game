using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SQLiteHelper : MonoBehaviour {
    public List<PlayerScore> GetMap(string currentMap)
    {
        string dbName = "unitygame.db";
        string dbPath = "URI=file:" + Path.Combine(Application.dataPath, "Scripts", "Leaderboard", dbName);
        List<PlayerScore> playerScores = new List<PlayerScore>();
        // Read all values from the table.
        using (IDbConnection dbConnection = new SqliteConnection(dbPath))
        {
            dbConnection.Open();
            using (IDbCommand dbReadCommand = dbConnection.CreateCommand())
            {
                // Assuming currentMap is a valid table name. Be cautious with dynamic table names.
                dbReadCommand.CommandText = "SELECT * FROM "+currentMap + " ORDER BY time_score ASC LIMIT 8";
                IDataReader dataReader = dbReadCommand.ExecuteReader(); // 17
                while (dataReader.Read())
                {
                    // Assuming your table has 'username' and 'time_score' columns
                    string username = dataReader.GetString(dataReader.GetOrdinal("username"));
                    int timeScore = dataReader.GetInt32(dataReader.GetOrdinal("time_score"));

                    // Create a new PlayerScore object and add it to the list
                    PlayerScore playerScore = new PlayerScore
                    {
                        Username = username,
                        TimeScore = timeScore
                    };

                    playerScores.Add(playerScore);
                }
            }
        }

        return playerScores;
    }

    // private void OnMouseDown()
    // {
    //     // Insert hits into the table.
    //     IDbConnection dbConnection = CreateAndOpenDatabase();
    //     IDbCommand dbCommandInsertValue = dbConnection.CreateCommand();
    //     dbCommandInsertValue.CommandText = "INSERT OR REPLACE INTO HitCountTableSimple (id, hits) VALUES (0, " + hitCount + ")";
    //     dbCommandInsertValue.ExecuteNonQuery();
    //
    //     // Remember to always close the connection at the end.
    //     dbConnection.Close();
    // }

    
}