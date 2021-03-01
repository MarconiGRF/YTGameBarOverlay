using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
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
            public const string ThumbnailUrl = "thumbnail_url";
            public const string Type = "type";
            public const string Timestamp = "timestamp";
        }

        private const string TableName = "history";

        private const string CreateTableIfNotExists = "CREATE TABLE IF NOT EXISTS " + TableName;
        private const string ColumnIdPrimary = "id INTEGER PRIMARY KEY";
        private const string ColumnTitle = Columns.Title + " TEXT NOT NULL";
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
                    ColumnThumbnailUrl + "," +
                    ColumnType + "," +
                    ColumnTimestamp + ");";

                new SqliteCommand(createTable, database).ExecuteReader();
                database.Close();
            } 
        }

        public void SaveEntry(HistoryEntry entry)
        {
            string insertValues = "INSERT INTO " + TableName + " VALUES " + entry.ToStorable() + ";";

            using (SqliteConnection database = new SqliteConnection($"Filename={DatabasePath}"))
            {
                database.Open();
                new SqliteCommand(insertValues, database).ExecuteReader();
                database.Close();
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
                    string rawEntryData = rows.GetString(0) + "," + rows.GetString(1) + "," + rows.GetString(2) + "," + rows.GetString(3) + "," + rows.GetString(4);
                    entries.Add(HistoryEntry.ofRaw(rawEntryData));
                }
                
                database.Close();
            }

            return entries;
        }
    }
}
