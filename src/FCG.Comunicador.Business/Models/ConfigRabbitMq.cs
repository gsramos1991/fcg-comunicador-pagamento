using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Comunicador.Business.Models
{
    public class ConfigRabbitMq
    {
        public static string HostName { get; set; } = string.Empty;
        public static string Username {  get; set; } = string.Empty;
        public static string Password { get; set; } = string.Empty;
    }
}