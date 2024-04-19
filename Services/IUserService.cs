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
        /// <param name="userID">Mã người dùng</param>
        /// <returns>Thông tin người dùng dưới dạng Model Users</returns>
        Users  GetUserById(int userID);

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
        /// <param name="UserID">Mã người dùng</param>
        /// <returns>Dữ liệu kiểu BaseRespone hiển thị thông tin người dùng bị xóa</returns>
        BaseRespone<Users> DeleteUser(int UserID);

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
            List<Users> lstUser;
            try
            {
                lstUser = _context.Set<Users>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return lstUser;
        }
        public Users GetUserById(int userID)
        {
            Users user;
            try
            {
                user = _context.Find<Users>(userID);
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
            List<Users> users = new List<Users>();
            List<Order> orders = new List<Order>();
            try
            {
                users = _context.Uses.Where(x => x.UserId == userID).ToList();
                if (users != null)
                {
                    foreach (var itemOrder in users)
                    {
                        itemOrder.order = _context.Orders.Where(x => x.UserID == itemOrder.UserId).ToList();
                        orders = _context.Orders.Where(x => x.UserID == userID).ToList();
                        if (orders != null)
                        {
                            foreach (var itemDonHang in orders)
                            {
                                itemDonHang.orderDetail = _context.OrderDetails.Where(e => e.OrderID == itemDonHang.OrderID).ToList();
                            }
                        }
                    }
                }

                response.Data = users;
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
            BaseRespone<Users> response = new BaseRespone<Users>();
            try
            {
                Users _temp = GetUserById(UserModel.UserId);
                if (_temp != null)
                {
                    _temp.UserName = UserModel.UserName;
                    _temp.UserPassword = UserModel.UserPassword;
                    _context.Update<Users>(_temp);
                    response.Message = "Cập nhật thông tin người dùng thành công";
                }
                else
                {
                    _context.Add<Users>(UserModel);
                    response.Message = "Thêm người dùng thành công";
                }
                _context.SaveChanges();
                response.Data = UserModel;
                response.Type = "Success";
            }
            catch (Exception ex)
            {
                response.Type = "Error";
                response.Message = "Error : " + ex.Message;
            }
            return response;
        }
        public BaseRespone<Users> DeleteUser(int UserID)
        {
            BaseRespone<Users> response = new BaseRespone<Users>();
            try
            {
                Users _temp = GetUserById(UserID);
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
                Users lstUser = new Users();
                try
                {
                    lstUser = _context.Uses.Where(e => e.UserName == model.UserName && e.UserPassword == model.UserPassword).FirstOrDefault();
                }
                catch (Exception)
                {
                    throw;
                }
                return lstUser;
            }
        }
    }
}