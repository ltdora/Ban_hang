using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public interface IEmployeeService
    {
        /// <summary>
        /// get list of all employees
        /// </summary>
        /// <returns></returns>
        List<Employees> GetEmployeesList();

        /// <summary>
        /// get employee details by employee id
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        Employees GetEmployeeDetailsById(int empId);

        /// <summary>
        ///  add edit employee
        /// </summary>
        /// <param name="employeeModel"></param>
        /// <returns></returns>
        ResponseModel SaveEmployee(Employees employeeModel);


        /// <summary>
        /// delete employees
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        ResponseModel DeleteEmployee(int employeeId);
    }
    public class EmployeeService : IEmployeeService
    {
        private ShopContext _context;
        public EmployeeService(ShopContext context)
        {
            _context = context;
        }

        public List<Employees> GetEmployeesList()
        {
            List<Employees> empList;
            try
            {
                empList = _context.Set<Employees>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return empList;
        }
        public Employees GetEmployeeDetailsById(int empId)
        {
            Employees emp;
            try
            {
                emp = _context.Find<Employees>(empId);
            }
            catch (Exception)
            {
                throw;
            }
            return emp;
        }
        public ResponseModel SaveEmployee(Employees employeeModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                Employees _temp = GetEmployeeDetailsById(employeeModel.EmployeeId);
                if (_temp != null)
                {
                    _temp.EmployeeName = employeeModel.EmployeeName;
                    _temp.EmployeePassword = employeeModel.EmployeePassword;
                    _context.Update<Employees>(_temp);
                    model.Messsage = "Employee Update Successfully";
                }
                else
                {
                    _context.Add<Employees>(employeeModel);
                    model.Messsage = "Employee Inserted Successfully";
                }
                _context.SaveChanges();
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error : " + ex.Message;
            }
            return model;
        }
        public ResponseModel DeleteEmployee(int employeeId)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                Employees _temp = GetEmployeeDetailsById(employeeId);
                if (_temp != null)
                {
                    _context.Remove<Employees>(_temp);
                    _context.SaveChanges();
                    model.IsSuccess = true;
                    model.Messsage = "Employee Deleted Successfully";
                }
                else
                {
                    model.IsSuccess = false;
                    model.Messsage = "Employee Not Found";
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error : " + ex.Message;
            }
            return model;
        }
    }
}
