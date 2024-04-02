using He_thong_ban_hang;
using System;
using System.Collections.Generic;
using System.Linq;

namespace He_thong_ban_hang
{
    public interface IUserService
    {
        /// <summary>
        /// get list of all Users
        /// </summary>
        /// <returns></returns>
        List<Users> GetUsersList();

        /// <summary>
        /// get User details by User id
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        Users GetUserDetailsById(int empId);

        /// <summary>
        ///  add edit User
        /// </summary>
        /// <param name="UserModel"></param>
        /// <returns></returns>
        ResponseModel SaveUser(Users UserModel);


        /// <summary>
        /// delete Users
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        ResponseModel DeleteUser(int UserId);
    }
    public class UserService : IUserService
    {
        private ShopContext _context;
        public UserService(ShopContext context)
        {
            _context = context;
        }

        public List<Users> GetUsersList()
        {
            List<Users> empList;
            try
            {
                empList = _context.Set<Users>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return empList;
        }
        public Users GetUserDetailsById(int empId)
        {
            Users emp;
            try
            {
                emp = _context.Find<Users>(empId);
            }
            catch (Exception)
            {
                throw;
            }
            return emp;
        }

        public ResponseModel SaveUser(Users UserModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                Users _temp = GetUserDetailsById(UserModel.UserId);
                if (_temp != null)
                {
                    _temp.UserName = UserModel.UserName;
                    _temp.UserPassword = UserModel.UserPassword;
                    _context.Update<Users>(_temp);
                    model.Messsage = "Cập nhật thông tin người dùng thành công";
                }
                else
                {
                    _context.Add<Users>(UserModel);
                    model.Messsage = "Thêm người dùng thành công";
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
        public ResponseModel DeleteUser(int UserId)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                Users _temp = GetUserDetailsById(UserId);
                if (_temp != null)
                {
                    _context.Remove<Users>(_temp);
                    _context.SaveChanges();
                    model.IsSuccess = true;
                    model.Messsage = "Xoá người dùng thành công";
                }
                else
                {
                    model.IsSuccess = false;
                    model.Messsage = "Không tìm thấy người dùng";
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