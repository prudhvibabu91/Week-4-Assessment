using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

// Employee base class to hold common properties
public abstract class Employee
{
    public int EmployeeID { get; set; }
    public string? Name { get; set; }
    public string? ReportingManager { get; set; }
}

// Payroll Employee class inheriting from the base class
public class PayrollEmployee : Employee
{
    public DateTime JoiningDate { get; set; }
    public int Experience { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal DA { get; set; }
    public decimal HRA { get; set; }
    public decimal PF { get; set; }
    public decimal NetSalary { get; set; }

    // Method to calculate net salary based on experience
    public void CalculateNetSalary()
    {
        if (Experience > 10)
        {
            DA = BasicSalary * 0.10m;
            HRA = BasicSalary * 0.085m;
            PF = 6200m;
        }
        else if (Experience > 7 && Experience <= 10)
        {
            DA = BasicSalary * 0.07m;
            HRA = BasicSalary * 0.065m;
            PF = 4100m;
        }
        else if (Experience > 5 && Experience <= 7)
        {
            DA = BasicSalary * 0.041m;
            HRA = BasicSalary * 0.038m;
            PF = 1800m;
        }
        else // Covers exp <= 5
        {
            DA = BasicSalary * 0.019m;
            HRA = BasicSalary * 0.020m;
            PF = 1200m;
        }

        NetSalary = BasicSalary + DA + HRA - PF;
    }
}

// Contract Employee class
public class ContractEmployee : Employee
{
    public DateTime ContractDate { get; set; }
    public int Duration { get; set; } // in months
    public decimal Charges { get; set; }
}

public class EmployeeManager
{
    private readonly string _connectionString = "Server= (localdb)\\MSSQLLocalDB;Database=CompanyDB;Integrated Security=SSPI;";

    // Method to add a new employee to the Employees table
    public int AddEmployee(string name, string reportingManager, string employeeType)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Employees (Name, ReportingManager, EmployeeType) VALUES (@name, @manager, @type); SELECT SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@manager", reportingManager);
            cmd.Parameters.AddWithValue("@type", employeeType);

            conn.Open();
            int employeeId = Convert.ToInt32(cmd.ExecuteScalar());
            return employeeId;
        }
    }

    // Method to add details for a Payroll Employee
    public void AddPayrollDetails(PayrollEmployee emp)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO PayrollEmployees (EmployeeID, JoiningDate, Experience, BasicSalary, DA, HRA, PF, NetSalary) VALUES (@id, @joining, @exp, @basic, @da, @hra, @pf, @net);";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", emp.EmployeeID);
            cmd.Parameters.AddWithValue("@joining", emp.JoiningDate);
            cmd.Parameters.AddWithValue("@exp", emp.Experience);
            cmd.Parameters.AddWithValue("@basic", emp.BasicSalary);
            cmd.Parameters.AddWithValue("@da", emp.DA);
            cmd.Parameters.AddWithValue("@hra", emp.HRA);
            cmd.Parameters.AddWithValue("@pf", emp.PF);
            cmd.Parameters.AddWithValue("@net", emp.NetSalary);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    // Method to add details for a Contract Employee
    public void AddContractDetails(ContractEmployee emp)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO ContractEmployees (EmployeeID, ContractDate, Duration, Charges) VALUES (@id, @cdate, @duration, @charges);";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", emp.EmployeeID);
            cmd.Parameters.AddWithValue("@cdate", emp.ContractDate);
            cmd.Parameters.AddWithValue("@duration", emp.Duration);
            cmd.Parameters.AddWithValue("@charges", emp.Charges);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    // Method to display all employees with their details
    public void DisplayAllEmployees()
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            Console.WriteLine("--- Payroll Employees ---");
            string payrollQuery = "SELECT e.EmployeeID, e.Name, e.ReportingManager, p.JoiningDate, p.Experience, p.BasicSalary, p.NetSalary FROM Employees e JOIN PayrollEmployees p ON e.EmployeeID = p.EmployeeID;";
            SqlCommand payrollCmd = new SqlCommand(payrollQuery, conn);
            using (SqlDataReader reader = payrollCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["EmployeeID"]}, Name: {reader["Name"]}, Manager: {reader["ReportingManager"]}, " +
                                      $"Joining: {reader["JoiningDate"]:d}, Exp: {reader["Experience"]} years, " +
                                      $"Basic: {reader["BasicSalary"]:C}, Net: {reader["NetSalary"]:C}");
                }
            }
            Console.WriteLine("\n--- Contract Employees ---");
            string contractQuery = "SELECT e.EmployeeID, e.Name, e.ReportingManager, c.ContractDate, c.Duration, c.Charges FROM Employees e JOIN ContractEmployees c ON e.EmployeeID = c.EmployeeID;";
            SqlCommand contractCmd = new SqlCommand(contractQuery, conn);
            using (SqlDataReader reader = contractCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["EmployeeID"]}, Name: {reader["Name"]}, Manager: {reader["ReportingManager"]}, " +
                                      $"Contract Date: {reader["ContractDate"]:d}, Duration: {reader["Duration"]} months, " +
                                      $"Charges: {reader["Charges"]:C}");
                }
            }
        }
    }

    // Method to get the total number of employees
    public int GetTotalEmployeeCount()
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query = "SELECT COUNT(*) FROM Employees;";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        EmployeeManager manager = new EmployeeManager();
        string continueInput = "y";

        // Using a while loop since the number of employees is unknown
        while (continueInput.ToLower() == "y")
        {
            Console.WriteLine("Enter employee type (Payroll/Contract):");
            string type = Console.ReadLine()!;

            Console.WriteLine("Enter Name:");
            string name = Console.ReadLine()!;

            Console.WriteLine("Enter Reporting Manager:");
            string managerName = Console.ReadLine()!;

            int newEmployeeId = manager.AddEmployee(name, managerName, type);

            if (type.ToLower() == "payroll")
            {
                PayrollEmployee payrollEmp = new PayrollEmployee
                {
                    EmployeeID = newEmployeeId,
                    Name = name,
                    ReportingManager = managerName
                };

                Console.WriteLine("Enter Joining Date (YYYY-MM-DD):");
                payrollEmp.JoiningDate = DateTime.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter Experience in years:");
                payrollEmp.Experience = int.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter Basic Salary:");
                payrollEmp.BasicSalary = decimal.Parse(Console.ReadLine()!);

                payrollEmp.CalculateNetSalary();
                manager.AddPayrollDetails(payrollEmp);
            }
            else if (type.ToLower() == "contract")
            {
                ContractEmployee contractEmp = new ContractEmployee
                {
                    EmployeeID = newEmployeeId,
                    Name = name,
                    ReportingManager = managerName
                };

                Console.WriteLine("Enter Contract Date (YYYY-MM-DD):");
                contractEmp.ContractDate = DateTime.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter Contract Duration in months:");
                contractEmp.Duration = int.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter Charges:");
                contractEmp.Charges = decimal.Parse(Console.ReadLine()!);

                manager.AddContractDetails(contractEmp);
            }

            Console.WriteLine("Employee added successfully!");
            Console.WriteLine("Do you want to add another employee? (y/n)");
            continueInput = Console.ReadLine()!;
        }

        Console.WriteLine("\nDisplaying all employees:");
        manager.DisplayAllEmployees();

        int totalCount = manager.GetTotalEmployeeCount();
        Console.WriteLine($"\nTotal number of employees: {totalCount}");

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}