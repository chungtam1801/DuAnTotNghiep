﻿using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels.ThongKe;
using System;

namespace AppAPI.Services
{
    public class ThongKeService : IThongKeService
    {
        private readonly IAllRepository<HoaDon> repos;
        AssignmentDBContext context = new AssignmentDBContext();
        public ThongKeService()
        {
            repos = new AllRepository<HoaDon>(context, context.HoaDons);
        }
        public decimal DoanhThuNam(int year)
        {
            var nam = context.HoaDons.Where(hd => hd.NgayTao.Year == year).ToList();
            decimal total = nam.Sum(hd => hd.TienShip);
            return total;
        }

        public decimal DoanhThuNgay(DateTime date)
        {
            var ngay = context.HoaDons.Where(hd => hd.NgayTao.Date == date.Date).ToList();
            decimal total = ngay.Sum(hd => hd.TienShip);
            return total;
        }

        public decimal DoanhThuThang(int month, int year)
        {
            var thang = context.HoaDons.Where(hd => hd.NgayTao.Month == month && hd.NgayTao.Year == year).ToList();
            decimal total = thang.Sum(hd => hd.TienShip);
            return total;
        }

        public ThongKeViewModel ThongKe(string startDate, string endDate)
        {
            //Lấy 3 cột đầu
            var soLuongThanhVien = context.KhachHangs.Count();
            var soLuongDonHangCho = context.HoaDons.Where(x => x.TrangThaiGiaoHang == 2).Count();
            var soLuongSanPham = context.ChiTietSanPhams.Sum(x => x.SoLuong);
            List<ChiTietHoaDon> lstChiTietHoaDon = new List<ChiTietHoaDon>();
            //var nam = DateTime.Now.Year;
            //var thang = DateTime.Now.Month;
            var start = Convert.ToDateTime(startDate);
            var end = Convert.ToDateTime(endDate);
            List<HoaDon> lstHoaDon = context.HoaDons.Where(x => (x.TrangThaiGiaoHang == 6 || x.TrangThaiGiaoHang==5 || x.TrangThaiGiaoHang==7) && x.NgayThanhToan >= start && x.NgayThanhToan<=end).ToList();
            var lstHoaDonTron = lstHoaDon.Where(x => (x.TrangThaiGiaoHang == 6 || x.TrangThaiGiaoHang == 5 || x.TrangThaiGiaoHang == 7) && x.NgayThanhToan >= start && x.NgayThanhToan<=end).ToList();
            var tongHoaDonTron = lstHoaDonTron.Count();
            //Lấy biểu đồ cột
            foreach (var hoaDon in lstHoaDon)
            {
                lstChiTietHoaDon.AddRange(context.ChiTietHoaDons.Where(x => x.IDHoaDon == hoaDon.ID));
            }
            List<ThongKeCotViewModel> thongKeCot = (from a in lstChiTietHoaDon
                                                    group a by a.IDCTSP into g
                                                    select new ThongKeCotViewModel()
                                                    {
                                                        TenSP = g.Key.ToString(),
                                                        SoLuong = g.Sum(x => x.SoLuong),
                                                    }).OrderByDescending(x => x.SoLuong).Take(10).ToList();
            ChiTietSanPham chiTietSanPham;
            MauSac mauSac;
            KichCo kichCo;
            SanPham sanPham;
            foreach(var item  in thongKeCot)
            {
                chiTietSanPham = context.ChiTietSanPhams.First(x=>x.ID==new Guid(item.TenSP));
                mauSac = context.MauSacs.First(x => x.ID == chiTietSanPham.IDMauSac);
                kichCo = context.KichCos.First(x => x.ID == chiTietSanPham.IDKichCo);
                sanPham = context.SanPhams.First(x => x.ID == chiTietSanPham.IDSanPham);
                item.TenSP = sanPham.Ten+"_"+mauSac.Ten+"_"+kichCo.Ten;
            }
            //Lấy biểu đồ đường
            List<ThongKeDuongViewModel> thongKeDuong = new List<ThongKeDuongViewModel>();
            //{
            //    new ThongKeDuongViewModel() { Ngay = start,SoLuongDon = context.HoaDons.Where(x => x.TrangThaiGiaoHang == 6 && x.NgayThanhToan==start).Count() }
            //};
            for (var i = start; i <= end; i=i.AddDays(1))
            {
                thongKeDuong.Add(new ThongKeDuongViewModel() { Ngay = i.Date, SoLuongDon = context.HoaDons.Where(x => x.TrangThaiGiaoHang == 6 && x.NgayThanhToan.Value.Date == i.Date).Count() });
            }
            //Lấy biểu đồ tròn
            List<ThongKeTronViewModel> thongKeTron = (from a in lstHoaDonTron
                                                      group a by a.TrangThaiGiaoHang into g
                                                      select new ThongKeTronViewModel()
                                                      {
                                                          TrangThaiHoaDon = g.Key == 6 ? "Thành công" : g.Key == 5 ? "Hoàn/Trả hàng" : "Hủy",
                                                          PhanTram = (tongHoaDonTron * 100) / g.Count(),
                                                      }).ToList();
            return new ThongKeViewModel() { SoLuongThanhVien = soLuongThanhVien, SoLuongDonHang = soLuongDonHangCho, SoLuongSanPham = soLuongSanPham, BieuDoCot = thongKeCot, BieuDoDuong = thongKeDuong.OrderBy(x=>x.Ngay).ToList(), BieuDoTron = thongKeTron };
        }
    }
}
