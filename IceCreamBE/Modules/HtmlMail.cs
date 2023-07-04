namespace IceCreamBE.Modules
{
    public static class HtmlMail
    {
        public static string get(int randomNumber)
        {
            return $"<mjml> <mj-body background-color=\"#fafbfc\"> <mj-section padding-bottom=\"20px\" padding-top=\"20px\"> <mj-column vertical-align=\"middle\" width=\"100%\"> <mj-image align=\"center\" padding=\"25px\" src=\"https://uploads-ssl.webflow.com/5f059a21d0c1c3278fe69842/5f188b94aebb5983b66610dd_logo-arengu.png\" width=\"125px\"></mj-image> </mj-column> </mj-section> <mj-section background-color=\"#fff\" padding-bottom=\"20px\" padding-top=\"20px\"> <mj-column vertical-align=\"middle\" width=\"100%\"> <mj-text align=\"center\" font-size=\"16px\" font-family=\"open Sans Helvetica, Arial, sans-serif\" padding-left=\"25px\" padding-right=\"25px\"><span>Hello,</span></mj-text> <mj-text align=\"center\" font-size=\"16px\" font-family=\"open Sans Helvetica, Arial, sans-serif\" padding-left=\"25px\" padding-right=\"25px\">Please use the verification code below on the Arengu website:</mj-text> <mj-text align=\"center\" font-size=\"24px\" background-color=\"#20c997\" font-weight=\"bold\" font-family=\"open Sans Helvetica, Arial, sans-serif\">{randomNumber}</mj-text> <mj-text align=\"center\" font-size=\"16px\" font-family=\"open Sans Helvetica, Arial, sans-serif\" padding-left=\"25px\" padding-right=\"16px\">If you didn't request this, you can ignore this email or let us know.</mj-text> <mj-text align=\"center\" font-size=\"16px\" font-family=\"open Sans Helvetica, Arial, sans-serif\" padding-left=\"25px\" padding-right=\"25px\">Thanks! <br /></mj-text> </mj-column> </mj-section> </mj-body> </mjml>";
        }
    }
}
