using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public interface IEmployeeService
    {
        List<Employees> GetEmployeesList();

        Employees GetEmployeeDetailsById(int empId);

        BaseRespone<Employees> SaveEmployee(Employees employeeModel);

        BaseRespone<Employees> DeleteEmployee(int employeeId);
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
        public BaseRespone<Employees> SaveEmployee(Employees employeeModel)
        {
            BaseRespone<Employees> respone = new BaseRespone<Employees>();
            try
            {
                Employees _temp = GetEmployeeDetailsById(employeeModel.EmployeeId);
                if (_temp != null)
                {
                    _temp.EmployeeName = employeeModel.EmployeeName;
                    _temp.EmployeePassword = employeeModel.EmployeePassword;
                    _context.Update<Employees>(_temp);
                    respone.Message = "Cập nhật thông tin nhân viên thành công";
                }
                else
                {
                    _context.Add<Employees>(employeeModel);
                    respone.Message = "Thêm nhân viên thành công";
                }
                _context.SaveChanges();
                respone.Data = employeeModel;
                respone.Type = "Success";
            }
            catch (Exception ex)
            {
                respone.Type = "Error";
                respone.Message = "Error : " + ex.Message;
            }
            return respone;
        }
        public BaseRespone<Employees> DeleteEmployee(int employeeId)
        {
            BaseRespone<Employees> respone = new BaseRespone<Employees>();
            try
            {
                Employees _temp = GetEmployeeDetailsById(employeeId);
                if (_temp != null)
                {
                    _context.Remove<Employees>(_temp);
                    _context.SaveChanges();
                    respone.Type = "Success";
                    respone.Message = "Xoá nhân viên thành công";
                }
                else
                {
                    respone.Type = "Success";
                    respone.Message = "Không tìm thấy nhân viên";
                }
            }
            catch (Exception ex)
            {
                respone.Type = "Error";
                respone.Message = "Error : " + ex.Message;
            }
            return respone;
        }
    }
}
