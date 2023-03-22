using CardanoSharp.Wallet;
using CardanoSharp.Wallet.Enums;
using CardanoSharp.Wallet.Extensions.Models;
using MonkeyWallet.Core.Common;
using MonkeyWallet.Core.Data;
using MonkeyWallet.Core.Data.Models;
using SQLite;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonkeyWallet.Core.Services
{
    public interface IWalletService
    {
        Task AddWallet(string name, string recoveryPhrase, string spendingPassword);
    }

    public class WalletService : IWalletService
    {
        private IMnemonicService _mnemonicService;
        private IWalletDatabase _walletDatabase;
        private IWalletKeyDatabase _walletKeyDatabase;

        public WalletService(
            IMnemonicService mnemonicService,
            IWalletKeyDatabase walletKeyDatabase,
            IWalletDatabase walletDatabase)
        {
            _mnemonicService = mnemonicService;
            _walletDatabase = walletDatabase;
            _walletKeyDatabase = walletKeyDatabase;
        }

        public async Task AddWallet(string name, string recoveryPhrase, string spendingPassword)
        {
            // Restore a Mnemonic
            var mnemonic = _mnemonicService.Restore(recoveryPhrase);
            Wallet? newlyCreatedWallet;

            if (await _walletDatabase.ExistsAsync(name))
            {
                throw new Exception("Wallet already exists");
            }

            int accountIx = 0;

            await _walletDatabase.SaveAsync(new Wallet
            {
                Name = name,
                WalletType = (int)WalletType.HD,
            });

            newlyCreatedWallet = await _walletDatabase.GetByNameAsync(name);

            var accountNode = mnemonic.GetMasterNode()
                .Derive(PurposeType.Shelley)
                .Derive(CoinType.Ada)
                .Derive(accountIx);
            accountNode.SetPublicKey();

            await _walletKeyDatabase.SaveWalletAsync(new WalletKey
            {
                WalletId = newlyCreatedWallet.Id,
                KeyType = (int)KeyType.Account,
                Skey = JsonSerializer.Serialize(accountNode.PrivateKey.Encrypt(spendingPassword)),
                Vkey = JsonSerializer.Serialize(accountNode.PublicKey),
                KeyIndex = accountIx,
                AccountIndex = accountIx
            });
        }
    }
}
