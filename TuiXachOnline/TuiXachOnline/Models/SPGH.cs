using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuiXachOnline.Models
{
    public class SPGH
    {
        QLTuiXachEntities db = new QLTuiXachEntities();
        public int MaSPham { get; set; }
        public string TenSPham { get; set; }
        public double GiaSP { get; set; }
        public string MoTaSP { get; set; }
        public string AnhSP { get; set; }
        public DateTime Ngay { get; set; }
        public int SoLuongTonSP { get; set; }
        public int SoLuong { get; set; }
        public int MaNXB { get; set; }
        public double TongTien
        {
            get { return SoLuong * GiaSP; }
        }
        public int MaChuDeSP { get; set; }
        
    }
}