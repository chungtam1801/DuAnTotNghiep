﻿using AppData.Models;
using AppData.ViewModels.SanPham;
using AppView.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IFileService _iFileService;
        public AdminController(IWebHostEnvironment hostEnvironment,IFileService iFileService)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
            _hostEnvironment = hostEnvironment;
            _iFileService = iFileService;
        }
        public IActionResult HomePageAdmin(Guid id)
        {
            return RedirectToAction("BanHang", "BanHangTaiQuay");
        }
        public IActionResult ProductManager()
        {
            var response = _httpClient.GetAsync(_httpClient.BaseAddress+ "SanPham/GetAllSanPhamAdmin").Result;
            if (response.IsSuccessStatusCode)
            {
                var lstSanPham = JsonConvert.DeserializeObject<List<SanPhamViewModelAdmin>>(response.Content.ReadAsStringAsync().Result);
                return View(lstSanPham);
            }
            else return BadRequest();
        }
        public IActionResult UpdateTrangThaiSanPham(string idSanPham,int trangThai)
        {
            var response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "SanPham/UpdateTrangThaiSanPham?id=" + idSanPham+"&trangThai="+trangThai).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ProductManager");
            }
            else return BadRequest();
        }
        public JsonResult GetLoaiSPCha()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllLoaiSPCha").Result;
            var loaiSP = JsonConvert.DeserializeObject<List<LoaiSP>>(response.Content.ReadAsStringAsync().Result);
            return Json(loaiSP);
        }
        public JsonResult GetAllMauSac()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllMauSac").Result;
            var mauSac = JsonConvert.DeserializeObject<List<MauSac>>(response.Content.ReadAsStringAsync().Result);
            return Json(mauSac);
        }
        public JsonResult GetAllKichCo()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllKichCo").Result;
            var kichCo = JsonConvert.DeserializeObject<List<KichCo>>(response.Content.ReadAsStringAsync().Result);
            return Json(kichCo);
        }
        public JsonResult GetAllChatLieu()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllChatLieu").Result;
            var chatLieu = JsonConvert.DeserializeObject<List<ChatLieu>>(response.Content.ReadAsStringAsync().Result);
            return Json(chatLieu);
        }
        public JsonResult GetLoaiSPCon(string tenLoaiSPCha)
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllLoaiSPCon?tenLoaiSPCha=" + tenLoaiSPCha).Result;
            var loaiSP = JsonConvert.DeserializeObject<List<LoaiSP>>(response.Content.ReadAsStringAsync().Result);
            return Json(loaiSP);
        }
        [HttpGet]
        public IActionResult AddSanPham()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSanPham(SanPhamRequest sanPhamRequest)
        {
            HttpResponseMessage response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "SanPham/AddSanPham", sanPhamRequest).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["UpdateChiTietSanPham"] = response.Content.ReadAsStringAsync().Result;
                TempData["MauSac"] = JsonConvert.SerializeObject(sanPhamRequest.MauSacs);
                return RedirectToAction("UpdateChiTietSanPham");
            }
            else return BadRequest();
        }
        [HttpGet]
        public IActionResult ProductDetail(string idSanPham)
        {
            var response = _httpClient.GetAsync(_httpClient.BaseAddress+ "SanPham/GetAllChiTietSanPhamAdmin?idSanPham=" + idSanPham).Result;
            if (response.IsSuccessStatusCode)
            {
                var lstSanPham = JsonConvert.DeserializeObject<List<ChiTietSanPhamViewModelAdmin>>(response.Content.ReadAsStringAsync().Result);
                TempData["IDSanPham"] = idSanPham;
                return View(lstSanPham);
            }
            else return BadRequest();
        }
        [HttpGet]
        public IActionResult QuanLyAnh(Guid idSanPham)
        {
            var response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllAnhSanPham?idSanPham=" + idSanPham).Result;
            if (response.IsSuccessStatusCode)
            {
                var lstAnh = JsonConvert.DeserializeObject<List<Anh>>(response.Content.ReadAsStringAsync().Result);
                ViewData["IDSanPham"] = idSanPham.ToString();
                return View("QuanLyAnh", lstAnh);
            }
            else return BadRequest();
        }
        [HttpPost]
        public IActionResult AddAnhNoColor(IFormFile file,string idSanPham)
        {
            string wwwrootPath = _hostEnvironment.WebRootPath;
            var anh = new Anh() { ID = Guid.NewGuid(),DuongDan = _iFileService.AddFile(file,wwwrootPath).Result,IDSanPham = new Guid(idSanPham),TrangThai =1};
            var response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "SanPham/AddImageNoColor", anh).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("QuanLyAnh", new { idSanPham });
            }
            else return BadRequest();
        }
        [HttpGet]
        public IActionResult AddChiTietSanPham(string idSanPham)
        {
            TempData["IDSanPham"] = idSanPham;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddChiTietSanPham(ChiTietSanPhamAddRequest request)
        {
            string idSanPham = TempData.Peek("IDSanPham").ToString();
            request.IDSanPham = new Guid(idSanPham);
            var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "SanPham/AddChiTietSanPham", request);
            if (response.IsSuccessStatusCode)
            {
                TempData["UpdateChiTietSanPham"] = response.Content.ReadAsStringAsync().Result;

                return RedirectToAction("UpdateChiTietSanPham");
            }
            else return BadRequest();
        }
        [HttpGet]
        public IActionResult AddAnhToMauSac()
        {
            var lstAnhRequest = JsonConvert.DeserializeObject<List<AnhRequest>>(TempData["MauSacs"].ToString());
            return View(lstAnhRequest);
        }
        [HttpPost]
        public JsonResult UpdateSoluongChiTietSanPham(string id,int soLuong)
        {
            ChiTietSanPhamRequest request = new ChiTietSanPhamRequest() { IDChiTietSanPham = new Guid(id), SoLuong = soLuong};
            var response = _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "SanPham/UpdateSoluongChiTietSanPham", request).Result;
            if (response.IsSuccessStatusCode)
            {
                return Json(new { Message = soLuong.ToString(), TrangThai = true });
            }
            else
            {
                return Json(new { Message = "Error",TrangThai = false });
            }
        }
        [HttpPost]
        public JsonResult UpdateGiaBanChiTietSanPham(string id, int giaBan)
        {
            ChiTietSanPhamRequest request = new ChiTietSanPhamRequest() { IDChiTietSanPham = new Guid(id), GiaBan = giaBan };
            var response = _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "SanPham/UpdateGiaBanChiTietSanPham", request).Result;
            if (response.IsSuccessStatusCode)
            {
                return Json(new { Message = giaBan.ToString(), TrangThai = true });
            }
            else
            {
                return Json(new { Message = "Error", TrangThai = false });
            }
        }
        [HttpPost]
        public JsonResult UpdateTrangThaiChiTietSanPham(string id)
        {
            var response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/UpdateTrangThaiChiTietSanPham?id="+id).Result;
            if (response.IsSuccessStatusCode)
            {
                return Json(new { TrangThai = true });
            }
            else
            {
                return Json(new { TrangThai = false });
            }
        }
        [HttpGet]
        public IActionResult UpdateChiTietSanPham()
        {
            var request = JsonConvert.DeserializeObject<ChiTietSanPhamUpdateRequest>(TempData["UpdateChiTietSanPham"].ToString());
            TempData["SanPham"]=request.IDSanPham.ToString();
            TempData["MaSP"] = request.Ma;
            if (request.MauSacs != null)
            {
                TempData["MauSac"] = JsonConvert.SerializeObject(request.MauSacs);
            }
            TempData["Location"] = request.Location.ToString();
            return View(request);
        }
        [HttpPost]
        public IActionResult UpdateChiTietSanPham(ChiTietSanPhamUpdateRequest request)
        {
            request.IDSanPham = new Guid(TempData.Peek("SanPham").ToString());
            request.Ma = TempData["MaSP"] as string;
            request.Location = Convert.ToInt32(TempData["Location"] as string);
            var response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress+ "SanPham/AddChiTietSanPhamFromSanPham", request).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AddAnhToSanPham");
            }
            else return BadRequest();
        }
        [HttpGet]
        public IActionResult AddAnhToSanPham()
        {
            var lst = JsonConvert.DeserializeObject<List<MauSac>>(TempData["MauSac"].ToString());
            return View(lst);
        }
        [HttpPost]
        public IActionResult AddAnhToSanPham(List<string> maMaus, List<IFormFile> images)
        {
            string wwwrootPath = _hostEnvironment.WebRootPath;
            string idSanPham = TempData["SanPham"].ToString();
            List<AnhRequest> lstAnhRequest = new List<AnhRequest>();
            for (var i = 0; i < images.Count; i++)
            {
                lstAnhRequest.Add(new AnhRequest() { IDSanPham = new Guid(idSanPham), MaMau = maMaus[i], DuongDan = _iFileService.AddFile(images[i], wwwrootPath).Result });
            }
            HttpResponseMessage response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "SanPham/AddAnh", lstAnhRequest).Result;
            if (response.IsSuccessStatusCode) return RedirectToAction("ProductManager");
            else return BadRequest();
        }
    }
}
