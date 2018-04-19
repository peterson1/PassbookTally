using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib45.LiteDbTools;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonTools.Lib45.BaseViewModels
{
    [AddINotifyPropertyChangedInterface]
    public abstract class SavedListVMBase<TDTO, TArg>
        where TDTO : IDocumentDTO
        where TArg : ICredentialsProvider
    {
        public event EventHandler<decimal>       TotalSumChanged;

        private SharedCollectionBase<TDTO> _repo;


        public SavedListVMBase(SharedCollectionBase<TDTO> sharedCollection, TArg appArguments, bool doReload = true)
        {
            _repo     = sharedCollection;
            AppArgs = appArguments;

            _repo.ContentChanged          += (s, e) => ReloadFromDB();
            ItemsList.ItemDeleted       += (s, e) => ExecuteDeleteRecord(e);
            ItemsList.CollectionChanged += (s, e) => UpdateTotalSum();
            ItemsList.ItemOpened        += ItemsList_ItemOpened;

            if (doReload) ReloadFromDB();
        }


        public TArg          AppArgs    { get; }
        public UIList<TDTO>  ItemsList  { get; } = new UIList<TDTO>();
        public decimal       TotalSum   { get; private set; }


        protected abstract Func<TDTO, decimal> SummedAmount { get; }


        private void ExecuteDeleteRecord(TDTO dto)
        {
            DeleteRecord(_repo, dto);
            UpdateTotalSum();
            TotalSumChanged?.Invoke(this, TotalSum);
        }


        protected virtual void DeleteRecord(SharedCollectionBase<TDTO> db, TDTO dto) 
            => db.Delete(dto);


        private void ItemsList_ItemOpened(object sender, TDTO e)
        {
            OnItemOpened(e);
            UpdateTotalSum();
        }


        protected virtual void OnItemOpened(TDTO e)
        {
        }


        public void ReloadFromDB()
            => ItemsList.SetItems(PostProcess(QueryItems(_repo)));


        protected virtual List<TDTO> QueryItems(SharedCollectionBase<TDTO> db)
            => db.GetAll();


        protected virtual IEnumerable<TDTO> PostProcess(IEnumerable<TDTO> items) 
            => items;


        private void UpdateTotalSum()
        {
            if (SummedAmount == null) return;
            if (!ItemsList.Any()) return;

            var oldSum = TotalSum;
            TotalSum = ItemsList.Sum(_ => SummedAmount(_));

            if (TotalSum != oldSum)
                TotalSumChanged?.Invoke(this, TotalSum);
        }


        //public void RaisePropertyChanged(object sender, PropertyChangedEventArgs e)
        //    => PropertyChanged?.Invoke(sender, e);
    }
}
