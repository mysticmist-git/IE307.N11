using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace DoAn_IE307_N11.ViewModels
{
    public class NoteViewModel : BaseViewModel
    {
        public object Parent { get; set; }
        public string Note { get; set; }
        public NoteViewModel(object parent)
        {
            Parent = parent;
        }
    }
}
