﻿using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using AppData.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LichSuTichDiemController : ControllerBase
    {
        private readonly ILishSuTichDiemServices _lichsu;
        private readonly AssignmentDBContext _dbcontext;
        public LichSuTichDiemController()
        {
            _dbcontext=new AssignmentDBContext();
           _lichsu = new LishSuTichDiemServices();
        }
        // GET: api/<LichSuTichDiemController>
        [HttpGet]
        public List<LichSuTichDiem> Get()
        {
            return _lichsu.GetAll();
        }
        // laam Strat
        [Route("GetLSTDByIdKH")]
        [HttpGet]
        public async Task<List<LichSuTichDiemView>> GetAllLSTDByKH()
        {
            
            var AllCTSP = await (from LSTD in _dbcontext.LichSuTichDiems.AsNoTracking()
                                 join kh in _dbcontext.KhachHangs.AsNoTracking() on LSTD.IDKhachHang equals kh.IDKhachHang
                                 join hd in _dbcontext.HoaDons.AsNoTracking() on LSTD.IDHoaDon equals hd.ID
                                 join qdd in _dbcontext.QuyDoiDiems.AsNoTracking() on LSTD.IDQuyDoiDiem equals qdd.ID
                                  
                                 select new LichSuTichDiemView()
                                 {
                                     Id=LSTD.ID,
                                     IDKhachHang = kh.IDKhachHang,
                                     IDHoaDon = hd.ID,
                                     IDQuyDoiDiem = qdd.ID,
                                     NgayTichOrTieuDiem = hd.NgayThanhToan,
                                     TenKhachHang = kh.Ten,
                                     SDT = kh.SDT,
                                    SoDiemTichOrTieu = qdd.SoDiem,
                                     DiemTichKH = kh.DiemTich,
                                    
                                     TrangThai = LSTD.TrangThai
                                 }).ToListAsync();
            return AllCTSP;
        }
        // laam end
        [HttpGet("GetAllDonMua")]
        public async Task<List<DonMuaViewModel>> GetAllDonMua(Guid IDkhachHang)
        {
            var listDonMua = await _lichsu.getAllDonMua(IDkhachHang);
            return listDonMua;
        }
        [HttpGet("GetCTHDDANHGIA")]
        public Task<ChiTietHoaDonDanhGiaViewModel> getCTHDDanhGia(Guid idhdct)
        {
            return _lichsu.getCTHDDanhGia(idhdct);
        }
        [HttpGet("GetAllDonMuaChiTiet")]
        public async Task<List<DonMuaChiTietViewModel>> GetAllDonMuaCT(Guid idHoaDon)
        {
            var listDonMuaCT = await _lichsu.getAllDonMuaChiTiet(idHoaDon);
            return listDonMuaCT;
        }
        // GET api/<LichSuTichDiemController>/5
        [HttpGet("{id}")]
        public LichSuTichDiem Get(Guid id)
        {
            return _lichsu.GetById(id);
        }

        // POST api/<LichSuTichDiemController>
        [HttpPost]
        public bool Post(int diem, int trangthai, Guid IdKhachHang, Guid IdQuyDoiDiem, Guid IdHoaDon)
        {
            return _lichsu.Add(diem, trangthai, IdKhachHang, IdQuyDoiDiem, IdHoaDon);
        }

        // PUT api/<LichSuTichDiemController>/5
        [HttpPut("{id}")]
        public bool Put(Guid id, int diem, int trangthai, Guid IdKhachHang, Guid IdQuyDoiDiem, Guid IdHoaDon)
        {
            var lichsu= _lichsu.GetById(id);
            if (lichsu != null)
            {
                return _lichsu.Update(lichsu.ID,diem, trangthai, IdKhachHang, IdQuyDoiDiem, IdHoaDon);
            }
            else
            {
                return false;
            }
        }

        // DELETE api/<LichSuTichDiemController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            var lichsu = _lichsu.GetById(id);
            if (lichsu != null)
            {
                return _lichsu.Delete(lichsu.ID);
            }
            else
            {
                return false;
            }
        }
    }
}
