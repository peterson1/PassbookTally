using PassbookTally.DatabaseLib.Repositories;
using PassbookTally.DomainLib.DTOs;
using CommonTools.Lib11.JsonTools;
using System;

namespace PassbookTally.DatabaseLib.StateTransitions
{
    public static class ChequeStateTransitioner
    {
        public static void ToPreparedCheque(this PassbookDB pbk, FundRequestDTO req, int chequeNumber, DateTime chequeDate)
        {
            //if (ChequeExists(chequeNumber, pbk)) return;
            pbk.ActiveCheques.Insert(new RequestedChequeDTO
            {
                Request       = req,
                ChequeNumber  = chequeNumber,
                ChequeDate    = chequeDate,
            });
            pbk.InactiveRequests.Insert(req);
            pbk.ActiveRequests.Delete(req);
        }


        //todo: implem this
        //private static bool ChequeExists(int chequeNumber, PassbookDB pbk)
        //{
        //    //var exists = pbk.InactiveCheques
        //    throw new NotImplementedException();
        //}


        public static void ToIssuedCheque(this PassbookDB pbk, RequestedChequeDTO cheque, string issuedTo, DateTime issuedDate)
        {
            cheque.IssuedTo   = issuedTo;
            cheque.IssuedDate = issuedDate;
            pbk.ActiveCheques.Update(cheque);
        }


        public static void ToBankTransaction(this PassbookDB pbk, RequestedChequeDTO chq, DateTime clearedDate, SoaRowsRepo1 txnsRepo)
        {
            //txnsRepo.Withdraw(clearedDate, req.Payee, req.Purpose, req.Amount.Value, chq.ChequeNumber.ToString());
            var dto = ToClearedTransaction(chq, clearedDate);
            txnsRepo.UpsertAndUpdateBalances(dto);
            //pbk.InactiveCheques.Insert(chq);
            pbk.ActiveCheques.Delete(chq);
        }


        private static SoaRowDTO ToClearedTransaction(RequestedChequeDTO chq, DateTime clearedDate)
        {
            var req = chq.Request;
            var dto = SoaRowDTO.Withdrawal(clearedDate,
                        req.Payee, req.Purpose, req.Amount.Value,
                        chq.ChequeNumber.ToString());
            dto.DocRefId   = chq.Id;
            dto.DocRefType = chq.GetType().FullName;
            dto.DocRefJson = chq.ToJson(true);
            return dto;
        }
    }
}
