using System;
using Microsoft.Extensions.Options;

namespace LementPro.Server.SvcTemplate.Service.Context.ConnectionString
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly ConnectionStringsSettings _connectionStrings;

        public ConnectionStringProvider(IOptions<ConnectionStringsSettings> settings)
        {
            _connectionStrings = settings.Value;
        }
        
        public string GetConnectionString(string contextTypeName)
        {
            if(!_connectionStrings.ContainsKey(contextTypeName))
                throw new ArgumentException($"ConnectionString name '{contextTypeName}' not registered!");
            
            return _connectionStrings[contextTypeName];
        }
    }
}
