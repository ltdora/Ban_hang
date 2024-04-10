using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public interface IProductService
    {
        /// <summary>
        /// Hiển thị danh sách sản phẩm
        /// </summary>
        /// <returns>Danh sách sản phẩm dưới dạng List</returns>
        List<Products> GetProductsList();

        /// <summary>
        /// Lấy thông tin sản phẩm theo mã sản phẩm
        /// </summary>
        /// <param name="proID">Mã sản phẩm</param>
        /// <returns>Thông tin sản phẩm dưới dạng Model Product</returns>
        Products GetProductDetailsById(int proID);

        /// <summary>
        /// Lấy thông tin sản phẩm theo tên
        /// </summary>
        /// <param name="proName">Tên sản phẩm</param>
        /// <returns>Thông tin sản phẩm dưới dạng Model Product</returns>
        List<Products> GetProductDetailsByName(string proName);

        /// <summary>
        /// Cập nhật thêm thông tin sản phẩm
        /// </summary>
        /// <param name="productModel">Một object kiểu Model Products</param>
        /// <returns>Dữ liệu kiểu BaseRespone hiển thị thông tin sản phẩm được cập nhật</returns>
        BaseRespone<Products> SaveProduct(Products productModel);

        /// <summary>
        /// Cập nhật xóa sản phẩm
        /// </summary>
        /// <param name="productID">Mã sản phẩm</param>
        /// <returns>Dữ liệu kiểu BaseRespone hiển thị thông tin sản phẩm vừa xóa</returns>
        BaseRespone<Products> DeleteProduct(int productID);
    }
    public class ProductService : IProductService
    {
        private ShopContext _context;
        public ProductService(ShopContext context)
        {
            _context = context;
        }
        public List<Products> GetProductsList()
        {
            List<Products> proList;
            try
            {
                proList = _context.Set<Products>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return proList;
        }
        public Products GetProductDetailsById(int proID)
        {
            Products pro;
            try
            {
                pro = _context.Find<Products>(proID);
            }
            catch (Exception)
            {
                throw;
            }
            return pro;
        }
        public List<Products> GetProductDetailsByName(string proName)
        {
            List<Products> pro = new List<Products>();
            try
            {
                pro = _context.Products.Where(p => p.ProductName == proName).ToList();
                //pro = _context.Set<Products>().ToList().Where(e => e.ProductName ==);
            }
            catch (Exception)
            {
                throw;
            }
            return pro;
        }
        public BaseRespone<Products> SaveProduct(Products productModel)
        {
            BaseRespone<Products> respone = new BaseRespone<Products>();
            try
            {
                Products _temp = GetProductDetailsById(productModel.ProductID);
                if (_temp != null)
                {
                    _temp.ProductName = productModel.ProductName;
                    _temp.ProductPrice = productModel.ProductPrice;
                    _context.Update<Products>(_temp);
                    respone.Message = "Cập nhật thông tin sản phẩm thành công";
                }
                else
                {
                    _context.Add<Products>(productModel);
                    respone.Message = "Thêm sản phẩm thành công";
                }
                _context.SaveChanges();
                respone.Data = productModel;
                respone.Type = "Success";
            }
            catch (Exception ex)
            {
                respone.Type = "Error";
                respone.Message = "Error : " + ex.Message;
            }
            return respone;
        }
        public BaseRespone<Products> DeleteProduct(int productID)
        {
            BaseRespone<Products> respone = new BaseRespone<Products>();
            try
            {
                Products _temp = GetProductDetailsById(productID);
                if (_temp != null)
                {
                    respone.Data = _temp;
                    _context.Remove<Products>(_temp);
                    _context.SaveChanges();
                    respone.Type = "Success";
                    respone.Message = "Xoá sản phẩm thành công";
                }
                else
                {
                    respone.Type = "Success";
                    respone.Message = "Không tìm thấy sản phẩm";
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
