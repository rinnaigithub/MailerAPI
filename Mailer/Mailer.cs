using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MailerAPI
{
    /// <summary>
    /// Mailer
    /// </summary>
    public class Mailer
    {
        private MailInfo info { get; set; }
        private MailSetting setting { get; set; }

        /// <summary>
        /// init MailInfo
        /// </summary>
        /// <param name="info"></param>
        public Mailer(MailSetting setting, MailInfo info)
        {
            this.info = info;
            this.setting = setting;
        }

        /// <summary>
        /// 多執行緒傳送
        /// </summary>
        //public void SendMailAsync()
        //{
        //    ThreadStart threadStart = delegate() { SendMail(); };
        //    Thread thread = new Thread(threadStart);
        //    thread.Start();
        //}

        public Boolean SendMail()
        {
            ValidMailSetting();
            Boolean isSuccess = false;
            string address = String.Format("{0}<{1}>", "林內工業", "System@Rinnai.com.tw");
            MailMessage message = new MailMessage();
            try
            {
                if (info.To.Count == 0)
                    return false;
                foreach (var to in info.To)
                    message.To.Add(to);
                //寄件者
                message.From = new MailAddress(address, "系統提醒");
                //以Html方式發送
                message.IsBodyHtml = true;
                message.Subject = info.Subject;
                message.Body = info.Body.ToString();
                if (info.CC != null) { info.CC.ForEach(x => message.CC.Add(x)); }

                # region 附件
                if (info.Files.Count > 0)
                {
                    foreach (string fileName in info.Files.Keys)
                    {
                        Attachment attfile = new Attachment(info.Files[fileName], fileName);
                        message.Attachments.Add(attfile);
                    }
                }
                #endregion

                #region SendMail

                #endregion
                using (SmtpClient client = new SmtpClient(setting.SmtpServer, (int)setting.Port))
                {
                    client.Credentials = new System.Net.NetworkCredential(setting.UserName, setting.Password);
                    client.Timeout = Int32.MaxValue;
                    //發送郵件
                    client.Send(message);
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                //todo log mail content
                throw new Exception(ex.Message + ", " + ex.StackTrace);
            }
            finally
            {
                if (message.Attachments != null && message.Attachments.Count > 0)
                    message.Attachments.All(a => { a.Dispose(); return true; });
                message.Dispose();
                message = null;
            }
            return isSuccess;
        }

        public async Task AsyncSendMail()
        {
            ValidMailSetting();
            Boolean isSuccess = false;
            string address = String.Format("{0}<{1}>", "林內工業", "System@Rinnai.com.tw");
            MailMessage message = new MailMessage();
            try
            {
                if (info.To.Count == 0)
                    throw new Exception("Mail To Count Is 0.");
                foreach (var to in info.To)
                    message.To.Add(to);
                //寄件者
                message.From = new MailAddress(address, "系統提醒");
                //以Html方式發送
                message.IsBodyHtml = true;
                message.Subject = info.Subject;
                message.Body = info.Body.ToString();
                if (info.CC != null) { info.CC.ForEach(x => message.CC.Add(x)); }

                # region 附件
                if (info.Files.Count > 0)
                {
                    foreach (string fileName in info.Files.Keys)
                    {
                        Attachment attfile = new Attachment(info.Files[fileName], fileName);
                        message.Attachments.Add(attfile);
                    }
                }
                #endregion

                #region SendMail

                #endregion
                using (SmtpClient client = new SmtpClient(setting.SmtpServer, (int)setting.Port))
                {
                    client.Credentials = new System.Net.NetworkCredential(setting.UserName, setting.Password);
                    client.Timeout = Int32.MaxValue;
                    await client.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ", " + ex.StackTrace);
            }
            finally
            {
                if (message.Attachments != null && message.Attachments.Count > 0)
                    message.Attachments.All(a => { a.Dispose(); return true; });
                message.Dispose();
                message = null;
            }
        }

        private void ValidMailSetting()
        {
            if (string.IsNullOrEmpty(this.setting.SmtpServer))
                throw new Exception("SMTP Server Is Empty.");
            if (string.IsNullOrEmpty(this.setting.UserName))
                throw new Exception("UserName Is Empty.");
            if (string.IsNullOrEmpty(this.setting.Password))
                throw new Exception("Password Is Empty.");
            if (this.setting.Port == null)
                throw new Exception("Port Is Null.");
        }
    }
}