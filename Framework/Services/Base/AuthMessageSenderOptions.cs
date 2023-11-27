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
        public string? Sender_Display_Name { get; set; } // = "Todoos.net"
        public string? Todoos_Key { get; set; } // = "AKIASQISUTS4PU2MVGPB"
        public string? Todoos_Sectret_Key { get; set; } // = "sOWtC4p86eBqJrY5rdwjIkzu8L3sDbi4vXLcH62r"
    }
}
