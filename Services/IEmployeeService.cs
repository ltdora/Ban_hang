using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Hiển thị toàn bộ danh sách nhân viên
        /// </summary>
        /// <returns>Danh sách toàn bộ nhân viên dưới dạng List</returns>
        List<Employees> GetEmployeesList();

        /// <summary>
        /// Hiển thị thông tin chi tiết của một nhân viên theo mã nhân viên EmployeeID
        /// </summary>
        /// <param name="employeeID">Mã nhân viên</param>
        /// <returns>Thông tin nhân viên dưới dạng Model Employees</returns>
        Employees GetEmployeeDetailsById(int employeeID);

        /// <summary>
        /// Thêm mới hoặc cập nhật thông tin nhân viên
        /// </summary>
        /// <param name="employeeModel">Một object kiểu Model Employees</param>
        /// <returns>Dữ liệu kiểu BaseRespone cho biết công việc đã thực hiện</returns>
        BaseRespone<Employees> SaveEmployee(Employees employeeModel);

        /// <summary>
        /// Xóa một nhân viên
        /// </summary>
        /// <param name="employeeID">Mã nhân viên</param>
        /// <returns>Dữ liệu kiểu BaseRespone cho biết công việc đã thực hiện</returns>
        BaseRespone<Employees> DeleteEmployee(int employeeID);
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
            List<Employees> lstEmployee;
            try
            {
                lstEmployee = _context.Set<Employees>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return lstEmployee;
        }
        public Employees GetEmployeeDetailsById(int employeeID)
        {
            Employees employee;
            try
            {
                employee = _context.Find<Employees>(employeeID);
            }
            catch (Exception)
            {
                throw;
            }
            return employee;
        }
        public BaseRespone<Employees> SaveEmployee(Employees employeeModel)
        {
            BaseRespone<Employees> response = new BaseRespone<Employees>();
            try
            {
                // Kiểm tra người dùng theo ID cùa thông tin truyền vào
                Employees _temp = GetEmployeeDetailsById(employeeModel.EmployeeId);
                if (_temp != null)
                {
                    _temp.EmployeeName = employeeModel.EmployeeName;
                    _temp.EmployeePassword = employeeModel.EmployeePassword;
                    _context.Update<Employees>(_temp);
                    response.Message = "Cập nhật thông tin nhân viên thành công";
                }
                else
                {
                    _context.Add<Employees>(employeeModel);
                    response.Message = "Thêm nhân viên thành công";
                }
                _context.SaveChanges();
                response.Data = employeeModel;
                response.Type = "Success";
            }
            catch (Exception ex)
            {
                response.Type = "Error";
                response.Message = "Error : " + ex.Message;
            }
            return response;
        }
        public BaseRespone<Employees> DeleteEmployee(int employeeID)
        {
            BaseRespone<Employees> response = new BaseRespone<Employees>();
            try
            {
                Employees _temp = GetEmployeeDetailsById(employeeID);
                if (_temp != null)
                {
                    _context.Remove<Employees>(_temp);
                    _context.SaveChanges();
                    response.Type = "Success";
                    response.Message = "Xoá nhân viên thành công";
                }
                else
                {
                    response.Type = "Success";
                    response.Message = "Không tìm thấy nhân viên";
                }
            }
            catch (Exception ex)
            {
                response.Type = "Error";
                response.Message = "Error : " + ex.Message;
            }
            return response;
        }
    }
}
