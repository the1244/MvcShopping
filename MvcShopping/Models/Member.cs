using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcShopping.Models
{
    [DisplayName("會員資料")]
    [DisplayColumn("Name")]
    public class Member
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("會員帳號")]
        [Required(ErrorMessage = "請輸入Email地址")]
        [Description("我們直接以email當成會員登入帳號")]
        [MaxLength(25, ErrorMessage = "Email長度無法超過250個字元")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("會員密碼")]
        [Required(ErrorMessage = "請輸入會員密碼")]
        [MaxLength(40, ErrorMessage = "密碼不得超過4.個字元")]
        [Description("密碼將以SHA1進行雜湊運算，透過SHA1雜湊運算後的結果轉為HEX表示法的字串長度皆為40個字元")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("中文姓名")]
        [Required(ErrorMessage = "請輸入中文姓名")]
        [MaxLength(5, ErrorMessage = "不可超過5個字")]
        [Description("暫不考慮外國人使用英文個情況")]
        public string Name { get; set; }

        [DisplayName("網路暱稱")]
        [Required(ErrorMessage = "請輸入網路暱稱")]
        [MaxLength(10, ErrorMessage = "不可超過10個字")]
        public string Nickname { get; set; }

        [DisplayName("會員註冊時間")]
        public DateTime RegisterOn { get; set; }


        [DisplayName("會員啟用認證碼")]
        [MaxLength(36)]
        [Description("當AutoCode等於Null 則表示只會原已通過Email有效性驗證")]
        public string AutoCode { get; set; }

        public virtual ICollection<OrderHeader> Orders { get; set; }
    }
}