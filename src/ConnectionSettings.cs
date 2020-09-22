using System;
using System.Data;
using Npgsql;

namespace ConsoleCrudPostgreSQL 
{
    public class ConnectionSettings 
    {
        private string Server { get { return "SERVER"; }}
        private string Port { get { return "5432"; }}
        private string UserId { get { return "USER"; }}
        private string Password { get { return "PASS"; }}
        private string Database { get { return "BASE"; }}
        public string ConnectionString { get; }

        public ConnectionSettings() 
        {
            this.ConnectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", this.Server, this.Port, this.UserId, this.Password, this.Database);
        }
    }
}