﻿using Counter.SDK.SharedModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nozomi.Data.WebModels
{
    public class Request : BaseEntityModel
    {
        public long Id { get; set; }

        public Guid Guid { get; set; }

        public RequestType RequestType { get; set; }

        /// <summary>
        /// URL.
        /// </summary>
        public string DataPath { get; set; }

        public ICollection<RequestComponent> RequestComponents { get; set; }
        public ICollection<RequestProperty> RequestProperties { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(DataPath) && !string.IsNullOrWhiteSpace(DataPath)
                    && RequestType >= 0);
        }
    }
}
