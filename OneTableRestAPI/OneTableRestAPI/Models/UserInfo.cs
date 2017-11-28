using System;
using System.Collections.Generic;

namespace OneTableRestAPI.Models
{
    public partial class UserInfo
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string AboutMe { get; set; }
    }
}
