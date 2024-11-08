using Microsoft.Extensions.Logging;
using Npgsql;
using Tdev702.Contracts.Exceptions;

namespace Tdev702.Repository.Utils;

public interface INpgsqlExceptionHandler
{
    Exception HandleException(Exception ex, string sql, object? parameters, long elapsedMs);
}

public class NpgsqlExceptionHandler : INpgsqlExceptionHandler
{
    private readonly ILogger<NpgsqlExceptionHandler> _logger;

    public NpgsqlExceptionHandler(ILogger<NpgsqlExceptionHandler> logger)
    {
        _logger = logger;
    }

    public Exception HandleException(Exception ex, string sql, object? parameters, long elapsedMs)
    {
        return ex switch
        {
            PostgresException pgEx => HandlePostgresException(pgEx, sql, parameters, elapsedMs),
            NpgsqlException npgEx => HandleNpgsqlException(npgEx, sql, parameters, elapsedMs),
            _ => HandleUnexpectedException(ex, sql, parameters, elapsedMs)
        };
    }

    private Exception HandlePostgresException(PostgresException pgEx, string sql, object? parameters, long elapsedMs)
    {
        return pgEx.SqlState switch
        {
            // Class 23 — Integrity Constraint Violation
            "23505" => HandleUniqueViolation(pgEx, sql, elapsedMs),
            "23503" => HandleForeignKeyViolation(pgEx, sql, elapsedMs),
            "23502" => HandleNotNullViolation(pgEx, sql, elapsedMs),
            "23514" => HandleCheckViolation(pgEx, sql, elapsedMs),
            "23P01" => HandleExclusionViolation(pgEx, sql, elapsedMs),

            // Class 42 — Syntax Error or Access Rule Violation
            "42P01" => HandleUndefinedTable(pgEx, sql, elapsedMs),
            "42703" => HandleUndefinedColumn(pgEx, sql, elapsedMs),
            "42P02" => HandleUndefinedParameter(pgEx, sql, elapsedMs),
            "42883" => HandleUndefinedFunction(pgEx, sql, elapsedMs),
            "42P03" => HandleDuplicateCursor(pgEx, sql, elapsedMs),
            "42P04" => HandleDuplicateDatabase(pgEx, sql, elapsedMs),
            "42723" => HandleDuplicateColumn(pgEx, sql, elapsedMs),
            "42P05" => HandleDuplicateSchema(pgEx, sql, elapsedMs),
            "42P06" => HandleDuplicateTable(pgEx, sql, elapsedMs),
            "42P07" => HandleDuplicateAlias(pgEx, sql, elapsedMs),
            "42601" => HandleSyntaxError(pgEx, sql, elapsedMs),
            "42501" => HandleInsufficientPrivilege(pgEx, sql, elapsedMs),

            // Class 53 — Insufficient Resources
            "53300" => HandleTooManyConnections(pgEx, sql, elapsedMs),
            "53400" => HandleConfigurationLimitExceeded(pgEx, sql, elapsedMs),

            // Class 08 — Connection Exception
            "08000" => HandleConnectionException(pgEx, sql, elapsedMs),
            "08003" => HandleConnectionDoesNotExist(pgEx, sql, elapsedMs),
            "08006" => HandleConnectionFailure(pgEx, sql, elapsedMs),
            "08001" => HandleSqlClientUnableToEstablish(pgEx, sql, elapsedMs),
            "08004" => HandleRejectedConnection(pgEx, sql, elapsedMs),

            // Class 22 — Data Exception
            "22000" => HandleDataException(pgEx, sql, elapsedMs),
            "22001" => HandleStringDataRightTrunc(pgEx, sql, elapsedMs),
            "22003" => HandleNumericValueOutOfRange(pgEx, sql, elapsedMs),
            "22004" => HandleNullValueNotAllowed(pgEx, sql, elapsedMs),
            "22005" => HandleErrorInAssignment(pgEx, sql, elapsedMs),
            "22007" => HandleInvalidDatetimeFormat(pgEx, sql, elapsedMs),
            "22008" => HandleDatetimeFieldOverflow(pgEx, sql, elapsedMs),
            "22012" => HandleDivisionByZero(pgEx, sql, elapsedMs),
            "22026" => HandleStringDataLengthMismatch(pgEx, sql, elapsedMs),

            // Default case for unhandled PostgreSQL errors
            _ => HandleGenericPostgresError(pgEx, sql, parameters, elapsedMs)
        };
    }

