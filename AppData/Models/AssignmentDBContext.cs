﻿using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AppData.Models
{
    public class AssignmentDBContext : DbContext
    {
        public AssignmentDBContext()
        {
        }
        public AssignmentDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<BienThe> BienThes { get; set; }
        public DbSet<ChiTietBienThe> ChiTietBienThes { get; set; }
        public DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public DbSet<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; }
        public DbSet<GiaTri> GiaTris { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<KhuyenMai> KhuyenMais { get; set; }
        public DbSet<LichSuTichDiem> LichSuTichDiems { get; set; }
        public DbSet<LoaiSP> LoaiSPs { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }
        public DbSet<QuyDoiDiem> QuyDoiDiems { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<ThuocTinh> ThuocTinhs { get; set; }
        public DbSet<ThuocTinhSanPham> ThuocTinhSanPhams { get; set; }
        public DbSet<VaiTro> VaiTros { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Anh> Anhs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-S6G7NFV\SQLEXPRESS;Initial Catalog=AppBanQuanAoThoiTrangNam;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
