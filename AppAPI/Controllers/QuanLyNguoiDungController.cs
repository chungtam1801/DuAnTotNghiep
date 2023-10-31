﻿using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using AppData.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyNguoiDungController : ControllerBase
    {
        private IQuanLyNguoiDungService service;

        public QuanLyNguoiDungController()
        {
            this.service = new QuanLyNguoiDungService();
        }

        // POST api/<DangNhapController>
        [HttpGet("DangNhap")]
        public async Task<IActionResult>Login(string lg, string password)
        {
            LoginViewModel login = await service.Login(lg, password);
            if(login == null)
            {
                ModelState.AddModelError(string.Empty, "Dang nhap that bai");
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(login);
            }
            //var result = await service.Login(email, password,  vaitro);
            //if (result != null)
            //{
            //    return Ok(result);
            //}
            //else
            //{
            //    ModelState.AddModelError(string.Empty, "Dang nhap that bai, ban nhap sai email hoac password");
            //    //return BadRequest("Dang nhap that bai");
            //}
            //return BadRequest("dang nhap that bai");
        }

        // POST api/<DangKyController>
        //[HttpPost("DangKyNhanVien")]
        //public async Task<IActionResult> DangKyNhanVien(NhanVienViewModel nhanVien)
        //{
        //    var nv = await service.RegisterNhanVien(nhanVien);
        //    if (nv == null)
        //    {
        //        return BadRequest();
        //    }

        //    return Ok("Đăng ký thành công");
        //}
        //POST api/<DangKyController>
        [HttpPost("DangKyKhachHang")]
        public async Task<IActionResult> DangKyKhachHang(KhachHangViewModel khachHang)
        {
            var kh = await service.RegisterKhachHang(khachHang);
            if (kh == null)
            {
                return BadRequest();
            }

            return Ok("Đăng ký thành công");
        }

        //[HttpPut("DoiMatKhauNhanVien")]
        //public async Task<IActionResult> DoiMatKhauNV(string email, string oldPassword,string newPassword)
        //{
        //    var dmk = await service.ChangePasswordNhanVien(email, oldPassword, newPassword);
        //    if (!dmk)
        //    {
        //        return Ok("Đổi mật khẩu thành công");
        //    }
        //    else
        //    {
        //        return BadRequest("Đổi mật khẩu không thành công");
        //    }
        //}
        [HttpPut("DoiMatKhau")]
        public async Task<IActionResult> DoiMatKhau(string email, string oldPassword, string newPassword)
        {
            var dmk = await service.ChangePassword(email, oldPassword, newPassword);
            if (!dmk)
            {
                return Ok("Đổi mật khẩu khong  thành công");
            }
            else
            {
                return BadRequest("Đổi mật khẩu  thành công");
            }
        }
        //Tam
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> DoiMatKhau(ChangePasswordRequest request)
        {
            var dmk = await service.ChangePassword(request);
            if (!dmk)
            {
                return BadRequest("Đổi mật khẩu khong  thành công");
            }
            else
            {
                return Ok("Đổi mật khẩu  thành công");
            }
        }
        //End
    }
}
