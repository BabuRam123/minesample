using Microsoft.AspNetCore.Mvc;

namespace ApprovalProcess.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
     private readonly StoredProcedureExecutorService _StoredProcedureExecutorService;
private readonly ILogger<EmployeeController> _logger;

   
    public EmployeeController(ILogger<EmployeeController> logger)
    {
         _logger = logger;
        _StoredProcedureExecutorService = new StoredProcedureExecutorService();
    }

        [HttpGet("EmployeeByUserID")]
        public IActionResult GetData(int userid)
        {
            try
            {
                 var parameters = new Dictionary<string, object>
        {
            
            { "@userid", userid }
            // Add other parameters as needed
        };
                var (employees, departments) = _StoredProcedureExecutorService.CallStoredProcedureWithMultipleResults("getEmployeesByUser",parameters);

                if (employees.Count == 0)
                {
                    return NotFound("No employees found.");
                }

                if (departments.Count == 0)
                {
                    return NotFound("No departments found.");
                }

                return Ok(new { Employees = employees, Departments = departments });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    


//     [HttpGet("AllEmployee")]
//     public IActionResult Get()
//     {
//         var results =_StoredProcedureExecutorService.ExecuteStoredProc("getEmployees", MapFunction);
//         return Ok(results);
//     }

//      [HttpGet("EmployeeByName")]
//     public IActionResult GetEmployeeByName(string name)
//     {
//         var parameters = new Dictionary<string, object>
//         {
            
//             { "@empname", name }
//             // Add other parameters as needed
//         };
//         var results =_StoredProcedureExecutorService.ExecuteStoredProcWithParams("getEmployeesByName", MapFunction,parameters);
//         return Ok(results);
//     }

// [HttpPost("InsertEmployee")]
//    public IActionResult Post([FromBody] Employee data)
//     {
//          var parameters = new Dictionary<string, object>
//         {
            
//             { "@empname", data.Id.ToString() },
//             { "@empaddress", data.Name.ToString() }
//             // Add other parameters as needed
//         };
//          var sqlParameters = new List<System.Data.SqlClient.SqlParameter>();
//         foreach (var param in parameters)
//         {
//             sqlParameters.Add(new System.Data.SqlClient.SqlParameter(param.Key, param.Value));
//         }

//         var rowsAffected = _StoredProcedureExecutorService.ExecuteNonQueryStoredProcedure("InsertEmployees", parameters);

//         if (rowsAffected > 0)
//         {
//             return Ok("Data inserted successfully!");
//         }
//         else
//         {
//             return BadRequest("Failed to insert data!");
//         }
//     }

//     private Employee MapFunction(System.Data.IDataReader reader)
//     {
//         // Map data from IDataRecord to YourModel

//         var model = new Employee
//         {
//             Id =  reader["name"].ToString() ,
//             Name = reader["address"].ToString()
//             // Map other properties accordingly
//         };
//         return model;
//     }
}