using CounterStrikeSharp.API.Core;
using System.Text.Json.Serialization;

namespace Stafflist
{
    public class StafflistConfig : BasePluginConfig
    {

		[JsonPropertyName("StaffCommand")]
		public string StaffCommand { get; set; } = "css_staff";

        [JsonPropertyName("HideCommand")]
        public string HideCommand { get; set; } = "css_hideme";

        [JsonPropertyName("HideCommandFlag")]
        public string HideCommandFlag { get; set; } = "@css/ban";

        [JsonPropertyName("Immunity")]
        public string Immunity { get; set; } = "@css/root";

        [JsonPropertyName("ShowOnList")]
        public string ShowOnList { get; set; } = "@css/ban";

    }
}
