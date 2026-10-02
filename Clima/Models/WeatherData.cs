using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clima.Models
{
    public class WeatherData
    {
        public string City { get; set; } = string.Empty;
        public string Temperature { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty; // Símbolos Unicode o nombres de imagen
        public string Humidity { get; set; } = string.Empty;
        public string WindSpeed { get; set; } = string.Empty;
        public Color BackgroundColor { get; set; } = Colors.SteelBlue;
    }
}
