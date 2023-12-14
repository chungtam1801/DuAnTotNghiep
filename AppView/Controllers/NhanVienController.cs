﻿using AppData.Models;
using AppData.ViewModels;
using AppView.PhanTrang;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Net.Http;
using System.Text;

namespace AppView.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly AssignmentDBContext dBContext;
        public NhanVienController()
        {
            _httpClient = new HttpClient();
            dBContext = new AssignmentDBContext();
        }
        public int PageSize = 8;

        public async Task<IActionResult> Show(int ProductPage = 1)
        {
            string apiUrl = $"https://localhost:7095/api/NhanVien/GetAll";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<NhanVien>>(apiData);
            return View(new PhanTrangNhanVien
            {
                listNv = users
                        .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = users.Count()
                }
            });
        }

        [HttpGet]
        public async Task<IActionResult> SearchTheoTen(string? Ten, int ProductPage = 1)
        {
            if (string.IsNullOrWhiteSpace(Ten))
            {
                ViewData["SearchError"] = "Vui lòng nhập tên để tìm kiếm";
                return RedirectToAction("Show");
            }
            string apiUrl = $"https://localhost:7095/api/NhanVien/TimKiemNhanVien?name={Ten}";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<NhanVien>>(apiData);
            if (users.Count == 0)
            {
                ViewData["SearchError"] = "Không tìm thấy kết quả phù hợp";
            }
            return View("Show", new PhanTrangNhanVien
            {
                listNv = users
                         .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = users.Count()
                }
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NhanVien nhanVien)
        {
            nhanVien.TrangThai = 0;
            if (ModelState.IsValid)
            {
                if (nhanVien.IDVaiTro == Guid.Empty)
                {
                    nhanVien.IDVaiTro = Guid.Parse("952c1a5d-74ff-4daf-ba88-135c5440809c");
                }
                string apiUrl = $"https://localhost:7095/api/NhanVien/DangKyNhanVien?ten={nhanVien.Ten}&email={nhanVien.Email}&password={nhanVien.PassWord}&sdt={nhanVien.SDT}&diachi={nhanVien.DiaChi}";
                var reponsen = await _httpClient.PostAsync(apiUrl, null);
                if (reponsen.IsSuccessStatusCode)
                {
                    return RedirectToAction("Show");
                }
            }
            return View(nhanVien);

        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/NhanVien/GetById?id={id}";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<NhanVien>(apiData);
            var vt = await dBContext.VaiTros.FindAsync(user.IDVaiTro);
            ViewBag.TenVaiTro = vt.Ten;
            return View(user);
        }

        public IActionResult Edit(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/NhanVien/GetById?id={id}";
            var response = _httpClient.GetAsync(apiUrl).Result;
            var apiData = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<NhanVien>(apiData);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, NhanVien nv)
        {
            if (ModelState.IsValid)
            {
                string apiUrl = $"https://localhost:7095/api/NhanVien/{id}?ten={nv.Ten}&email={nv.Email}&password={nv.PassWord}&sdt={nv.SDT}&diachi={nv.DiaChi}&trangthai={nv.TrangThai}&idvaitro={nv.IDVaiTro}";

                var reponsen = await _httpClient.PutAsync(apiUrl, null);
                if (reponsen.IsSuccessStatusCode)
                {
                    return RedirectToAction("Show");
                }
            }
            return View(nv);

        }
        public async Task<IActionResult> Delete(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/NhanVien/{id}";
            var reposen = await _httpClient.DeleteAsync(apiUrl);
            if (reposen.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            return RedirectToAction("Show");
            //else
            //{
            //    // Log the error to the console.
            //    Console.WriteLine(reposen.StatusCode);
            //    Console.WriteLine(reposen.ReasonPhrase);
            //}

            //return View("DeleteError");
        }
        public async Task<IActionResult> Sua(Guid id)
        {
            var timkiem = dBContext.NhanViens.FirstOrDefault(x => x.ID == id);
            if (timkiem != null)
            {
                timkiem.TrangThai = timkiem.TrangThai == 1 ? 0 : 1;
                dBContext.NhanViens.Update(timkiem);
                dBContext.SaveChanges();
                return RedirectToAction("Show");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Next(int ProductPage = 1)
        {
            ProductPage++;
            return await Show(ProductPage);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Previous(int ProductPage = 1)
        {
            ProductPage--;
            return await Show(ProductPage);
        }

    }
}
