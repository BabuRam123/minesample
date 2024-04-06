
namespace ApprovalProcess;
public class Employee
{
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public int DepartmentId { get; set; }
}

public class Department
{
    public int DepartmentId { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
}