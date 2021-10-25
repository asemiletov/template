namespace LementPro.Server.SvcTemplate.Service.Context.ConnectionString
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString(string contextTypeName);

    }
}
