using System;
using System.Collections.Generic;
using System.Linq;

namespace He_thong_ban_hang
{
    public interface IUserService
    {
        /// <summary>
        /// Hiển thị danh sách người dùng
        /// </summary>
        /// <returns>Danh sách người dùng dưới dạng List</returns>
        List<Users> GetUsersList();

        /// <summary>
        /// Hiển thị thông tin người dùng theo mã người dùng
        /// </summary>
        /// <param name="userId">Mã người dùng</param>
        /// <returns>Thông tin người dùng dưới dạng Model Users</returns>
        Users  GetUserById(int userId);

        /// <summary>
        /// Hiển thị chi tiết thông tin đơn hàng theo mã người dùng
        /// </summary>
        /// <param name="userID">Mã người dùng</param>
        /// <returns>Dữ liệu kiểu BaseRespone hiển thị chi tiết thông tin đơn hàng theo mã người dùng</returns>
        BaseRespone<List<Users>> GetUserDetailsById(int userID);

        /// <summary>
        /// Cập nhật thêm thông tin người dùng
        /// </summary>
        /// <param name="UserModel">Một object kiểu Model Users</param>
        /// <returns>Dữ liệu kiểu BaseRespone hiển thị thông tin người dùng được cập nhật</returns>
        BaseRespone<Users> SaveUser(Users UserModel);

        /// <summary>
        /// Xóa người dùng theo mã người dùng
        /// </summary>
        /// <param name="UserId">Mã người dùng</param>
        /// <returns>Dữ liệu kiểu BaseRespone hiển thị thông tin người dùng bị xóa</returns>
        BaseRespone<Users> DeleteUser(int UserId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Users LogInUser(LogInRequest model);
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
            List<Users> userList;
            try
            {
                userList = _context.Set<Users>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return userList;
        }
        public Users GetUserById(int userId)
        {
            Users user;
            try
            {
                user = _context.Find<Users>(userId);
            }
            catch (Exception)
            {
                throw;
            }
            return user;
        }
        public BaseRespone<List<Users>> GetUserDetailsById(int userID)
        {
            BaseRespone<List<Users>> response = new BaseRespone<List<Users>>();
            List<Users> user = new List<Users>();
            List<Order> ord = new List<Order>();
            try
            {
                user = _context.Uses.Where(x => x.UserId == userID).ToList();
                if (user != null)
                {
                    foreach (var itemOrder in user)
                    {
                        itemOrder.order = _context.Orders.Where(x => x.UserID == itemOrder.UserId).ToList();
                        ord = _context.Orders.Where(x => x.UserID == userID).ToList();
                        if (ord != null)
                        {
                            foreach (var itemDonHang in ord)
                            {
                                itemDonHang.orderDetail = _context.OrderDetails.Where(e => e.OrderID == itemDonHang.OrderID).ToList();
                            }
                        }
                    }
                }

                response.Data = user;
                response.Message = "Thành công";
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public BaseRespone<Users> SaveUser(Users UserModel)
        {
            BaseRespone<Users> respone = new BaseRespone<Users>();
            try
            {
                Users _temp = GetUserById(UserModel.UserId);
                if (_temp != null)
                {
                    _temp.UserName = UserModel.UserName;
                    _temp.UserPassword = UserModel.UserPassword;
                    _context.Update<Users>(_temp);
                    respone.Message = "Cập nhật thông tin người dùng thành công";
                }
                else
                {
                    _context.Add<Users>(UserModel);
                    respone.Message = "Thêm người dùng thành công";
                }
                _context.SaveChanges();
                respone.Data = UserModel;
                respone.Type = "Success";
            }
            catch (Exception ex)
            {
                respone.Type = "Error";
                respone.Message = "Error : " + ex.Message;
            }
            return respone;
        }
        public BaseRespone<Users> DeleteUser(int UserId)
        {
            BaseRespone<Users> response = new BaseRespone<Users>();
            try
            {
                Users _temp = GetUserById(UserId);
                if (_temp != null)
                {
                    response.Data = _temp;
                    _context.Remove<Users>(_temp);
                    _context.SaveChanges();
                    response.Type = "Success";
                    response.Message = "Xoá người dùng thành công";
                }
                else
                {
                    response.Type = "Success";
                    response.Message = "Không tìm thấy người dùng";
                }
            }
            catch (Exception ex)
            {
                response.Type = "Error";
                response.Message = "Error : " + ex.Message;
            }
            return response;
        }
        public Users LogInUser(LogInRequest model)
        {
            {
                Users userList = new Users();
                try
                {
                    userList = _context.Uses.Where(e => e.UserName == model.UserName && e.UserPassword == model.UserPassword).FirstOrDefault();
                }
                catch (Exception)
                {
                    throw;
                }
                return userList;
            }
        }
    }
}