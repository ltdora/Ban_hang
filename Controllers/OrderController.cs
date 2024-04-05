using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class OrderController : Controller
    {
        IOrderService _orderService;
        public OrderController(IOrderService service)
        {
            _orderService = service;
        }
        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateOrder(List<OrderDetail> liOrderDetail, int userID)
        {
            try
            {
                var model = _orderService.CreateOrder(liOrderDetail, userID);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult DisplayOrder(int userID)
        {
            try
            {
                var ord = _orderService.DisplayOrder(userID);
                if (ord == null) return NotFound();
                return Ok(ord);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        [HttpPost]
        [Route("[action]")]
        public IActionResult SaveOrder(int quantity, int productID, int orderID, decimal price)
        {
            try
            {
                var model = _orderService.SaveOrder(quantity, productID, orderID, price);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("[action]")]
        public IActionResult DeleteOrder(int quantity, int productID, int orderID)
        {
            try
            {
                var model = _orderService.DeleteOrder(quantity, productID, orderID);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult ExportExcelOrder()
        {
            var data = _orderService.DisplayExcelOrder().Data;
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var properties = data[0].GetType().GetProperties();
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells["A:AZ"].Style.Font.Size = 13;
                //Tiêu đề
                workSheet.Cells[1, 1, 1, properties.Length + 1].Merge = true;
                workSheet.Cells[1, 1].Value = "DANH SÁCH";
                workSheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                //Header
                workSheet.Cells[2, 1].Value = "STT";
                int i = 1;
                foreach (var prop in properties)
                {
                    i++;
                    workSheet.Cells[2, i].Value = prop.Name;
                }
                workSheet.Rows[2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Rows[2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Rows[2].Style.Font.Bold = true;
                //Data
                int row = 3;
                i = 1;
                foreach (var item in data)
                {
                    workSheet.Cells[row, 1].Value = i;
                    var propertiesTmp = item.GetType().GetProperties();
                    int j = 1;
                    foreach (var prop in propertiesTmp)
                    {
                        j++;
                        if (prop.PropertyType == typeof(DateTime))
                            workSheet.Cells[row, j].Value = prop.GetValue(item).ToString();
                        else
                            workSheet.Cells[row, j].Value = prop.GetValue(item);
                    }
                    row++;
                    i++;
                }
                workSheet.Cells["A:AZ"].AutoFitColumns();
                workSheet.Columns[1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                using (ExcelRange Rng = workSheet.Cells[2, 1, row - 1, properties.Length + 1])
                {
                    Rng.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"List-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
