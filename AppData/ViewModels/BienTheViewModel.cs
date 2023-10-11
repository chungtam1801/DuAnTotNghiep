﻿using AppData.Models;
using AppData.ViewModels.SanPham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class BienTheViewModel
    {
        public Guid ID { get; set; }
        public string Ten { get; set; }
        public int SoLuong { get; set; }
        public int GiaBan { get; set; } 
        public int GiaGoc { get; set; }
        public int TrangThai { get; set; }
        public List<string> Anh { get; set; }
        public List<GiaTriRequest> GiaTris { get; set; }
    }
}
