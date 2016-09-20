using Erebus.Core.Mobile.Contracts;
using Erebus.Mobile.Presenters.Contracts;
using Erebus.Mobile.Views.Contracts;
using Erebus.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Mobile.Presenters.Implementations
{
    public class EntryDetailsPresenter : IEntryDetailsPresenter
    {
        private IEntryDetailsView View;
        private Model.Entry Entry;
        private IClipboardService ClipboardService;
        private IAlertDisplayer AlertDisplayer;

        public EntryDetailsPresenter(IEntryDetailsView view, IClipboardService clipboardService, IAlertDisplayer alertDisplayer, Model.Entry entry)
        {
            this.View = view;
            this.Entry = entry;
            this.ClipboardService = clipboardService;
            this.AlertDisplayer = alertDisplayer;

            this.View.Entry = entry;
            this.View.ShowHidePassword += OnShowHidePassword;
            this.View.CopyPasswordToClipboard += OnCopyPasswordToClipboard;
        }

        public object GetView()
        {
            return View;
        }

        public void OnCopyPasswordToClipboard()
        {
            this.ClipboardService.CopyToClipboard(this.Entry.Password);
            this.AlertDisplayer.DisplayAlert("", StringResources.CopiedAlertMessage);
        }

        public void OnShowHidePassword()
        {
            this.View.IsPasswordVisible = !this.View.IsPasswordVisible;
        }
    }
}
