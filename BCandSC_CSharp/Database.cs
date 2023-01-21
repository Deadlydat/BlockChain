using System.Data;
using System.Data.SqlClient;
using System.Numerics;
using System.Threading.Tasks;

namespace BCandSC_CSharp
{
    public class Database
    {
        public SqlConnection conn;

        public Database()
        {
            conn = new SqlConnection();
            conn.ConnectionString = "Data Source=public.emmel-it.de, 1533;User ID=BCandSC;Password=FHWS2022;";
        }
    }
}
