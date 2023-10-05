﻿using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class QuanLyNguoiDungService : IQuanLyNguoiDungService
    {
        private readonly IAllRepository<NhanVien> reposNV;
        private readonly IAllRepository<KhachHang> reposKH;
        AssignmentDBContext context = new AssignmentDBContext();

        public QuanLyNguoiDungService()
        {
            reposNV = new AllRepository<NhanVien>(context, context.NhanViens);
            reposKH = new AllRepository<KhachHang>(context, context.KhachHangs);

        }

        public async Task<bool> ChangePassword(string email, string password, string newPassword)
        {
            var kh = await context.KhachHangs.FirstOrDefaultAsync(h => h.Email == email && h.Password == password);
            if (kh != null)
            {
                kh.Password = newPassword;
                await context.SaveChangesAsync();
                return true;
            }
            var nv = await context.NhanViens.FirstOrDefaultAsync(h => h.Email == email && h.PassWord == password);
            if (kh != null)
            {
                nv.PassWord = newPassword;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Login(string email, string password)
        {
            var nv = await context.NhanViens.FirstOrDefaultAsync(a => a.Email == email && a.PassWord == password);
            if (nv != null)
            {
                return true;
            }
            var kh = await context.KhachHangs.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
            if (kh != null)
            {
                return true;
            }
            return false;
        }

        public async Task<KhachHang> RegisterKhachHang(KhachHangViewModel khachHang)
        {
            KhachHang kh = new KhachHang()
            {
                IDKhachHang = Guid.NewGuid(),
                Ten = khachHang.Name,
                Email = khachHang.Email,
                Password = khachHang.Password
            };
            await context.KhachHangs.AddAsync(kh);
            //await context.SaveChangesAsync();
            GioHang gioHang = new GioHang()
            {
                IDKhachHang = kh.IDKhachHang,
                NgayTao = DateTime.Now,
            };
            await context.GioHangs.AddAsync(gioHang);
            await context.SaveChangesAsync();
            return kh;
        }

        public async Task<NhanVien> RegisterNhanVien(NhanVienViewModel nhanVien)
        {
            var kh = new NhanVien
            {
                ID = Guid.NewGuid(),
                Ten = nhanVien.Name,
                Email = nhanVien.Email,
                PassWord = nhanVien.Password,
                IDVaiTro = nhanVien.IDVaiTro
            };
            context.NhanViens.Add(kh);
            await context.SaveChangesAsync();
            return kh;
        }

    }
}
