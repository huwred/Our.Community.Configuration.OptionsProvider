using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using Effortless.Net.Encryption;
using Microsoft.Extensions.Configuration;

namespace Our.Community.Configuration.OptionsProvider.Provider
{
    public class OurConfigurationProvider : ConfigurationProvider
    {
        private readonly string _connectionstring;
        private readonly string _providername;
        private readonly string _cmsid;
        public OurConfigurationProvider(OurConfigurationSource source)
        {
            _connectionstring = source.ConnectionString;
            _providername = source.ProviderName;
            _cmsid = source.CmsId.Replace("-","").Substring(0,16);

        }

        public override void Load()
        {
            Data = _providername.ToLowerInvariant().Contains("sqlite") ? SqliteData() : SqlData();
        }

        private IDictionary<string, string> SqlData()
        {
            
            Dictionary<string, string> values = new Dictionary<string, string>();
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", Microsoft.Data.SqlClient.SqlClientFactory.Instance);
            var connstr = _connectionstring.Replace("|DataDirectory|",Path.Combine(Environment.CurrentDirectory, "umbraco","Data"));
            using var conn = new Microsoft.Data.SqlClient.SqlConnection(connstr);
            var query = new Microsoft.Data.SqlClient.SqlCommand("SELECT [Name],[StringValue],[Group],[IsEncrypted],[Key] FROM OurConfig", conn);

            query.Connection.Open();

            using var reader = query.ExecuteReader();
            while (reader.Read())
            {
                var rawval = reader.GetString(1);
                if (reader.GetBoolean(3))
                {
                    rawval = Strings.Decrypt(rawval, "our.community.config",reader.GetString(4),_cmsid,Bytes.KeySize.Size256,6);
                }
                values.Add(/*reader.GetString(2) + ":" +*/ reader.GetString(0).Replace(" ",""), rawval);
            }

            return values;
        }

        private IDictionary<string, string> SqliteData()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            DbProviderFactories.RegisterFactory("Microsoft.Data.Sqlite", Microsoft.Data.Sqlite.SqliteFactory.Instance);
            var connstr = _connectionstring.Replace("|DataDirectory|",Path.Combine(Environment.CurrentDirectory, "umbraco","Data"));
            using var conn = new Microsoft.Data.Sqlite.SqliteConnection(connstr);
            var query = new Microsoft.Data.Sqlite.SqliteCommand("SELECT [Name],[StringValue],[Group],[IsEncrypted],[Key] FROM OurConfig", conn);

            query.Connection.Open();

            using var reader = query.ExecuteReader();
            while (reader.Read())
            {
                var rawval = reader.GetString(1);
                if (reader.GetBoolean(3))
                {
                    rawval = Strings.Decrypt(rawval, "our.community.config",reader.GetString(4),_cmsid,Bytes.KeySize.Size256,6);
                }
                values.Add(/*reader.GetString(2) + ":" +*/ reader.GetString(0).Replace(" ",""), rawval);
            }

            return values;
        }
    }
}
