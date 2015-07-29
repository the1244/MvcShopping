using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcShopping.Models
{
    [DisplayName("訂單主檔")]
    [DisplayColumn("DisplayNmae")]
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("訂購會員")]
        [Required]
        public virtual Member Member { get; set; }

        [DisplayName("收件人姓名")]
        [Required(ErrorMessage = "請輸入收件人姓名")]
        [MaxLength(40, ErrorMessage = "不可超過40個字")]
        [Description("訂購會員不一定是收件人")]
        public string ContactName { get; set; }

        [DisplayName("連絡電話")]
        [Required(ErrorMessage = "請輸入連絡電話ex+886-2-23222480#6342")]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhoneNO { get; set; }

        [DisplayName("遞送地址")]
        [Required(ErrorMessage = "請輸入遞送地址")]
        public string ContactAddress { get; set; }

        [DisplayName("訂單金額")]
        [Required]
        [DataType(DataType.Currency)]
        [Description("由於訂單金額可能受商品遞送方式或優惠折扣等方式異動價格，因此必須保留購買當下算出來的訂單金額")]
        public int TotalPrice { get; set; }

        [DisplayName("訂單備註")]
        [DataType(DataType.MultilineText)]
        public string Memo { get; set; }

        [DisplayName("訂購時間")]
        public DateTime BuyOn { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get { return this.Member.Name + "於" + this.BuyOn + "訂購商品"; }
        }

        public virtual ICollection<OrderDetail> OrderDetailItems { get; set; }



    }
}