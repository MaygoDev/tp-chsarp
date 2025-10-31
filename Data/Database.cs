using Npgsql;

namespace CarDealer.Data;

public class Database
{
    private readonly NpgsqlConnection _connection;

    public Database(NpgsqlConnection connection)
    {
        _connection = connection;
    }

}