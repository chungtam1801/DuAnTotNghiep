﻿using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;

namespace AppAPI.IServices
{
    public interface ISanPhamService
    {
        #region SanPham
        Task<List<SanPhamViewModel>> GetAllSanPham();
        Task<List<SanPhamViewModel>> TimKiemSanPham(SanPhamTimKiemNangCao sp);
        Task<SanPhamDetail> GetSanPhamById(Guid id);
        Task<List<SanPhamViewModel>> GetSanPhamByIdDanhMuc(Guid idloaisp);
        Task<bool> AddSanPham(SanPhamRequest request);
        Task<bool> UpdateSanPham(SanPhamRequest request);
        Task<bool> DeleteSanPham(Guid id);
        bool CheckTrungTenSP(SanPhamRequest lsp);
        #endregion

        #region LoaiSanPham
        Task<List<LoaiSP>> GetAllLoaiSPCha();
        Task<List<LoaiSP>> GetAllLoaiSPCon(string tenLoaiSPCha);
        Task<LoaiSP> GetLoaiSPById(Guid id);
        Task<LoaiSP> SaveLoaiSP(LoaiSPRequest lsp);
        Task<bool> DeleteLoaiSP(Guid id);
        bool CheckTrungLoaiSP(LoaiSPRequest lsp);
        #endregion

        #region ChiTietSanPham
        Task<List<ChiTietSanPham>> GetAllChiTietSanPham(Guid idSanPham);
        Task<bool> DeleteChiTietSanPham(Guid id);
        Task<bool> UpdateChiTietSanPham(ChiTietSanPham chiTietSanPham);
        Task<bool> AddChiTietSanPham(ChiTietSanPham chiTietSanPham);
        #endregion
        Task<List<MauSac>> GetAllMauSac();
        Task<List<KichCo>> GetAllKichCo();
        Task<List<ChatLieu>> GetAllChatLieu();
    }
}
