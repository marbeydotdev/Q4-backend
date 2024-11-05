using System.Data;
using Dapper;

namespace Infrastructure.Repositories;

public class MachineRepository
{
    private readonly IDbConnection _connection;

    public MachineRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Object> GetAllAsync()
    {
        var machines = await _connection.QueryAsync("SELECT id, board, port, name, visible FROM machine_monitoring_poorten");
        return machines;
    }

    public async Task<Object> GetMachineShotHistoryAsync(int board, int port, int skip, int limit)
    {
        var machineHistory = await _connection.QueryAsync("SELECT timestamp, shot_time FROM monitoring_data_202009 WHERE board = @board AND port = @port LIMIT @skip,@limit", new {board, port, skip, limit});
        return machineHistory;
    }
}