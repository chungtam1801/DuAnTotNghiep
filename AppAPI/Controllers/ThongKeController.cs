﻿using AppAPI.IServices;
using AppAPI.Services;
using AppData.ViewModels.ThongKe;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        private readonly IThongKeService thongKeService;

        public ThongKeController(IThongKeService thongKeService)
        {
            this.thongKeService = thongKeService;
        }


        // GET: api/<ThongKeController>
        [HttpGet("DoanhThuTheoNgay")]
        public async Task<IActionResult> GetTotalByDay(DateTime date)
        {
            decimal total = thongKeService.DoanhThuNgay(date);
            return Ok(total);
        }

        // GET: api/<ThongKeController>
        [HttpGet("DoanhThuTheoThang")]
        public async Task<IActionResult> GetTotalByMonth(int month, int year)
        {
            decimal total = thongKeService.DoanhThuThang(month, year);
            return Ok(total);
        }

        // GET: api/<ThongKeController>
        [HttpGet("DoanhThuTheoNam")]
        public async Task<IActionResult> GetTotalByYear(int year)
        {
            decimal total = thongKeService.DoanhThuNam(year);
            return Ok(total);
        }
        [HttpGet("ThongKe")]
        public ThongKeViewModel ThongKe(string startDate, string endDate)
        {
            return thongKeService.ThongKe(startDate,endDate);
        }
        [HttpGet("ThongKeSanPham")]
        public List<ThongKeSanPham> ThongKeSanPham()
        {
            return thongKeService.ThongKeSanPham();
        }
    }
}
