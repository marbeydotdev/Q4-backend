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
}