using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ConsoleCrudPostgreSQL {
    class User 
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }

    // private string _connectionString;  

    // public UserDAL(IConfiguration iconfiguration)  
    // {  
    //     _connectionString = iconfiguration.GetConnectionString("Default");  
    // }

    class Program 
    {
        private static IConfiguration _iconfiguration;
        static void Main (string[] args) 
        {
            Create(new User());
            ReadById(Guid.NewGuid());
            ReadAll();
            Update(new User());
            Delete(Guid.NewGuid());
        }

        // static void GetAppSettingsFile() 
        // {  
        //     var builder = new ConfigurationBuilder()  
        //                 .SetBasePath(Directory.GetCurrentDirectory())  
        //                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);  
        //                 _iconfiguration = builder.Build();  
        // }

        static void Create (User user) {
            NpgsqlConnection pgsqlConnection = null;

            try {
                using (pgsqlConnection = new NpgsqlConnection (new ConnectionSettings ().ConnectionString)) {

                    pgsqlConnection.Open ();

                    string command = String.Format ("insert into \"Users\" (Id, Name, Email) values('{0}','{1}',{2})", user.Id, user.Name, user.Email);

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand (command, pgsqlConnection)) 
                    {
                        pgsqlcommand.ExecuteNonQuery ();
                    }
                }
            } catch (NpgsqlException ex) {
                throw ex;
            } catch (Exception ex) {
                throw ex;
            } finally {
                pgsqlConnection.Close ();
            }
        }

        static DataTable ReadById(Guid id)
        {
            NpgsqlConnection pgsqlConnection = null;
            //List<User> users = new List<User>();
            DataTable dataTable = new DataTable();

            try
            {
                // using(pgsqlConnection = new NpgsqlConnection(_connectionString))
                // {
                //     NpgsqlCommand command = new NpgsqlCommand("SP_CREATE_USER", pgsqlConnection);
                //     command.CommandType = CommandType.StoredProcedure;
                //     pgsqlConnection.Open();

                //     NpgsqlDataReader dataReader = command.ExecuteReader();
                        
                //     while(dataReader.Read())
                //     {
                //         users.Add(new User{
                //             Id = (Guid)dataReader[0],
                //             Name = dataReader[1].ToString(),
                //             Email = dataReader[2].ToString()
                //         });
                //     }
                // }

                using (pgsqlConnection = new NpgsqlConnection(new ConnectionSettings().ConnectionString))
                {
                    pgsqlConnection.Open();

                    string select = "select * from \"User\" where Id = " + id;

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(select, pgsqlConnection))
                    {
                        adapter.Fill(dataTable);
                    }                   
                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pgsqlConnection.Close();
            }

            //return users;
            return dataTable;
        }

        static void ReadAll () {
            NpgsqlConnection pgsqlConnection = null;

            DataTable dataTable = new DataTable ();

            try {
                using (pgsqlConnection = new NpgsqlConnection (new ConnectionSettings ().ConnectionString)) 
                {
                    pgsqlConnection.Open ();
                    string command = "select * from \"Users\" limit 100;";

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter (command, pgsqlConnection)) {
                        adapter.Fill (dataTable);
                    }
                }
            } catch (NpgsqlException ex) {
                throw ex;
            } catch (Exception ex) {
                throw ex;
            } finally {
                pgsqlConnection.Close ();
            }

            foreach (DataRow item in dataTable.Rows) {
                Console.WriteLine (string.Concat ("Id: ", item["Id"].ToString (), " - Name: ", item["FullName"].ToString (), " - Email: ", item["Email"].ToString ()));
            }

            Console.WriteLine ("Press any key to stop.");
        }

        static void Update (User user) {
            NpgsqlConnection pgsqlConnection = null;

            try {
                using (pgsqlConnection = new NpgsqlConnection (new ConnectionSettings ().ConnectionString)) 
                {
                    pgsqlConnection.Open ();

                    string command = String.Format ("update \"Users\" set Name = '" + user.Name + "' , Email = " + user.Email + " where Id = " + user.Id);

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand (command, pgsqlConnection)) {
                        pgsqlcommand.ExecuteNonQuery ();
                    }
                }
            } catch (NpgsqlException ex) {
                throw ex;
            } catch (Exception ex) {
                throw ex;
            } finally {
                pgsqlConnection.Close ();
            }
        }

        static void Delete (Guid id) {
            NpgsqlConnection pgsqlConnection = null;

            try {
                using (pgsqlConnection = new NpgsqlConnection (new ConnectionSettings ().ConnectionString)) 
                {
                    pgsqlConnection.Open ();

                    string command = String.Format ("delete from \"User\" where Id = '{0}'", id);

                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand (command, pgsqlConnection)) {
                        pgsqlcommand.ExecuteNonQuery ();
                    }
                }
            } catch (NpgsqlException ex) {
                throw ex;
            } catch (Exception ex) {
                throw ex;
            } finally {
                pgsqlConnection.Close ();
            }
        }
    }
}