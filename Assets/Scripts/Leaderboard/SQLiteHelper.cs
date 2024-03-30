﻿using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;
using System;
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
    public string UpdateScores(string currentMap, string username, int timescore)
    {
        string dbName = "unitygame.db";
        string dbPath = "URI=file:" + Path.Combine(Application.dataPath, "Scripts", "Leaderboard", dbName);
        using (IDbConnection dbConnection = new SqliteConnection(dbPath))
        {
            dbConnection.Open();
            int count = 0;
            using (IDbTransaction dbTransaction = dbConnection.BeginTransaction())
            {
                using (IDbCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.Transaction = dbTransaction;

                    // Use parameterized query for safety
                    dbCommand.CommandText = $"SELECT * FROM {currentMap} WHERE username = @username";
                    var usernameParam = dbCommand.CreateParameter();
                    usernameParam.ParameterName = "@username";
                    usernameParam.Value = username;
                    dbCommand.Parameters.Add(usernameParam);

                    // Check existence
                    var dataReader = dbCommand.ExecuteReader(); // 17
                    while (dataReader.Read())
                    {
                        count++;
                    }
                    dataReader.Close();

                    if (count == 0)
                    {
                        // Insert
                        dbCommand.CommandText = $"INSERT INTO {currentMap} (username, time_score) VALUES (@username, @timescore)";
                        Debug.Log("Inserted "+ username + " with score" + timescore);
                    }
                    else
                    {
                        // Update
                        dbCommand.CommandText = $"UPDATE {currentMap} SET time_score = @timescore WHERE username = @username AND time_score > @timescore";
                        Debug.Log("Updated "+ username + " with score" + timescore);

                    }

                    // Common parameters for both queries
                    dbCommand.Parameters.Add(usernameParam); // Reuse the username parameter
                    var timescoreParam = dbCommand.CreateParameter();
                    timescoreParam.ParameterName = "@timescore";
                    timescoreParam.Value = timescore;
                    dbCommand.Parameters.Add(timescoreParam);

                    dbCommand.ExecuteNonQuery();
                }
                dbTransaction.Commit();
            }
        }


        return "Success";
    }
    public static void InsertPlayer(string username)
    {
        string dbName = "unitygame.db";
        string dbPath = "URI=file:" + Path.Combine(Application.dataPath, "Scripts", "Leaderboard", dbName);
        using (IDbConnection dbConnection = new SqliteConnection(dbPath))
        {
            dbConnection.Open();
            int count = 0;
            using (IDbTransaction dbTransaction = dbConnection.BeginTransaction())
            {
                using (IDbCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.Transaction = dbTransaction;

                    // Use parameterized query for safety
                    dbCommand.CommandText = $"SELECT * FROM players WHERE username = @username";
                    var usernameParam = dbCommand.CreateParameter();
                    usernameParam.ParameterName = "@username";
                    usernameParam.Value = username;
                    dbCommand.Parameters.Add(usernameParam);

                    // Check existence
                    var dataReader = dbCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        count++;
                    }
                    dataReader.Close();

                    if (count == 0)
                    {
                        // Insert
                        dbCommand.CommandText = $"INSERT INTO players (username) VALUES (@username)";
                        Debug.Log("Inserted "+ username);
                    }
                    
                    dbCommand.ExecuteNonQuery();
                }
                dbTransaction.Commit();
            }
        }
    }
    // USE FOR MONEY UPDATING WHEN BUYING AS WELL.
    public static void UpdateCoin(string username, int coins)
    {
        string dbName = "unitygame.db";
        string dbPath = "URI=file:" + Path.Combine(Application.dataPath, "Scripts", "Leaderboard", dbName);
        using (IDbConnection dbConnection = new SqliteConnection(dbPath))
        {
            dbConnection.Open();
            int count = 0;
            using (IDbTransaction dbTransaction = dbConnection.BeginTransaction())
            {
                using (IDbCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.Transaction = dbTransaction;

                    // Use parameterized query for safety
                    dbCommand.CommandText = $"SELECT * FROM player_currency WHERE username = @username";
                    var usernameParam = dbCommand.CreateParameter();
                    usernameParam.ParameterName = "@username";
                    usernameParam.Value = username;
                    dbCommand.Parameters.Add(usernameParam);
                    int currency = 0;

                    // Check existence
                    var dataReader = dbCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        count++;
                        currency = dataReader.GetInt32(dataReader.GetOrdinal("currency"));
                    }
                    dataReader.Close();

                    if (count == 1)
                    {
                        // update
                        dbCommand.CommandText = $"UPDATE player_currency SET currency = @currency WHERE username = @username";
                        Debug.Log("Updated "+ username);
                    }
                    var currencyParam = dbCommand.CreateParameter();
                    currencyParam.ParameterName = "@currency";
                    currencyParam.Value = currency + coins;
                    dbCommand.Parameters.Add(currencyParam);
                    dbCommand.ExecuteNonQuery();
                }
                dbTransaction.Commit();
            }
        }
    }

    
}