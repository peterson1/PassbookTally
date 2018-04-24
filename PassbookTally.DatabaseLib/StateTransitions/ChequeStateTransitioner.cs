using PassbookTally.DomainLib.DTOs;
using System;

namespace PassbookTally.DatabaseLib.StateTransitions
{
    public static class ChequeStateTransitioner
    {
        public static void ToActiveCheque(this PassbookDB pbk, FundRequestDTO req, int chequeNumber, DateTime chequeDate)
        {
            pbk.ActiveCheques.Insert(new RequestedChequeDTO
            {
                Request      = req,
                ChequeNumber = chequeNumber,
                ChequeDate   = chequeDate,
            });
            pbk.InactiveRequests.Insert(req);
            pbk.ActiveRequests.Delete(req);
        }
    }
}
