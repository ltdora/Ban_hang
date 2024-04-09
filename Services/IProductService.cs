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

        BaseRespone<Products> SaveProduct(Products productModel);

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
