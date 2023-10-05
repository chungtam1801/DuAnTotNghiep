﻿using AppData.Models;
using AppData.ViewModels;

namespace AppAPI.IServices
{
    public interface IKhachHangService
    {
        Task<KhachHang> Add(KhachHangViewModel nv);
        public KhachHang GetById(Guid id);
        public bool Delete(Guid id);
        public bool Update(KhachHang khachHang);
        public List<KhachHang> GetAll();
    }
}
