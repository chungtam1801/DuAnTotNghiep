﻿using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using AppView.PhanTrang;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AppView.Controllers
{
    public class MauSacController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly AssignmentDBContext dBContext;
        public MauSacController()
        {
            _httpClient = new HttpClient();
            dBContext = new AssignmentDBContext();
        }
        public int PageSize = 8;

        public async Task<IActionResult> Show(int ProductPage = 1)
        {
            string apiUrl = $"https://localhost:7095/api/MauSac/GetAllMauSac";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<MauSac>>(apiData);
            return View(new PhanTrangMauSac
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
            string apiUrl = $"https://localhost:7095/api/MauSac/TimKiemMauSac?name={Ten}";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<MauSac>>(apiData);
            if (users.Count == 0)
            {
                ViewData["SearchError"] = "Không tìm thấy kết quả phù hợp";
            }
            return View("Show", new PhanTrangMauSac
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
        public async Task<IActionResult> Create(MauSac ms)
        {
            ms.TrangThai = 1;
            string apiUrl = $"https://localhost:7095/api/MauSac/ThemMauSac?ten={ms.Ten}&ma={ms.Ma}";
            var content = new StringContent(JsonConvert.SerializeObject(ms), Encoding.UTF8, "application/json");
            var reponsen = await _httpClient.PostAsync(apiUrl, content);
            if (reponsen.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/MauSac/GetMauSacById?id={id}";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<MauSac>(apiData);
            return View(user);
        }
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/MauSac/GetMauSacById?id={id}";
            var response = _httpClient.GetAsync(apiUrl).Result;
            var apiData = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<MauSac>(apiData);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MauSac ms)
        {
            ms.TrangThai = 1;
            string apiUrl = $"https://localhost:7095/api/MauSac/{id}?ten={ms.Ten}&ma={ms.Ma}";
            var content = new StringContent(JsonConvert.SerializeObject(ms), Encoding.UTF8, "application/json");
            var reponsen = await _httpClient.PutAsync(apiUrl, content);
            if (reponsen.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            return View();
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/MauSac/{id}";
            var reposen = await _httpClient.DeleteAsync(apiUrl);
            if (reposen.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            return RedirectToAction("Show");
        }
        public async Task<IActionResult> Sua(Guid id)
        {
            var timkiem = dBContext.MauSacs.FirstOrDefault(x => x.ID == id);
            if (timkiem != null)
            {
                timkiem.TrangThai = timkiem.TrangThai == 0 ? 1 : 0;
                dBContext.MauSacs.Update(timkiem);
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
