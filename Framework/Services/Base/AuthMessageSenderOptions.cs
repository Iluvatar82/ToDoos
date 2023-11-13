namespace Framework.Services.Base
{
    public class AuthMessageSenderOptions
    {
        public string? SendGridKey { get; set; }// = "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";
        public Email? Email { get; set; }
    }

    public class Email
    {
        public string? Sender_Address { get; set; } // = "user@todoos.net"
        public string? Sender_Username { get; set; } // = "79497mail1"
        public string? Sender_Display_Name { get; set; } // = "Todoos.net"
        public string? Sender_Password { get; set; } // = ".yxsqq6hgxoi"
        public string? SMTP_Host { get; set; } // = "smtp.wh20.easyname.systems"
        public int? SMTP_Host_Port { get; set; } // = 465
    }
}
