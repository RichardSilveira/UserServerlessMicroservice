using System;

namespace UserService.Libraries.Email
{
    public class SmtpSettings
    {
        public string Host { get; }
        public int Port { get; }
        public string UserName { get; }
        public string Password { get; }
        public bool UseSsl { get; }

        public SmtpSettings(string host, int port, string userName, string password, bool? useSsl = null)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host));
            Port = port;
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Password = password ?? throw new ArgumentNullException(nameof(password));

            UseSsl = useSsl ?? false;
        }

        public void Deconstruct(out string host, out int port, out string username, out string password, out bool useSsl)
        {
            host = Host;
            port = Port;
            username = UserName;
            password = Password;
            useSsl = UseSsl;
        }
    }

}