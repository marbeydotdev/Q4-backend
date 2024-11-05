using System.Data;
using Dapper;

namespace Infrastructure.Repositories;

public class MoldRepository
{
    private readonly IDbConnection _connection;

    public MoldRepository(IDbConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<Object> GetMoldHistoryAsync(int skip, int limit)
    {
        var moldHistory = await _connection.QueryAsync(
            "SELECT timestamp(start_date, start_time) as start, timestamp(end_date, end_time) as end, treeview_id as mold_1_id, treeview2_id as mold_2_id, t1.naam as mold_1_name, t1.omschrijving as mold_1_description, t2.naam as mold_2_name, t2.omschrijving as mold_2_description FROM production_data LEFT JOIN treeview t1 ON t1.id = production_data.treeview_id LEFT JOIN treeview t2 ON t2.id = treeview2_id LIMIT @skip,@limit", new {skip, limit});

        return moldHistory;
    }
    
    public async Task<Object> GetMoldHistoryAsync(int skip, int limit, int moldId)
    {
        var moldHistory = await _connection.QueryAsync(
            "SELECT timestamp(start_date, start_time) as start, timestamp(end_date, end_time) as end, treeview_id as mold_1_id, treeview2_id as mold_2_id, t1.naam as mold_1_name, t1.omschrijving as mold_1_description, t2.naam as mold_2_name, t2.omschrijving as mold_2_description FROM production_data LEFT JOIN treeview t1 ON t1.id = production_data.treeview_id LEFT JOIN treeview t2 ON t2.id = treeview2_id WHERE t1.id = @moldId OR t2.id = @moldId LIMIT @skip,@limit", new {skip, limit, moldId});

        return moldHistory;
    }

    public async Task<Object> GetMolds(int skip, int limit)
    {
        var molds = await _connection.QueryAsync("SELECT t.naam, t.omschrijving, p.treeview_id as id, m.id as current_machine_id, m.name as current_machine_name \nFROM production_data p \nLEFT JOIN treeview t ON p.treeview_id = t.id \nLEFT JOIN machine_monitoring_poorten m \n    ON m.board = p.board \n    AND m.port = p.port \n    AND m.id = (SELECT MAX(pp.id) FROM machine_monitoring_poorten pp WHERE pp.port = p.port AND pp.board = p.board)\n    \nUNION ALL \n\nSELECT t.naam, t.omschrijving, p.treeview2_id as id, m.id as current_machine_id, m.name as current_machine_name \nFROM production_data p \nLEFT JOIN treeview t ON p.treeview2_id = t.id \nLEFT JOIN machine_monitoring_poorten m \n    ON m.board = p.board \n    AND m.port = p.port \n    AND m.id = (SELECT MAX(pp.id) FROM machine_monitoring_poorten pp WHERE pp.port = p.port AND pp.board = p.board)\n    \nLIMIT @skip, @limit;", new {skip, limit});
        return molds;
    }
}