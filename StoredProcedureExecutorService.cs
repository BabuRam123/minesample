using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ApprovalProcess;

public class StoredProcedureExecutorService
{
    private readonly string _connectionString="Integrated Security=SSPI;Initial Catalog=testsampledb;Data Source=HQAPEW1C005-AAZ;";
   

    // public StoredProcedureExecutorService(IConfiguration configuration)
    // {
    //     _connectionString = configuration.GetConnectionString("DB_DEV");
    // }

    // public List<T> ExecuteStoredProc<T>(string storedProcName, Func<IDataReader, T> mapFunction, params SqlParameter[] parameters)
    // {
    //     using (var connection = new SqlConnection (_connectionString))
    //     {
    //         using (var command = new SqlCommand(storedProcName, connection))
    //         {
    //             command.CommandType = CommandType.StoredProcedure;
    //             command.Parameters.AddRange(parameters);

    //             connection.Open();
    //             using (var reader = command.ExecuteReader())
    //             {
    //                 var results = new List<T>();
    //                 while (reader.Read())
    //                 {
    //                     results.Add(mapFunction(reader));
    //                 }
    //                 return results;
    //             }
    //         }
    //     }
    // }

    // public List<T> ExecuteStoredProcWithParams<T>(string storedProcName, Func<IDataReader, T> mapFunction, Dictionary<string, object> parameters)
    // {
    //     var sqlParameters = new List<SqlParameter>();
    //     foreach (var param in parameters)
    //     {
    //         sqlParameters.Add(new SqlParameter(param.Key, param.Value));
    //     }

    //     return ExecuteStoredProc(storedProcName, mapFunction, sqlParameters.ToArray());
    // }

    // public int ExecuteNonQueryStoredProcedure(string storedProcName, Dictionary<string, object> parameters)
    // {
    //     using (var connection = new SqlConnection(_connectionString))
    //     {
    //         using (var command = new SqlCommand(storedProcName, connection))
    //         {
    //             command.CommandType = CommandType.StoredProcedure;
    //     //          var sqlParameters = new List<SqlParameter>();
    //     // foreach (var param in parameters)
    //     // {
    //     //     sqlParameters.Add(new SqlParameter(param.Key, param.Value));
    //     // }
    //     command.Parameters.AddRange(parameters.ToArray());

    //             connection.Open();
    //             return command.ExecuteNonQuery();
    //         }
    //     }
    // }

    public (List<Employee>, List<Department>) CallStoredProcedureWithMultipleResults(string storedProcedureName,Dictionary<string, object> parameters)
        {
            List<Employee> employees = new List<Employee>();
            List<Department> departments = new List<Department>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        if(parameters !=null)
                        {
                             foreach (var param in parameters)
        {
            command.Parameters.AddWithValue(param.Key,param.Value);
        }
                        }

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(MapEmployee(reader));
                                // Check if the current result set is for employees
                                // if (reader.GetName(0).Equals("EmployeeId", StringComparison.OrdinalIgnoreCase))
                                // {
                                //     employees.Add(MapEmployee(reader));
                                // }
                                // Check if the current result set is for departments
                                // else if (reader.GetName(0).Equals("DepartmentId", StringComparison.OrdinalIgnoreCase))
                                // {
                                //     departments.Add(MapDepartment(reader));
                                // }
                                // Add more checks for additional result sets as needed
                            }
                            // Move to the next result set
                             reader.NextResult();
                             while(reader.Read())
                             {
                                departments.Add(MapDepartment(reader));
                             }

                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log or handle the SQL exception
                throw new Exception("Error executing stored procedure", ex);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                throw new Exception("An error occurred", ex);
            }

            return (employees, departments);
        }

        private Employee MapEmployee(IDataRecord record)
        {
            return new Employee
            {
                EmployeeId = Convert.ToInt32(record["EmployeeId"]),
                Name = record["Name"].ToString(),
                DepartmentId = Convert.ToInt32(record["DepartmentId"])
            };
        }

        private Department MapDepartment(IDataRecord record)
        {
            return new Department
            {
                DepartmentId = Convert.ToInt32(record["DepartmentId"]),
                Name = record["Name"].ToString(),
                Location = record["Location"].ToString()
            };
        }
    }


