namespace Anresh.Application
{
    public class Token
    {
        public string Key { get; set; }
    }
    public class ConnectionStrings
    {
        public string SqlServerDb {  get; set; }
    }
    public class MailSetings
    {
        public string Email {  get; set; }
        public string Password {  get; set; }
        public string Name {  get; set; }
        public string Smtp {  get; set; }
        public int Port { get; set; }
    }
    public class Options
    {
        public string ApiUri { get; set; }
        public string FrontUri {  get; set; }
        public string FilePath { get; set; }
        public Token Token {  get; set; }
        public ConnectionStrings ConnectionStrings {  get; set; }
        public MailSetings MailSettings {  get; set; }
    }
}
