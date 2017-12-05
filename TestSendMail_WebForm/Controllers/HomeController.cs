using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TestSendMail_WebForm.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> IndexAsync(List<HttpPostedFileBase> files)
        {
            MailerAPI.MailSetting setting = new MailerAPI.MailSetting()
                    {
                        SmtpServer = "192.168.1.27",
                        Port = 25,
                        UserName = "juncheng.liu",
                        Password = "ja$19840528"
                    };
            MailerAPI.MailInfo info = new MailerAPI.MailInfo();
            info.Subject = "test";
            info.Body = new System.Text.StringBuilder("test");
            List<string> testBigData = new List<string>();
            for (int i = 0; i < 50; i++)
            {
                testBigData.Add("j205073@gmail.com");
            }
            info.To = new List<string>() { "j205073@gmail.com" };
            if (files != null && files.Count > 0)
            {
                files.All(a =>
               {
                   if (a != null)
                       info.Files.Add(a.FileName, a.InputStream);
                   return true;
               });
            }
            MailerAPI.Mailer mailer = new MailerAPI.Mailer(setting, info);

            await mailer.AsyncSendMail();
            return Content("Done");
        }

        [HttpPost]
        public ActionResult Index(List<HttpPostedFileBase> files)
        {
            MailerAPI.MailSetting setting = new MailerAPI.MailSetting()
            {
                SmtpServer = "192.168.1.27",
                Port = 25,
                UserName = "juncheng.liu",
                Password = "ja$19840528"
            };
            MailerAPI.MailInfo info = new MailerAPI.MailInfo();
            info.Subject = "test";
            info.Body = new System.Text.StringBuilder("test");
            info.To = new List<string>() { "j205073@gmail.com" };
            if (files != null && files.Count > 0)
            {
                files.All(a =>
               {
                   if (a != null)
                       info.Files.Add(a.FileName, a.InputStream);
                   return true;
               });
            }

            MailerAPI.Mailer mailer = new MailerAPI.Mailer(setting, info);
            mailer.SendMail();

            return Content("Done");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}