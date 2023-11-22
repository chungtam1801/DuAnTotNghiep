﻿using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace AppView.Controllers
{
    public class BanHangTaiQuayController : Controller
    {
        private readonly HttpClient _httpClient;

        public BanHangTaiQuayController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");

        }
        //Giao diện bán hàng
        [HttpGet]
        public async Task<IActionResult> BanHang()
        {
            var listhdcho = await _httpClient.GetFromJsonAsync<List<HoaDon>>("HoaDon/GetAllHDCho");
            ViewData["lsthdcho"] = listhdcho;
            //var listpttt = await _httpClient.GetFromJsonAsync<List<PhuongThucThanhToan>>("HoaDon/PhuongThucThanhToan");
            //ViewData["lstPttt"] = listpttt;
            return View();
        }
        // Sản phẩm
        [HttpGet]
        public async Task<IActionResult> LoadSp(int page, int pagesize)
        {
            var listsanPham = await _httpClient.GetFromJsonAsync<List<SanPhamBanHang>>("SanPham/getAllSPBanHang");
            var model = listsanPham.Skip((page - 1) * pagesize).Take(pagesize).ToList();
            int totalRow = listsanPham.Count;
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true,
            });
        }
        //Hiển thị sản phẩm
        [HttpGet("/BanHangTaiQuay/ShowSPDetail/{idsp}")]
        public async Task<IActionResult> ShowSPDetail(string idsp)
        {
            var sP = await _httpClient.GetFromJsonAsync<ChiTietSanPhamBanHang>($"SanPham/getChiTietSPBHById/{idsp}");
            return PartialView("_SanPhamDetail", sP);
        }
        //Hiển thị lọc
        public async Task<IActionResult> ShowFilterSP()
        {
            var lsp = await _httpClient.GetFromJsonAsync<List<LoaiSP>>($"LoaiSP/getAll");
            ViewData["lstLSP"] = lsp;
            return PartialView("_LocSP");
        }
        //Tìm kiếm sản phẩm
        [HttpGet("/BanHangTaiQuay/Search/{keyword}")]
        public async Task<IActionResult> Search(string keyword)
        {
            var listsanPham = await _httpClient.GetFromJsonAsync<List<SanPhamBanHang>>("SanPham/getAllSPBanHang");
            var distinctResult = listsanPham
                .Where(c => c.Ten.ToLower().Contains(keyword.ToLower()))
                .Distinct()
                .ToList();
           var result = new List<SanPhamBanHang>();
            if (distinctResult.Count < 3)
            {
                var additionalItems = distinctResult.Take(result.Count).ToList();
                result.AddRange(additionalItems);
            }
            result = distinctResult.Take(3).ToList();
            return Json(new { data = result });
        }
        // Lấy Load CTSP trong SP
        [HttpGet("/BanHangTaiQuay/ShowListCTSP/{idsp}")]
        public async Task<IActionResult> ShowListCTSP(string idsp)
        {
            var lstctsP = await _httpClient.GetFromJsonAsync<List<ChiTietCTSPBanHang>>($"SanPham/getChiTietCTSPBHById/{idsp}");
            return Json(new { data = lstctsP });
        }
        public async Task<IActionResult> FilterCTSP(FilterCTSP filter)
        {
            var lstctsP = await _httpClient.GetFromJsonAsync<List<ChiTietCTSPBanHang>>($"SanPham/getChiTietCTSPBHById/{filter.IdSanPham}");
            //Lọc màu
            if(filter.lstIdMS != null)
            {
                lstctsP = lstctsP.Where(c => filter.lstIdMS.Contains(c.idMauSac)).ToList();
            }
            //Lọc kích thước
            if(filter.lstIdKC != null)
            {
                lstctsP = lstctsP.Where(c => filter.lstIdKC.Contains(c.idKichCo)).ToList();
            }
            return Json(new { data = lstctsP });
        }
        //Lấy Hóa đơn chi tiết
        [HttpGet("/BanHangTaiQuay/getCTHD/{id}")]
        public async Task<IActionResult> getCTHD(string id)
        {
            var hdon = await _httpClient.GetFromJsonAsync<HoaDonViewModelBanHang>($"HoaDon/GetHDBanHang/{id}");
            var kh = await _httpClient.GetFromJsonAsync<List<KhachHang>>($"KhachHang");
            ViewBag.lstKH = kh;
            return PartialView("GioHang", hdon);
        }

        // Thêm hóa đơn chi tiết
        public async Task<ActionResult> addHdct(HoaDonChiTietRequest request)
        {
            try
            {
                HoaDonChiTietRequest hdct = new HoaDonChiTietRequest()
                {
                    Id = new Guid(),
                    IdChiTietSanPham = request.IdChiTietSanPham,
                    IdHoaDon = request.IdHoaDon,
                    SoLuong = request.SoLuong,
                    //DonGia = request.DonGia,//Thanh toán rồi mới lưu
                };
                var response = await _httpClient.PostAsJsonAsync("ChiTietHoaDon/saveHDCT/", hdct);
                if (response.IsSuccessStatusCode) return Json(new { success = true });
                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
        //Xóa chi tiết hóa đơn
        [HttpDelete("/BanHangTaiQuay/deleteHdct/{id}")]
        public async Task<ActionResult> deleteHdct(String id)
        {
            var response = await _httpClient.DeleteAsync($"ChiTietHoaDon/delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Xóa thành công" });
            }
            else
                return Json(new { success = false, message = "Xóa thất bại" });
        }
        //Cập nhật số lượng 
        public async Task<IActionResult> UpdateSL(string idhdct, int sl)
        {
            try
            {
                var response = await _httpClient.PostAsync($"ChiTietHoaDon/UpdateSL?id={idhdct}&sl={sl}", null);
                if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Thêm số lượng thành công" });
                else return Json(new { success = false, message = "Thêm số lượng thất bại" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Thêm số lượng thất bại" });
            }
        }
        //Load Modal Thanh Tóan
        [HttpGet("/BanHangTaiQuay/ViewThanhToan/{id}")]
        public async Task<IActionResult> ViewThanhToan(string id)
        {
            var hd = await _httpClient.GetFromJsonAsync<HoaDon>($"HoaDon/GetById/{id}");
            var lstcthd = await _httpClient.GetFromJsonAsync<List<HoaDonChiTietViewModel>>($"ChiTietHoaDon/getByIdHD/{id}");
            //var listpttt = await _httpClient.GetFromJsonAsync<List<PhuongThucThanhToan>>("HoaDon/PhuongThucThanhToan");
            //Quy đổi điểm
            var qdd = await _httpClient.GetFromJsonAsync<List<QuyDoiDiem>>("QuyDoiDiem");
            var qddActive = qdd.FirstOrDefault(c => c.TrangThai == 1);
            //Kiểm tra là hóa đơn của khách có tài khoản không?
            var khachHang = "Khách lẻ";
            int? dtkh = 0;
            var response = await _httpClient.GetFromJsonAsync<bool>($"HoaDon/CheckLSGDHD/{id}");
            if (response == true)
            {
                var lstd = await _httpClient.GetFromJsonAsync<LichSuTichDiem>($"HoaDon/LichSuGiaoDich/{id}");
                var kh = await _httpClient.GetFromJsonAsync<KhachHang>($"KhachHang/GetById?id={lstd.IDKhachHang}");
                khachHang = kh.Ten;
                dtkh = kh.DiemTich;
            }
            var loginInfor = new LoginViewModel();
            string? session = HttpContext.Session.GetString("LoginInfor");
            if (session != null)
            {
                loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
            }
            var soluong = lstcthd.Sum(c => c.SoLuong);
            var ttien = lstcthd.Sum(c => c.SoLuong * c.GiaKM);
            //ViewData["lstPttt"] = listpttt;
            var hdtt = new HoaDonThanhToanViewModel()
            {
                Id = hd.ID,
                MaHD = hd.MaHD,
                NgayThanhToan = DateTime.Now,
                KhachHang = khachHang,
                TongSL = soluong,
                TongTien = ttien,
                DiemKH = dtkh,
                DiemTichHD = qddActive != null ? Convert.ToInt32(ttien / qddActive?.TiLeTichDiem) : 0,
                NhanVien = loginInfor.Ten,
                ThueVAT = (ttien * 10 / 100), // 10%
            };
            ViewBag.tileTieu = qddActive != null ? (qddActive.TiLeTieuDiem) : 0;
            return PartialView("_ThanhToan", hdtt);
        }
        
        //ThanhToan
        public async Task<IActionResult> ThanhToan(HoaDonThanhToanRequest request)
        {
            var hdrequest = new HoaDonThanhToanRequest()
            {
                Id = request.Id,
                IdNhanVien = request.IdNhanVien,
                NgayThanhToan = DateTime.Now,
                IdVoucher = request.IdVoucher == Guid.Empty ? Guid.Empty : request.IdVoucher,
                //IdPTTT = request.IdPTTT,
                PTTT = request.PTTT,
                TongTien = request.TongTien,
                ThueVAT = request.ThueVAT,
                DiemTichHD = request.DiemTichHD,
                DiemSD = request.DiemSD,
                TrangThai = 6,
            };
            var response = await _httpClient.PutAsJsonAsync("HoaDon/UpdateHoaDon/",hdrequest);
            if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Thanh toán thành công" });
            return Json(new { success = false, message = "Thanh toán thất bại" });
        }

        //Thêm nhanh khách hàng
        [HttpPost]
        public async Task<IActionResult> AddKhachHang(KhachHangView request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("KhachHang/PostKHView/", request);
                if (response.IsSuccessStatusCode) // Thêm khách hàng thành công -> tạo lịch sử tích điểm
                {
                    var qdd = await _httpClient.GetFromJsonAsync<List<QuyDoiDiem>>("QuyDoiDiem");
                    var idqdd = qdd.FirstOrDefault(c => c.TrangThai == 1).ID;
                    var kh = await _httpClient.GetFromJsonAsync<KhachHang>($"KhachHang/getBySDT?sdt={request.SDT}");
                    var IDHD = request.IDKhachHang; // Luu tam idhd qua idkh
                                                    // ktra hd đã có lstd 
                    var checkexist = await _httpClient.GetFromJsonAsync<bool>($"HoaDon/CheckLSGDHD/{IDHD}");
                    if (checkexist == true) // Tồn tại-> xóa
                    {
                        var lstdexist = await _httpClient.GetFromJsonAsync<LichSuTichDiem>($"HoaDon/LichSuGiaoDich/{IDHD}");
                        var deletelstd = await _httpClient.DeleteAsync($"LichSuTichDiem/{lstdexist.ID}");
                    }
                    string apiUrl = $"https://localhost:7095/api/LichSuTichDiem?diem=0&trangthai=1&IdKhachHang={kh.IDKhachHang}&IdQuyDoiDiem={idqdd}&IdHoaDon={IDHD}";
                    var lstdresponse = await _httpClient.PostAsync(apiUrl, null);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        //Sửa khách hàng
        public async Task<IActionResult> UpdateKHinHD(string idkh, string idhd)
        {
            try
            {
                var qdd = await _httpClient.GetFromJsonAsync<List<QuyDoiDiem>>("QuyDoiDiem");
                var idqdd = qdd.FirstOrDefault(c => c.TrangThai == 1).ID;
                var checkexist = await _httpClient.GetFromJsonAsync<bool>($"HoaDon/CheckLSGDHD/{idhd}");
                if (checkexist == true) // Tồn tại-> sửa
                {
                    var lstd = await _httpClient.GetFromJsonAsync<LichSuTichDiem>($"HoaDon/LichSuGiaoDich/{idhd}");
                    string apiUrl = $"https://localhost:7095/api/LichSuTichDiem/{lstd.ID}?diem={lstd.Diem}&trangthai={lstd.TrangThai}&IdKhachHang={idkh}&IdQuyDoiDiem={lstd.IDQuyDoiDiem}&IdHoaDon={idhd}";
                    var response = await _httpClient.PutAsync(apiUrl, null);
                }
                else // Chưa có lstd-> tạo mới
                {
                    string apiUrl = $"https://localhost:7095/api/LichSuTichDiem?diem=0&trangthai=1&IdKhachHang={idkh}&IdQuyDoiDiem={idqdd}&IdHoaDon={idhd}";
                    var lstdresponse = await _httpClient.PostAsync(apiUrl, null);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        //Xóa khách hàng
        [HttpGet("/BanHangTaiQuay/DeleteKHinHD/{idhd}")]
        public async Task<IActionResult> DeleteKHinHD(string idhd)
        {
            try
            {
                var checkexist = await _httpClient.GetFromJsonAsync<bool>($"HoaDon/CheckLSGDHD/{idhd}");
                if (checkexist == true) // Tồn tại-> xóa
                {
                    var lstd = await _httpClient.GetFromJsonAsync<LichSuTichDiem>($"HoaDon/LichSuGiaoDich/{idhd}");
                    var response = await _httpClient.DeleteAsync($"LichSuTichDiem/{lstd.ID}");
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        //Tìm kiếm khách hàng
        [HttpGet("/BanHangTaiQuay/SearchKH/{keyword}")]
        public async Task<IActionResult> SearchKH(string keyword)
        {
            var lstkh = await _httpClient.GetFromJsonAsync<List<KhachHang>>("KhachHang");
            var distinctResult = lstkh
                                .Where(c => c.Ten.ToLower().Contains(keyword.ToLower()) || c.SDT.Contains(keyword))
                                .Distinct()
                                .ToList();
            var result = new List<KhachHang>();
            if (distinctResult.Count < 3)
            {
                var additionalItems = distinctResult.Take(result.Count).ToList();
                result.AddRange(additionalItems);
            }
            result = distinctResult.Take(3).ToList();
            return Json(new { data = result });
        }
        //Check voucher

        [HttpGet]
        public  async Task<IActionResult> CheckVoucher(string voucher, int ttien)
        {
            string apiURL = $"https://localhost:7095/api/Voucher";
            var listvc = await _httpClient.GetFromJsonAsync<List<Voucher>>(apiURL);
            var vc = listvc.FirstOrDefault(c=>c.Ten.ToUpper() == voucher.ToUpper());
            if(vc == null)
            {
                return Json(new { success = false, message = "Voucher không hợp lệ" });
            }else if(vc.SoTienCan > ttien)
            {
                return Json(new { success = false, message = "Đặt đơn "+vc.SoTienCan.ToString("no")+" để áp dụng" });
            }else if( vc.HinhThucGiamGia == 0)
            {
                return Json(new { success = true, idvoucher = vc.ID, giatri = vc.GiaTri, message="Bạn được giảm "+ vc.GiaTri.ToString("n0") +" VND" });

            }else if(vc.HinhThucGiamGia == 1)
            {
                return Json(new { success = true, idvoucher = vc.ID, giatri = (ttien * vc.GiaTri/100), message = "Bạn được giảm " + vc.GiaTri+"%" });
            }
            return Json(new { message = "Đã xảy ra lỗi" });
        }
        //HÓA ĐƠN
        //Chuyển view hóa đơn
        [HttpGet("/BanHangTaiQuay/QuanLyHD")]
        public IActionResult QuanLyHD()
        {
            return PartialView("_QuanLyHoaDon");
        }
    }
}