    private Exception HandleUniqueViolation(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogWarning(ex,
            "Unique constraint violation after {ElapsedMilliseconds}ms. Constraint: {Constraint}. SQL: {Sql}",
            elapsedMs, ex.ConstraintName, sql);
        return new DatabaseUniqueViolationException(
            "A record with this key already exists.",
            ex.ConstraintName ?? string.Empty,
            ex);
    }

    private Exception HandleForeignKeyViolation(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogWarning(ex,
            "Foreign key constraint violation after {ElapsedMilliseconds}ms. Constraint: {Constraint}. SQL: {Sql}",
            elapsedMs, ex.ConstraintName, sql);
        return new DatabaseForeignKeyViolationException(
            "Referenced record does not exist.",
            ex.ConstraintName ?? string.Empty,
            ex);
    }

    private Exception HandleNotNullViolation(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Not null constraint violation after {ElapsedMilliseconds}ms. Column: {Column}. SQL: {Sql}",
            elapsedMs, ex.ColumnName, sql);
        return new DatabaseNotNullViolationException(
            "Required field cannot be null.",
            ex.ColumnName ?? string.Empty,
            ex);
    }

    private Exception HandleCheckViolation(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Check constraint violation after {ElapsedMilliseconds}ms. Constraint: {Constraint}. SQL: {Sql}",
            elapsedMs, ex.ConstraintName, sql);
        return new DatabaseCheckViolationException(
            "Data validation constraint violated.",
            ex.ConstraintName ?? string.Empty,
            ex);
    }

    private Exception HandleExclusionViolation(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Exclusion constraint violation after {ElapsedMilliseconds}ms. Constraint: {Constraint}. SQL: {Sql}",
            elapsedMs, ex.ConstraintName, sql);
        return new DatabaseExclusionViolationException(
            "Exclusion constraint violated.",
            ex.ConstraintName ?? string.Empty,
            ex);
    }

    private Exception HandleUndefinedTable(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Table not found error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseObjectNotFoundException(
            "The specified table does not exist.",
            DatabaseObjectType.Table,
            ex);
    }

    private Exception HandleUndefinedColumn(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Column not found error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseObjectNotFoundException(
            "The specified column does not exist.",
            DatabaseObjectType.Column,
            ex);
    }

    private Exception HandleUndefinedParameter(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Parameter not found error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseObjectNotFoundException(
            "The specified parameter does not exist.",
            DatabaseObjectType.Parameter,
            ex);
    }

    private Exception HandleUndefinedFunction(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Function not found error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseObjectNotFoundException(
            "The specified function does not exist.",
            DatabaseObjectType.Function,
            ex);
    }

    private Exception HandleDuplicateCursor(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Duplicate cursor error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseDuplicateObjectException(
            "A cursor with this name already exists.",
            DatabaseObjectType.Cursor,
            ex);
    }

    private Exception HandleDuplicateDatabase(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Duplicate database error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseDuplicateObjectException(
            "A database with this name already exists.",
            DatabaseObjectType.Database,
            ex);
    }

    private Exception HandleDuplicateColumn(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Duplicate column error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseDuplicateObjectException(
            "A column with this name already exists.",
            DatabaseObjectType.Column,
            ex);
    }

    private Exception HandleDuplicateSchema(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Duplicate schema error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseDuplicateObjectException(
            "A schema with this name already exists.",
            DatabaseObjectType.Schema,
            ex);
    }

    private Exception HandleDuplicateTable(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Duplicate table error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseDuplicateObjectException(
            "A table with this name already exists.",
            DatabaseObjectType.Table,
            ex);
    }

    private Exception HandleDuplicateAlias(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Duplicate alias error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseDuplicateObjectException(
            "An alias with this name already exists.",
            DatabaseObjectType.Alias,
            ex);
    }

    private Exception HandleSyntaxError(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "SQL syntax error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseSyntaxException(
            "SQL syntax error occurred.",
            ex);
    }

    private Exception HandleInsufficientPrivilege(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Insufficient privilege error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabasePermissionException(
            "Insufficient database privileges.",
            ex);
    }

    private Exception HandleTooManyConnections(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogCritical(ex,
            "Too many database connections after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseConnectionException(
            "Database connection limit reached.",
            DatabaseConnectionError.TooManyConnections,
            ex);
    }

    private Exception HandleConfigurationLimitExceeded(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogCritical(ex,
            "Configuration limit exceeded after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseConfigurationException(
            "Database configuration limit exceeded.",
            ex);
    }

    private Exception HandleConnectionException(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "General connection error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseConnectionException(
            "Database connection error occurred.",
            DatabaseConnectionError.General,
            ex);
    }

    private Exception HandleConnectionDoesNotExist(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Connection does not exist error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseConnectionException(
            "Database connection does not exist.",
            DatabaseConnectionError.ConnectionDoesNotExist,
            ex);
    }

    private Exception HandleConnectionFailure(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Connection failure after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseConnectionException(
            "Database connection failure occurred.",
            DatabaseConnectionError.ConnectionFailure,
            ex);
    }

    private Exception HandleSqlClientUnableToEstablish(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Unable to establish connection after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseConnectionException(
            "Unable to establish database connection.",
            DatabaseConnectionError.UnableToEstablish,
            ex);
    }

    private Exception HandleRejectedConnection(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Connection rejected after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseConnectionException(
            "Database connection rejected.",
            DatabaseConnectionError.Rejected,
            ex);
    }

    private Exception HandleDataException(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Data error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseDataException(
            "Data error occurred.",
            ex);
    }

    private Exception HandleStringDataRightTrunc(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "String data truncation error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseDataException(
            "String data was truncated.",
            ex);
    }

    private Exception HandleNumericValueOutOfRange(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Numeric value out of range error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseDataException(
            "Numeric value is out of range.",
            ex);
    }

    private Exception HandleNullValueNotAllowed(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Null value not allowed error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseDataException(
            "Null value is not allowed in this context.",
            ex);
    }

    private Exception HandleErrorInAssignment(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Error in assignment after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseDataException(
            "Error occurred during value assignment.",
            ex);
    }

    private Exception HandleInvalidDatetimeFormat(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Invalid datetime format error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseDataException(
            "Invalid datetime format.",
            ex);
    }

    private Exception HandleDatetimeFieldOverflow(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Datetime field overflow error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseDataException(
            "Datetime field overflow occurred.",
            ex);
    }

    private Exception HandleDivisionByZero(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "Division by zero error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseDataException(
            "Division by zero occurred.",
            ex);
    }

    private Exception HandleStringDataLengthMismatch(PostgresException ex, string sql, long elapsedMs)
    {
        _logger.LogError(ex,
            "String data length mismatch error after {ElapsedMilliseconds}ms. SQL: {Sql}",
            elapsedMs, sql);
        return new DatabaseDataException(
            "String data length mismatch.",
            ex);
    }

    private Exception HandleGenericPostgresError(PostgresException ex, string sql, object? parameters, long elapsedMs)
    {
        _logger.LogError(ex,
            "Unhandled PostgreSQL error ({SqlState}) after {ElapsedMilliseconds}ms. SQL: {Sql}, Parameters: {params}",
            ex.SqlState, elapsedMs, sql, parameters);
        return new DatabaseException(
            $"Database error: {ex.MessageText}",
            ex);
    }

    private Exception HandleNpgsqlException(NpgsqlException ex, string sql, object? parameters, long elapsedMs)
    {
        if (ex.InnerException is TimeoutException)
        {
            _logger.LogError(ex,
                "Database timeout after {ElapsedMilliseconds}ms. SQL: {Sql}, Parameters: {params}",
                elapsedMs, sql, parameters);
            return new DatabaseTimeoutException(
                "Database operation timed out.",
                ex);
        }

        _logger.LogError(ex,
            "Npgsql error after {ElapsedMilliseconds}ms. SQL: {Sql}, Parameters: {params}",
            elapsedMs, sql, parameters);
        return new DatabaseConnectionException(
            "Database connection or communication error occurred.",
            DatabaseConnectionError.Communication,
            ex);
    }

    private Exception HandleUnexpectedException(Exception ex, string sql, object? parameters, long elapsedMs)
    {
        _logger.LogError(ex,
            "Unexpected error after {ElapsedMilliseconds}ms. SQL: {Sql}, Parameters: {params}",
            elapsedMs, sql, parameters);
        return new DatabaseException(
            "An unexpected database error occurred.",
            ex);
    }
}