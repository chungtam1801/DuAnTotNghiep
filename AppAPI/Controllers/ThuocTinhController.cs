﻿using AppAPI.IServices;
using AppAPI.Services;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThuocTinhController : ControllerBase
    {
        private readonly ISanPhamService _sanPhamService;
        public ThuocTinhController()
        {
            this._sanPhamService = new SanPhamService();
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllThuocTinh()
        {
            var listTT = await _sanPhamService.GetAllThuocTinh();
            return Ok(listTT);
        }
        [HttpPost("save")]
        public async Task<IActionResult> CreateOrUpdateThuocTinh(ThuocTinhRequest Request)
        {
            if (Request == null) return BadRequest();
            var tt = await _sanPhamService.SaveThuocTinh(Request);
            return Ok(tt);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> CreateOrUpdateThuocTinh(Guid id)
        {
            var tt = await _sanPhamService.DeleteThuocTinh(id);
            return Ok();
        }
    }
}
