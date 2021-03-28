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
            public const string Id = "id";
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

        /// <summary>
        /// Initializes the database file, creating one if the file do not exists.
        /// It also will create the history table if it does not exist.
        /// </summary>
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

        /// <summary>
        /// Asynchronally Performs the necessary checks and calls to save a History Entry.
        /// </summary>
        /// <param name="newEntry"></param>
        /// <returns>True if operations were well succeeded, false otherwise.</returns>
        public async Task<bool> SaveEntry(HistoryEntry newEntry)
        {
            HistoryEntry latestSavedValue = GetLatestEntry();
            if (latestSavedValue == null)
            {
                return Save(newEntry);
            }
            else
            {
                if (latestSavedValue.MediaURL != newEntry.MediaURL)
                {
                    return Save(newEntry);
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Saves an HistoryEntry on databse.
        /// If such entry has an id different of 0 it will update its information.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>True if the operation was well succeeded, false otherwise.</returns>
        private bool Save(HistoryEntry entry)
        {
            string query = "";
            query = "INSERT INTO " + TableName + " VALUES " + entry.ToStorable() + ";";
            SqliteConnection database = new SqliteConnection($"Filename={DatabasePath}");

            try
            {
                database.Open();
                
                new SqliteCommand(query, database).ExecuteReader();
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

        /// <summary>
        /// Deletes a HistoryEntry on databse.
        /// </summary>
        /// <param name="id">The id of the entry.</param>
        /// <returns>True if the operation was well succeeded, false otherwise.</returns>
        public async Task<bool> Delete(string id)
        {
            string query = "DELETE FROM " + TableName + " WHERE " + Columns.Id + "=" + id + ";";
            SqliteConnection database = new SqliteConnection($"Filename={DatabasePath}");

            try
            {
                database.Open();

                new SqliteCommand(query, database).ExecuteReader();
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

        /// <summary>
        /// Deletes all history entries stored on database.
        /// </summary>
        public async Task<bool> DeleteAll()
        {
            string deleteAllValues = "DELETE FROM " + TableName + ";";
            SqliteConnection database = new SqliteConnection($"Filename={DatabasePath}");

            try
            {
                database.Open();

                new SqliteCommand(deleteAllValues, database).ExecuteReader();
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

        /// <summary>
        /// Gets the latest entry stored on database.
        /// </summary>
        /// <returns>The latest entry on databse or null if there's no entry.</returns>
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

        /// <summary>
        /// Gets and returns a list of all entries stored on database.
        /// </summary>
        /// <returns>THe list of newly formed HistoryEntry objects with the stored data.</returns>
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
