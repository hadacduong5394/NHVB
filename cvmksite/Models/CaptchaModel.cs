namespace cvmksite.Models
{
    public class CaptchaModel
    {
        public string Hash { get; set; }

        public byte[] ImageByteArray { get; set; }
    }
}