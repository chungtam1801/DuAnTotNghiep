﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppData.Configurations
{
    internal class DanhGiaConfiguration : IEntityTypeConfiguration<DanhGia>
    {
        public void Configure(EntityTypeBuilder<DanhGia> builder)
        {
            builder.ToTable("DanhGia");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.BinhLuan).HasColumnType("nvarchar(100)");
            builder.Property(x => x.Sao).HasColumnType("int");
            builder.Property(x => x.TrangThai).HasColumnType("int");
            builder.HasOne(x => x.KhachHang).WithMany(x => x.DanhGias).HasForeignKey(x => x.IDKhachHang);
            builder.HasOne(x => x.BienThe).WithMany(x => x.DanhGias).HasForeignKey(x => x.IDBienThe);
        }
    }
}
