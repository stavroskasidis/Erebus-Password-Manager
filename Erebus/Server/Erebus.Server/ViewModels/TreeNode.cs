using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.ViewModels
{
    public class TreeNode
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "parent")]
        public string Parent { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "state")]
        public State State { get; set; }

        [JsonProperty(PropertyName = "li_attr")]
        public Dictionary<string, string> LiAttributes { get; set; }

        [JsonProperty(PropertyName = "a_attr")]
        public Dictionary<string, string> AnchorAttributes { get; set; }

        [JsonProperty(PropertyName = "children")]
        public bool HasChildren { get; set; }

        public TreeNode()
        {
            this.State = new State();
            this.LiAttributes = new Dictionary<string, string>();
            this.AnchorAttributes = new Dictionary<string, string>();
        }
    }

    public class State
    {
        [JsonProperty(PropertyName = "opened")]
        public bool Opened { get; set; }

        [JsonProperty(PropertyName = "disabled")]
        public bool Disabled { get; set; }

        [JsonProperty(PropertyName = "selected")]
        public bool Selected { get; set; }
    }
}
