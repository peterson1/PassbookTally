using PassbookTally.DatabaseLib.Repositories;
using PassbookTally.DomainLib.DTOs;
using System;

namespace PassbookTally.DatabaseLib.StateTransitions
{
    public static class ChequeStateTransitioner
    {
        public static void ToPreparedCheque(this PassbookDB pbk, FundRequestDTO req, int chequeNumber, DateTime chequeDate)
        {
            pbk.ActiveCheques.Insert(new RequestedChequeDTO
            {
                Request       = req,
                ChequeNumber  = chequeNumber,
                ChequeDate    = chequeDate,
            });
            pbk.InactiveRequests.Insert(req);
            pbk.ActiveRequests.Delete(req);
        }


        public static void ToIssuedCheque(this PassbookDB pbk, RequestedChequeDTO cheque, string issuedTo, DateTime issuedDate)
        {
            cheque.IssuedTo   = issuedTo;
            cheque.IssuedDate = issuedDate;
            pbk.ActiveCheques.Update(cheque);
        }


        public static void ToBankTransaction(this PassbookDB pbk, RequestedChequeDTO chq, DateTime clearedDate, SoaRowsRepo1 txnsRepo)
        {
            var req = chq.Request;
            txnsRepo.Withdraw(clearedDate, req.Payee, req.Purpose, req.Amount.Value, chq.ChequeNumber.ToString());
            pbk.InactiveCheques.Insert(chq);
            pbk.ActiveCheques.Delete(chq);
        }
    }
}
