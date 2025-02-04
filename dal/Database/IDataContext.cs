using Microsoft.Data.SqlClient;

namespace apiExo.dal.database;

public interface IDataContext {
    SqlConnection CreateConnection();
}