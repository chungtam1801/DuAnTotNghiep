﻿using AppAPI.IServices;
using AppData.Models;
using AppData.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class KhachHangService : IKhachHangService
    {
        private readonly AssignmentDBContext _dbContext;
        public KhachHangService()
        {
            _dbContext = new AssignmentDBContext();
        }

        public async Task<KhachHang> Add(KhachHangViewModel nv)
        {
            KhachHang kh = new KhachHang()
            {
                IDKhachHang = Guid.NewGuid(),
                Ten = nv.Email,
                Password = nv.Password,

            };
            await _dbContext.KhachHangs.AddAsync(kh);
            await _dbContext.SaveChangesAsync();
            GioHang gh = new GioHang()
            {
                IDKhachHang = kh.IDKhachHang,
            };
            await _dbContext.GioHangs.AddAsync(gh);
            await _dbContext.SaveChangesAsync();
            return kh;
        }

        public bool Delete(Guid id)
        {
            try
            {
                var kh = _dbContext.KhachHangs.FirstOrDefault(x => x.IDKhachHang == id);
                if (kh != null)
                {
                    _dbContext.KhachHangs.Remove(kh);
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<KhachHang> GetAll()
        {
            return _dbContext.KhachHangs.ToList();
        }

        public KhachHang GetById(Guid id)
        {
            return _dbContext.KhachHangs.FirstOrDefault(x => x.IDKhachHang == id);

        }

        public bool Update(Guid id, string email, string password)
        {
            var kh= _dbContext.KhachHangs.FirstOrDefault(a=>a.IDKhachHang == id);
            if (kh != null)
            {
                kh.Email = email;
                kh.Password = password;
                _dbContext.KhachHangs.Update(kh);   
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }



        //public bool Update(KhachHang nv)
        //{
        //    try
        //    {
        //        var kh = _dbContext.KhachHangs.FirstOrDefault(x => x.IDKhachHang == nv.IDKhachHang);
        //        if (kh != null)
        //        {
        //            _dbContext.KhachHangs.Update(kh);
        //            _dbContext.SaveChanges();
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}
    }
}
