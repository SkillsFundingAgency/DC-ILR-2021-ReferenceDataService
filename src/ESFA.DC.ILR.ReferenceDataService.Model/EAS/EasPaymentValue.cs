﻿using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Model.EAS
{
    public struct EasPaymentValue
    {
        public EasPaymentValue(decimal? paymentValue, IEnumerable<int> devolvedAraSofs)
        {
            PaymentValue = paymentValue;
            DevolvedAreaSofs = devolvedAraSofs;
        }

        public decimal? PaymentValue { get; set; }

        public IEnumerable<int> DevolvedAreaSofs { get; set; }
    }
}
