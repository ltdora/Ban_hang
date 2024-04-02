using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public interface IProductService
    {
        List<Products> GetProductsList();

        Products GetProductDetailsById(int proID);

        List<Products> GetProductDetailsByName(string proName);

        ResponseModel SaveProduct(Products productModel);

        ResponseModel DeleteProduct(int productID);
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

        public ResponseModel SaveProduct(Products productModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                Products _temp = GetProductDetailsById(productModel.ProductID);
                if (_temp != null)
                {
                    _temp.ProductName = productModel.ProductName;
                    _temp.ProductPrice = productModel.ProductPrice;
                    _context.Update<Products>(_temp);
                    model.Messsage = "Cập nhật thông tin sản phẩm thành công";
                }
                else
                {
                    _context.Add<Products>(productModel);
                    model.Messsage = "Thêm sản phẩm thành công";
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
        public ResponseModel DeleteProduct(int productID)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                Products _temp = GetProductDetailsById(productID);
                if (_temp != null)
                {
                    _context.Remove<Products>(_temp);
                    _context.SaveChanges();
                    model.IsSuccess = true;
                    model.Messsage = "Xoá sản phẩm thành công";
                }
                else
                {
                    model.IsSuccess = false;
                    model.Messsage = "Không tìm thấy sản phẩm";
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
