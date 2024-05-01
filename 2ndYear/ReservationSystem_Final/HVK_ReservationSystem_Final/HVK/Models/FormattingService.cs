namespace HVK.Models
{
    public class FormattingService
    {

        public string DateDisplay(DateTime Date)
        {
            return Date.ToString("yyyy/MM/dd");
        }

        public string PostalCode(string? code)
        {
            code = code.Trim();
            return code != null ? string.Format("(0) {1}", code.Substring(0, 3), code.Substring(3, 3)) : "";
        }
        
        public string PhoneDisplay(string? tel)
        {
            tel = tel.Trim();
            return tel != null ? string.Format("({0}) {1}-{2}", tel.Substring(0, 3), tel.Substring(3, 3), tel.Substring(6, 4)) : "";
        }


                
           
    }
}
