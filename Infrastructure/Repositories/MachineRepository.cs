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

    public async Task<object> GetAllAsync(int skip, int limit)
    {
        var machines = await _connection.QueryAsync(
            "SELECT id, board, port, name FROM machine_monitoring_poorten WHERE visible = 1 LIMIT @skip,@limit",
            new { skip, limit });
        return machines;
    }

    public async Task<object> GetMachineShotHistoryAsync(int machineId, DateTime from, DateTime to)
    {
        var machineHistory = await _connection.QueryAsync(
            "SELECT md.shot_time as shot_time, md.timestamp as timestamp FROM monitoring_data_202009 md LEFT JOIN machine_monitoring_poorten mmp ON md.board = mmp.board AND md.port = mmp.port AND md.timestamp > @from AND md.timestamp < @to WHERE mmp.id = @machineId LIMIT 200",
            new { machineId, from, to });
        return machineHistory;
    }
}