using MonkeyWallet.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyWallet.Desktop.Models;

public class SelectedWalletState
{
    private Wallet? _wallet { get; set; }

    public Wallet? GetWallet() => _wallet;
    public void SetWallet(Wallet? wallet)
    {
        _wallet = wallet;
    }

}
