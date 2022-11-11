﻿using System.Data;
using System.Data.SqlClient;

namespace EBanking.Connector
{
    public class SqlConnector
    {
        private const string CONNECTION_STRING =
            @"Data Source=DESKTOP-A2R6AE6\SQLEXPRESS;Initial Catalog=EBankingDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        SqlConnection connection;
        SqlTransaction? transaction;
        SqlCommand? command;
        public SqlConnector()
        {
            connection = new SqlConnection(CONNECTION_STRING);
        }
        public async Task StartConnection()
        {
            await connection.OpenAsync();
        }
        public async Task StartTransaction()
        {
            transaction = (SqlTransaction)(await connection.BeginTransactionAsync());
        }
        public void StartCommand()
        {
            command = connection.CreateCommand();
            if(transaction != null)
                command.Transaction = transaction;
        }
        public async Task CommitTransaction()
        {
            if(transaction != null)
                await transaction.CommitAsync();
            transaction = null;
        }
        public async Task RollbackTransaction()
        {
            if(transaction != null)
                await transaction.RollbackAsync();
            transaction = null;
        }
        public async Task EndConnection()
        {
            await connection.CloseAsync();
        }
        public bool IsConnected() => connection.State == ConnectionState.Open;
        public SqlConnection GetConnection() => connection;
        public SqlTransaction GetTransaction() => transaction;
        public SqlCommand GetCommand() => command;
    }
}