using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using YoutubeGameBarWidget.Pages.PageObjects;

namespace YoutubeGameBarWidget.Utilities
{
    /// <summary>
    /// A class to storage, retrieve and display data for the application general purposes.
    /// </summary>
    class Cabinet
    {
        private class Columns
        {
            public const string Title = "title";
            public const string Channel = "channel";
            public const string MediaUrl = "media_url";
            public const string ThumbnailUrl = "thumbnail_url";
            public const string Type = "type";
            public const string Timestamp = "timestamp";
        }

        private const string TableName = "history";

        private const string CreateTableIfNotExists = "CREATE TABLE IF NOT EXISTS " + TableName;
        private const string ColumnIdPrimary = "id INTEGER PRIMARY KEY";
        private const string ColumnTitle = Columns.Title + " TEXT NOT NULL";
        private const string ColumnChannel = Columns.Channel + " TEXT NOT NULL";
        private const string ColumnMediaUrl = Columns.MediaUrl + " TEXT NOT NULL";
        private const string ColumnThumbnailUrl = Columns.ThumbnailUrl + " TEXT NOT NULL";
        private const string ColumnType = Columns.Type + " TEXT NOT NULL";
        private const string ColumnTimestamp = Columns.Timestamp + " TEXT NOT NULL";
        private const string DatabaseFilename = "history.db";

        private string DatabasePath;

        public Cabinet() 
        {
            DatabasePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DatabaseFilename);
        }

        public async void Initialize()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync(DatabaseFilename, CreationCollisionOption.OpenIfExists);

            using (SqliteConnection database = new SqliteConnection($"Filename={DatabasePath}"))
            {
                database.Open();

                string createTable = CreateTableIfNotExists +
                    "(" + ColumnIdPrimary + "," +
                    ColumnTitle + "," +
                    ColumnChannel + "," +
                    ColumnMediaUrl + "," +
                    ColumnThumbnailUrl + "," +
                    ColumnType + "," +
                    ColumnTimestamp + ");";

                new SqliteCommand(createTable, database).ExecuteReader();
                database.Close();
            } 
        }

        public async Task<bool> SaveEntry(HistoryEntry newEntry)
        {
            HistoryEntry latestSavedValue = GetLatestEntry();
            if (latestSavedValue != null)
            {
                if (latestSavedValue.MediaURL == newEntry.MediaURL)
                {
                    return true;
                }
            }

            return InsertEntry(newEntry);
        }

        private bool InsertEntry(HistoryEntry entry)
        {
            string insertValues = "INSERT INTO " + TableName + " VALUES " + entry.ToStorable() + ";";
            SqliteConnection database = new SqliteConnection($"Filename={DatabasePath}");

            try
            {
                database.Open();
                
                new SqliteCommand(insertValues, database).ExecuteReader();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                database.Close();
            }
        }

        private HistoryEntry GetLatestEntry()
        {
            string selectLastValue = "SELECT * FROM " + TableName + " WHERE id = (SELECT MAX(id) FROM " + TableName + ")";
            using (SqliteConnection database = new SqliteConnection($"Filename={DatabasePath}"))
            {
                database.Open();
                
                SqliteDataReader row = new SqliteCommand(selectLastValue, database).ExecuteReader();
                if (!row.HasRows)
                {
                    return null;
                }
                else
                {
                    string rawData = "";
                    while (row.Read())
                    {
                        rawData = row.GetString(0) + "," +
                            row.GetString(1) + "," +
                            row.GetString(2) + "," +
                            row.GetString(3) + "," +
                            row.GetString(4) + "," +
                            row.GetString(5) + "," +
                            row.GetString(6);
                    }
                    return HistoryEntry.OfRaw(rawData);
                }
            }
        }

        public List<HistoryEntry> GetEntries()
        {
            string selectValues = "SELECT * FROM " + TableName + ";";
            List<HistoryEntry> entries = new List<HistoryEntry>();

            using (SqliteConnection database = new SqliteConnection($"Filename={DatabasePath}"))
            {
                database.Open();

                SqliteDataReader rows = new SqliteCommand(selectValues, database).ExecuteReader();
                while(rows.Read())
                {
                    string rawEntryData = rows.GetString(0) + "," + 
                        rows.GetString(1) + "," + 
                        rows.GetString(2) + "," + 
                        rows.GetString(3) + "," + 
                        rows.GetString(4) + "," + 
                        rows.GetString(5) + "," + 
                        rows.GetString(6);
                    HistoryEntry entry = HistoryEntry.OfRaw(rawEntryData);
                    entry.Id = long.Parse(rows.GetString(0));
                    entries.Add(entry);
                }
                
                database.Close();
            }

            return entries;
        }
    }
}
