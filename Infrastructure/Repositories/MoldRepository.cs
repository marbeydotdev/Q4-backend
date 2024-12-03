using System.Data;
using Common;
using Common.Models;
using Dapper;

namespace Infrastructure.Repositories;

public class MoldRepository
{
    private readonly IDbConnection _connection;

    public MoldRepository(IDbConnection connection)
    {
        _connection = connection;
    }

public async Task<object> GetMoldHistoryAsync(int skip, int limit)
{
    var moldHistory = await _connection.QueryAsync(
        "SELECT timestamp(start_date, start_time) as start, timestamp(end_date, end_time) as end, treeview_id as mold_1_id, treeview2_id as mold_2_id, t1.naam as mold_1_name, t1.omschrijving as mold_1_description, t2.naam as mold_2_name, t2.omschrijving as mold_2_description FROM production_data LEFT JOIN treeview t1 ON t1.id = production_data.treeview_id LEFT JOIN treeview t2 ON t2.id = treeview2_id LIMIT @skip,@limit",
        new { skip, limit });

    var groupedHistory = moldHistory.GroupBy(m => new { m.start, end = (DateTime?)m.end })
        .Select(g => new
        {
            start = g.Key.start,
            end = g.Key.end,
            durationHours = g.Key.end.HasValue ? (g.Key.end.Value - g.Key.start).TotalHours : (double?)null,
            molds = g.Select(m => new
            {
                id = m.mold_1_id,
                name = m.mold_1_name,
                description = m.mold_1_description
            }).Concat(g.Select(m => new
            {
                id = m.mold_2_id,
                name = m.mold_2_name,
                description = m.mold_2_description
            })).Where(m => m.id != 0).Distinct().ToList()
        });

    return groupedHistory;
}

public async Task<object> GetMoldHistoryAsync(int skip, int limit, int moldId)
{
    var moldHistory = await _connection.QueryAsync(
        "SELECT timestamp(start_date, start_time) as start, timestamp(end_date, end_time) as end, treeview_id as mold_1_id, treeview2_id as mold_2_id, t1.naam as mold_1_name, t1.omschrijving as mold_1_description, t2.naam as mold_2_name, t2.omschrijving as mold_2_description FROM production_data LEFT JOIN treeview t1 ON t1.id = production_data.treeview_id LEFT JOIN treeview t2 ON t2.id = treeview2_id WHERE t1.id = @moldId OR t2.id = @moldId LIMIT @skip,@limit",
        new { skip, limit, moldId });

    var groupedHistory = moldHistory.GroupBy(m => new { m.start, end = (DateTime?)m.end })
        .Select(g => new
        {
            start = g.Key.start,
            end = g.Key.end,
            durationHours = g.Key.end.HasValue ? (g.Key.end.Value - g.Key.start).TotalHours : (double?)null,
            molds = g.Select(m => new
            {
                id = m.mold_1_id,
                name = m.mold_1_name,
                description = m.mold_1_description
            }).Concat(g.Select(m => new
            {
                id = m.mold_2_id,
                name = m.mold_2_name,
                description = m.mold_2_description
            })).Where(m => m.id != 0).Distinct().ToList()
        });

    return groupedHistory;
}

    public async Task<object> GetMolds(string? search, int? skip, int limit)
    {
        var molds = await _connection.QueryAsync(
        "SELECT DISTINCT t.naam         as name, p.board, p.port,                t.omschrijving as description,                 p.treeview_id  as id,                 m.id           as current_machine_id,                 m.name         as current_machine_name FROM production_data p          LEFT JOIN treeview t ON p.treeview_id = t.id          LEFT JOIN (SELECT port, board, MAX(id) as latest_machine_id                     FROM machine_monitoring_poorten                     WHERE visible = 1                     GROUP BY port, board) lm ON lm.port = p.port AND lm.board = p.board          LEFT JOIN machine_monitoring_poorten m ON m.id = lm.latest_machine_id WHERE p.treeview_id != 1   AND p.treeview_id != 154   AND p.treeview_id != 161   AND p.treeview_id != 166  AND (t.naam LIKE '%' || @search || '%' OR t.omschrijving LIKE '%' || @search || '%') UNION ALL  SELECT DISTINCT p.board, p.port, t.naam,                 t.omschrijving,                 p.treeview2_id as id,                 m.id           as current_machine_id,                 m.name         as current_machine_name FROM production_data p          LEFT JOIN treeview t ON p.treeview2_id = t.id          LEFT JOIN (SELECT port, board, MAX(id) as latest_machine_id                     FROM machine_monitoring_poorten                     WHERE visible = 1                       AND id != 1                       AND id != 154                       AND id != 161                       AND id != 166                     GROUP BY port, board) lm ON lm.port = p.port AND lm.board = p.board          LEFT JOIN machine_monitoring_poorten m ON m.id = lm.latest_machine_id WHERE p.treeview_id != 1   AND p.treeview_id != 154   AND p.treeview_id != 161   AND p.treeview_id != 166  AND (t.naam LIKE '%' || @search || '%' OR t.omschrijving LIKE '%' || @search || '%') LIMIT @skip,@limit",            new { skip, limit });
        return molds.DistinctBy(m => m.id).Select(m => new
        {
            m.id,
            m.name,
            m.description,
            m.board,
            m.port,
            health = -1,
            machine = new { id = m.current_machine_id, name = m.current_machine_name }
        });
    }
    
    public async Task<object> GetMoldByIdAsync(int moldId)
    {
        var mold = await _connection.QuerySingleOrDefaultAsync(
            "SELECT t.naam as name, p.board, p.port, t.omschrijving as description, p.treeview_id as id, m.id as current_machine_id, m.name as current_machine_name FROM production_data p LEFT JOIN treeview t ON p.treeview_id = t.id LEFT JOIN (SELECT port, board, MAX(id) as latest_machine_id FROM machine_monitoring_poorten WHERE visible = 1 GROUP BY port, board) lm ON lm.port = p.port AND lm.board = p.board LEFT JOIN machine_monitoring_poorten m ON m.id = lm.latest_machine_id WHERE p.treeview_id = @moldId",
            new { moldId });

        return mold;
    }

    public async Task<List<Shot>> GetMoldShotHistory(int moldId, DateTime lastMaintenance)
    {
        var result = await _connection.QueryAsync<Shot>(
            "SELECT md.shot_time as Duration, md.timestamp as Timestamp FROM monitoring_data_202009 md LEFT JOIN production_data pd ON pd.treeview_id = @moldId WHERE timestamp > @lastMaintenance LIMIT 0,50",
            new { moldId, lastMaintenance });
        return result.ToList();
    }

    public async Task<double> GetMoldHealth(int moldId, DateTime lastMaintenance, double tolerance)
    {
        var shots = await GetMoldShotHistory(moldId, lastMaintenance);

        var median = Utils.GetMedian(shots.Select(s => s.Duration).ToArray());

        double health = 100;

        foreach (var shot in shots)
        {
            if (Math.Abs(median - shot.Duration) > tolerance)
            {
                health -= 1;
            }
        }

        return health;
    }
}