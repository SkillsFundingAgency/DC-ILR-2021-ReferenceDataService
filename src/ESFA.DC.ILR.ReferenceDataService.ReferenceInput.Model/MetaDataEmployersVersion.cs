﻿using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaDataEmployersVersion
    {
        public MetaDataEmployersVersion()
        {
            MetaDataReferenceDataVersion = new HashSet<MetaDataReferenceDataVersion>();
        }

        public int Id { get; set; }
        public string Version { get; set; }

        public virtual ICollection<MetaDataReferenceDataVersion> MetaDataReferenceDataVersion { get; set; }
    }
}
