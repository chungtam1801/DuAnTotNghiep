﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class LoaiSPRequest
    {
        public Guid ID { get; set; }
        public string Ten { get; set; }
        public int TrangThai { get; set; }
        public Guid? IDLoaiSPCha { get; set; }

    }
}
