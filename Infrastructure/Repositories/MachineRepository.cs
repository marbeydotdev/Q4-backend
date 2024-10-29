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
        var machines = await _connection.QueryAsync("SELECT id, board, port, name FROM machine_monitoring_poorten");
        return machines;
    }

    public async Task<Object> GetMachineShotHistoryAsync(int board, int port, int skip, int limit)
    {
        var machineHistory = await _connection.QueryAsync("SELECT timestamp, shot_time FROM monitoring_data_202009 WHERE board = @board AND port = @port LIMIT @skip,@limit", new {board, port, skip, limit});
        return machineHistory;
    }

    public async Task<Object> GetMoldHistoryAsync(int board, int port, int skip, int limit)
    {
        var moldHistory = await _connection.QueryAsync(
            "SELECT timestamp(start_date, start_time) as start, timestamp(end_date, end_time) as end, treeview_id as mold_1, treeview2_id as mold_2 FROM production_data");

        return moldHistory;
    }
}