using Dapper;
using Microsoft.Data.SqlClient;
using OrderlyACS.Endpoints.Emploees;

namespace OrderlyACS.Infra.Data;

public class QueryAllUsersAndClaimName
{
    private readonly IConfiguration configuration;

    public QueryAllUsersAndClaimName(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IEnumerable<EmployeeResponse> Execute(int page, int rows)
    {
        var db = new SqlConnection(configuration.GetConnectionString("Conexao"));
        var query = @"SELECT Email, ClaimValue as Name
                FROM AspNetUsers u INNER JOIN AspNetUserClaims c
                    on u.id = c.UserId and claimtype = 'Name'
                ORDER BY name
                OFFSET (@page - 1) * @rows ROWS FETCH NEXT @rows ROWS ONLY";

        return db.Query<EmployeeResponse>(query, new { page, rows });
    }
}
