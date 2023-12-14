﻿using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class NhanVienService : INhanVienService
    {
        private readonly
        AssignmentDBContext _dbContext;

        public NhanVienService()
        {
            _dbContext = new AssignmentDBContext();
        }

        public bool Delete(Guid id)
        {
            var nv = _dbContext.NhanViens.FirstOrDefault(nv => nv.ID == id);
            if (nv != null)
            {
                _dbContext.NhanViens.Remove(nv);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public List<NhanVien> GetAll()
        {
            return _dbContext.NhanViens
        .Include(u => u.VaiTro)
        .Where(u => u.VaiTro.Ten == "Nhân viên")
        .OrderByDescending(u => u.TrangThai)
        .ToList();
        }

        public bool Update(Guid id, string ten, string email, string password, string sdt, string diachi, int trangthai, Guid idvaitro)
        {
            var nv = _dbContext.NhanViens.FirstOrDefault(x => x.ID == id);
            if (nv != null)
            {
                nv.Ten = ten;
                nv.Email = email;
                nv.PassWord = password;
                nv.SDT = sdt;
                nv.DiaChi = diachi;
                nv.TrangThai = trangthai;
                nv.IDVaiTro = idvaitro;
                _dbContext.NhanViens.Update(nv);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        private string MaHoaMatKhau(string matKhau)
        {
            // Ở đây, bạn có thể sử dụng bất kỳ phương thức mã hóa mật khẩu nào phù hợp
            // Ví dụ: sử dụng thư viện BCrypt.Net để mã hóa mật khẩu
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(matKhau);
            return hashedPassword;

            // Đây chỉ là ví dụ đơn giản, không nên sử dụng trong môi trường thực tế
            //return matKhau;
        }
        public async Task<NhanVien> Add(string ten, string email, string password, string sdt, string diachi, int trangthai, Guid idvaitro)
        {
            NhanVien nv = new NhanVien()
            {
                ID = Guid.NewGuid(),
                Ten = ten,
                Email = email,
                PassWord = MaHoaMatKhau(password),
                SDT = sdt,
                DiaChi = diachi,
                TrangThai = 1,
                IDVaiTro = Guid.Parse("952c1a5d-74ff-4daf-ba88-135c5440809c"),
            };
            _dbContext.NhanViens.Add(nv);
            _dbContext.SaveChanges();
            return nv;

        }

        public NhanVien GetById(Guid id)
        {
            var nv = _dbContext.NhanViens.FirstOrDefault(nv => nv.ID == id);
            return nv;
        }
    }
}
