﻿using System;
using Marten.Schema.Sequences;
using Marten.Services;
using Npgsql;

namespace Marten
{
    public class StoreOptions
    {
        private ISerializer _serializer;
        private IConnectionFactory _factory;
        
        public void Connection(string connectionString)
        {
            _factory = new ConnectionFactory(connectionString);
        }

        public void Connection(Func<string> connectionSource)
        {
            _factory = new ConnectionFactory(connectionSource);
        }

        public void Connection(Func<NpgsqlConnection> source)
        {
            _factory = new LambdaConnectionFactory(source);
        }

        public void Serializer(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public void Serializer<T>() where T : ISerializer, new()
        {
            _serializer = new T();
        }
        
        public RequestCounterThreshold RequestCounterThreshold { get; set; } = RequestCounterThreshold.Empty;

        public readonly MartenRegistry Schema = new MartenRegistry();

        public bool AutoCreateSchemaObjects = false;


        public HiloDef HiloSequenceDefaults { get; } = new HiloDef();

        public int UpdateBatchSize { get; set; } = 500;

        internal ISerializer Serializer()
        {
            return _serializer ?? new JsonNetSerializer();
        }

        internal IConnectionFactory ConnectionFactory()
        {
            if (_factory == null) throw new InvalidOperationException("No database connection source is configured");

            return _factory;
        }
    }
}