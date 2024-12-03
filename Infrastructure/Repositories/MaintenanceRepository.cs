using System.Data;
using Common.dto;
using Dapper;
using FluentResults;

namespace Infrastructure.Repositories;

public class MaintenanceRepository
{
    private readonly IDbConnection _connection;

    public MaintenanceRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<object> GetPlannedMaintenance()
    {
        var results = await _connection.QueryAsync("SELECT * FROM maintenance_plans");
        return results;
    }

    public async Task<Result<object>> GetMaintenance(int id)
    {
        if (!await MaintenanceExists(id))
        {
            return Result.Fail<object>($"Maintenance with id {id} does not exist");
        }

        var result = await _connection.QueryFirstAsync("SELECT * FROM maintenance_plans WHERE id = @id", new { id });
        return Result.Ok(result);
    }

    public async Task<Result<object>> CreateMaintenance(CreateMaintenanceDto request)
    {
        try
        {
            if (!await MechanicExists(request.AssignedTo))
            {
                return Result.Fail("Maintenance guy does not exist.");
            }

            var newId = await _connection.ExecuteAsync(
                "INSERT INTO maintenance_plans (planned_date, mold_id, maintenance_type, description, assigned_to) VALUES (@plannedDate, @moldId, @maintenanceType, @description, @assignedTo); SELECT last_insert_id();",
                new
                {
                    plannedDate = request.PlannedDate,
                    moldId = request.MoldId,
                    maintenanceType = request.MaintenanceType,
                    description = request.Description,
                    assignedTo = request.AssignedTo
                });
            var newMaintenance = await GetMaintenance(newId);
            return Result.Ok(newMaintenance.Value);
        }
        catch (Exception exception)
        {
            return Result.Fail(exception.Message);
        }
    }

    public async Task<Result> UpdateMaintenance(UpdateMaintenanceDto request)
    {
        if (!await MaintenanceExists(request.Id))
        {
            return Result.Fail("Maintenance not found");
        }

        try
        {
            await _connection.ExecuteAsync(
                "REPLACE INTO maintenance_plans (id, planned_date, maintenance_type, description, assigned_to, status) VALUES (@id, @plannedDate, @maintenanceType, @description, @assignedTo, @status)");
            return Result.Ok();
        }
        catch (Exception exception)
        {
            return Result.Fail(exception.Message);
        }
    }

    public async Task<Result> DeleteMaintenance(int id)
    {
        if (!await MaintenanceExists(id))
        {
            return Result.Fail("Maintenance not found");
        }

        await _connection.ExecuteAsync("DELETE FROM maintenance_plans WHERE id = @id", new { id });

        return Result.Ok();
    }

    private async Task<bool> MechanicExists(int id)
    {
        var exists =
            await _connection.QuerySingleAsync<bool>("SELECT EXISTS(SELECT * FROM mechanics WHERE id = @id)",
                new { id });
        return exists;
    }

    private async Task<bool> MaintenanceExists(int id)
    {
        var exists =
            await _connection.QuerySingleAsync<bool>("SELECT EXISTS(SELECT * FROM maintenance_plans WHERE id = @id)",
                new { id });
        return exists;
    }
}