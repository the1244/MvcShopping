using MvcShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcShopping.Controllers
{
    public class MemberController : Controller
    {
        MvcShoppingContext db = new MvcShoppingContext();
        //
        // GET: /Member/

        //會員註冊頁面
        public ActionResult Register()
        {
            return View();
        }

        //密碼雜湊所需的Salt亂數值
        private string pwSalt = "A1rySq1oPe2Mh784QQwG6jRAfkdPpDa90J0i";

        //寫入會員資料
        [HttpPost]
        public ActionResult Register([Bind(Exclude = "RegisterOn,AuthCode")]  Member member)
        {
            var chk_member = db.Members.Where(p => p.Email == member.Email).FirstOrDefault();

            if (chk_member != null)
            {
                ModelState.AddModelError("Email", "您輸入的Email已經有人註冊過了");
            }
            if (ModelState.IsValid)
            {
                //將密碼加鹽(salt)之後進行雜湊運算以提升會員密碼的安全性
                member.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(pwSalt + member.Password, "SHA1");
                //會員註冊時間
                member.RegisterOn = DateTime.Now;
                //會員驗證碼，採用Guid當驗證內容，避免有會員使用到重覆的驗證碼
                member.AutoCode = Guid.NewGuid().ToString();

                db.Members.Add(member);
                db.SaveChanges();

                SendAuthCodeToMember(member);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }

        }

        private void SendAuthCodeToMember(Member member)
        {
            string mailBody = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/MemberRegisterEmailTemplate.htm"));
            mailBody = mailBody.Replace("{{Name}}", member.Name);
            mailBody = mailBody.Replace("{{RegisterOn}}", member.RegisterOn.ToString("F"));

            var auth_url = new UriBuilder(Request.Url)
            {
                Path = Url.Action("ValidateRegister", new { id = member.AutoCode }),
                Query = ""
            };

            mailBody = mailBody.Replace("{{AUTH_URL}}", auth_url.ToString());

            try
            {
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("mylife1244@gmail.com", "@The1244");
                SmtpServer.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("mylife1244@gmail.com");
                mail.To.Add(member.Email);
                mail.Subject = "確認信";
                mail.Body = mailBody;
                mail.IsBodyHtml = true;

                SmtpServer.Send(mail);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //顯示會員登入頁面
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        //執行會員登入
        [HttpPost]
        public ActionResult Login(string email,string password,string returnUrl)
        {
            if (ValidateUser(email, password))
            {
                FormsAuthentication.SetAuthCookie(email, false);

                if (String.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(returnUrl);
                }
                
            }
            ModelState.AddModelError("", "帳號或密碼錯誤");
            return View();

        }

        private bool ValidateUser(string email,string password)
        {
            var hash_pw = FormsAuthentication.HashPasswordForStoringInConfigFile(pwSalt + password, "SHA1");

            var member = (from p in db.Members where p.Email == email && p.Password == hash_pw select p).FirstOrDefault();

            //如果member物件不等於null則代表會員的帳號，密碼輸入正確
            return (member != null);
                
        }

        //執行會員登出
        public ActionResult Logout()
        {
            //清除表單驗證的Cookies
            FormsAuthentication.SignOut();

            //清除所有曾經寫入過的Session資料
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
