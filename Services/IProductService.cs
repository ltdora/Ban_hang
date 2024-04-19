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
        /// <param name="productID">Mã sản phẩm</param>
        /// <returns>Thông tin sản phẩm dưới dạng Model Product</returns>
        Products GetProductDetailsById(int productID);

        /// <summary>
        /// Lấy thông tin sản phẩm theo tên
        /// </summary>
        /// <param name="productName">Tên sản phẩm</param>
        /// <returns>Thông tin sản phẩm dưới dạng Model Product</returns>
        List<Products> GetProductDetailsByName(string productName);

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
            List<Products> lstProduct;
            try
            {
                lstProduct = _context.Set<Products>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return lstProduct;
        }
        public Products GetProductDetailsById(int productID)
        {
            Products product;
            try
            {
                product = _context.Find<Products>(productID);
            }
            catch (Exception)
            {
                throw;
            }
            return product;
        }
        public List<Products> GetProductDetailsByName(string productName)
        {
            List<Products> product = new List<Products>();
            try
            {
                product = _context.Products.Where(p => p.ProductName == productName).ToList();
                //pro = _context.Set<Products>().ToList().Where(e => e.ProductName ==);
            }
            catch (Exception)
            {
                throw;
            }
            return product;
        }
        public BaseRespone<Products> SaveProduct(Products productModel)
        {
            BaseRespone<Products> response = new BaseRespone<Products>();
            try
            {
                Products _temp = GetProductDetailsById(productModel.ProductID);
                if (_temp != null)
                {
                    _temp.ProductName = productModel.ProductName;
                    _temp.ProductPrice = productModel.ProductPrice;
                    _context.Update<Products>(_temp);
                    response.Message = "Cập nhật thông tin sản phẩm thành công";
                }
                else
                {
                    _context.Add<Products>(productModel);
                    response.Message = "Thêm sản phẩm thành công";
                }
                _context.SaveChanges();
                response.Data = productModel;
                response.Type = "Success";
            }
            catch (Exception ex)
            {
                response.Type = "Error";
                response.Message = "Error : " + ex.Message;
            }
            return response;
        }
        public BaseRespone<Products> DeleteProduct(int productID)
        {
            BaseRespone<Products> response = new BaseRespone<Products>();
            try
            {
                Products _temp = GetProductDetailsById(productID);
                if (_temp != null)
                {
                    response.Data = _temp;
                    _context.Remove<Products>(_temp);
                    _context.SaveChanges();
                    response.Type = "Success";
                    response.Message = "Xoá sản phẩm thành công";
                }
                else
                {
                    response.Type = "Success";
                    response.Message = "Không tìm thấy sản phẩm";
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
