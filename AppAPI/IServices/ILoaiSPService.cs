﻿using AppData.Models;
using AppData.ViewModels.SanPham;

namespace AppAPI.IServices
{
    public interface ILoaiSPService
    {
        #region LoaiSP
        Task<List<LoaiSP>> GetAllLoaiSP();
        Task<LoaiSP> GetLoaiSPById(Guid id);
        Task<LoaiSP> SaveLoaiSP(LoaiSPRequest lsp);
        Task<bool> DeleteLoaiSP(Guid id);
        bool CheckTrungLoaiSP(LoaiSPRequest lsp);
        #endregion
        //Tam
        Task<List<LoaiSP>> GetLoaiSPCha();
        Task<List<LoaiSP>> GetLoaiSPCon(Guid idLoaiSPCha);
        //End
    }
}
