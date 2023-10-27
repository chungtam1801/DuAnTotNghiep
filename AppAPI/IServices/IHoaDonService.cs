﻿using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;

namespace AppAPI.IServices
{
    public interface IHoaDonService
    {
        public bool CreateHoaDon(List<ChiTietHoaDonViewModel> chiTietHoaDons,HoaDonViewModel hoaDon);
        public bool CreateHoaDonOffline(HoaDonNhap hdnhap);
        public List<HoaDon> GetAllHoaDon();
        public List<ChiTietHoaDon> GetAllChiTietHoaDon(Guid idHoaDon);
        public bool UpdateTrangThaiGiaoHang(Guid idHoaDon, int trangThai,Guid idNhanVien);
        public int CheckVoucher(string ten, int tongtien);
        public List<HoaDon> TimKiemVaLocHoaDon(string ten,int? loc);
        public List<HoaDon> LichSuGiaoDich(Guid idNguoiDung);
        public bool DeleteHoaDon(Guid id);
        public bool UpdateHoaDon(HoaDon hoaDon);

    }
}
