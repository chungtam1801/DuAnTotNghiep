﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.BanOffline
{
    public class HoaDonThanhToanRequest
    {
        public Guid Id { get; set; }
        public Guid IdNhanVien { get; set; }
        public Guid IdPTTT { get; set; }
        public DateTime NgayThanhToan { get; set; }
        public Guid IdVoucher { get; set; }
        public int TrangThai { get; set; }
    }
}