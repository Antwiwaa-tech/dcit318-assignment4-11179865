using System.Configuration;
using System.Data.SqlClient;

public static class Db
{
    public static string ConnString =>
        ConfigurationManager.ConnectionStrings["MedicalDb"].ConnectionString;

    public static SqlConnection GetOpenConnection()
    {
        var conn = new SqlConnection(ConnString);
        conn.Open();
        return conn;
    }
}
