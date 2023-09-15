﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class DanhGia
    {
        public Guid ID { get; set; }
        public string BinhLuan { get; set; }
        public int Sao { get; set; }
        public int TrangThai { get; set; }
        public Guid IDBienThe { get; set; }
        public Guid IDKhachHang { get; set; }
        public virtual BienThe BienThe { get; set; }
        public virtual KhachHang KhachHang { get; set; }
    }
}
