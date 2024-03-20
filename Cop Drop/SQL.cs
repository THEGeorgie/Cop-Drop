    //SQlite
    using System.Data.SQLite;
namespace CopDrop
{
    class SQL
    {
        static void SQLiteInsert(string dbPath, string input)
        {
            using (SQLiteConnection connection = new SQLiteConnection(dbPath))
            {
                connection.Open();

                // Insert data
                using (SQLiteCommand insertData = new SQLiteCommand(
                    "INSERT INTO NOTES(data) VALUES (@dataNotes)", connection))
                {
                    insertData.Parameters.AddWithValue("@dataNotes", input);
                    insertData.ExecuteNonQuery();
                }
                connection.Close();
            }

        }
        static string[] SQLiteSelect(string dbPath, string table, string filter)
        {
            using (SQLiteConnection connection = new SQLiteConnection(dbPath))
            {
                connection.Open();
                int counter = 0;
                string[] bufferMem = null;
                string command;
                if (filter != null)
                {
                    command = $"SELECT * FROM {table} {filter}";
                }
                else
                {
                    command = $"SELECT * FROM {table}";
                }
                using (SQLiteCommand selectData = new SQLiteCommand(
                command, connection))
                {

                    try
                    {
                        using (SQLiteDataReader reader = selectData.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //how much data it will be stored
                                counter++;
                            }
                            bufferMem = new string[counter];

                        }
                        using (SQLiteDataReader reader = selectData.ExecuteReader())
                        {
                            counter = 0;
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);

                                bufferMem[counter] = "id: " + reader.GetString(1);
                                counter++;

                            }
                            connection.Close();
                            return bufferMem;

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return null;
                    }


                }


            }
        }
        static void SQLiteDelete(string dbPath, char input)
        {
            int i = input - 48;
            using (SQLiteConnection connection = new SQLiteConnection(dbPath))
            {
                using (SQLiteCommand command = new SQLiteCommand("DELETE FROM NOTES WHERE id = @idNote ", connection))
                {
                    command.Parameters.AddWithValue("idNote", i);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}