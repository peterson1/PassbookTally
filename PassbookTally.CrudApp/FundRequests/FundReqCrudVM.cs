﻿using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using PassbookTally.DatabaseLib;
using PassbookTally.DatabaseLib.Repositories;
using PassbookTally.DomainLib.Authorization;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib45.Configuration;
using System;

namespace PassbookTally.CrudApp.FundRequests
{
    public class FundReqCrudVM : CrudWindowVMBase<FundRequestDTO, FundReqCrudWindow, AppArguments>
    {
        public    override string TypeDescription => "Voucher Request";
        protected override string CaptionPrefix   => "Voucher Request";

        //private PassbookDB         _db;
        //private ActiveFundReqsRepo _repo;
        private int                _savedSerial;


        public FundReqCrudVM(AppArguments appArguments) : base(appArguments)
        {
            //_db   = passbookDB;
            //_repo = appArguments.PassbookDB.ActiveRequests;
        }



        public UIList<string>  Payees  { get; } = new UIList<string>();

        private PassbookDB         DB   => AppArgs.PassbookDB;
        private ActiveFundReqsRepo Repo => DB.ActiveRequests;



        protected override void SetNewDraftDefaults(FundRequestDTO draft)
        {
            draft.RequestDate = DateTime.Now;
            draft.SerialNum   = DB.NextRequestSerial();
            Payees.SetItems(DB.GetPayees());
        }


        protected override bool CanEncodeNewDraft()
            => AppArgs.CanAddVoucherRequest(false);


        protected override void SaveNewRecord(FundRequestDTO draft) 
            => Repo.Insert(draft);


        protected override void UpdateRecord(FundRequestDTO record)
            => Repo.Update(record);


        protected override bool IsValidDraft(FundRequestDTO draft, out string whyInvalid)
        {
            if (IsDuplicateSerial(draft))
            {
                whyInvalid = "Serial number is used in another request.";
                return false;
            }

            //if (draft.Amount == 0)
            //{
            //    whyInvalid = "Amount should not be zero.";
            //    return false;
            //}
            if (draft.Payee.IsBlank())
            {
                whyInvalid = "Payee should not be blank.";
                return false;
            }
            if (draft.Purpose.IsBlank())
            {
                whyInvalid = "Purpose should not be blank.";
                return false;
            }
            whyInvalid = "";
            return true;
        }


        protected override FundRequestDTO CreateDraftFromRecord(FundRequestDTO record)
        {
            _savedSerial = record.SerialNum;
            return base.CreateDraftFromRecord(record);
        }


        private bool IsDuplicateSerial(FundRequestDTO draft)
        {
            if (draft.Id == 0)
                return DB.HasRequestSerial(draft.SerialNum);

            if (_savedSerial == draft.SerialNum)
                return false;

            return DB.HasRequestSerial(draft.SerialNum);
        }
    }
}
