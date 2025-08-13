using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DribionPayExemplos.Model
{
    public class AutenticarDribionPay
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public int expiresIn { get; set; }
        public int expiresRefreshTokenIn { get; set; }
        public string role { get; set; }
    }
}
