using System.Text;

namespace IceCreamBE.Modules
{
    public static class Coupon
    {
        public static String CouponGenarate(int length)
        {
            const string chars = "0123456789" + "QWERTYUIOPASDFGHJKLZXCVBNM" + "qwertyuiopasdfghjklzxcvbnm";
            StringBuilder strbr = new StringBuilder();
            Random rdm = new Random();
            for(int i=0; i < length; i++)
            {
                int index = rdm.Next(chars.Length);
                strbr.Append(chars[index]);
            }

            return strbr.ToString();
        }
    }
}
