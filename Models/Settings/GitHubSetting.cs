﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mocker.Models.Settings
{
    public class GitHubSetting
    {
        public long RepositoryID { get; set; }
        public string Branch { get; set; }
        public string HttpMethodsFolderPath { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
    }
}
