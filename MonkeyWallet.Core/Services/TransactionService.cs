using CardanoSharp.Koios.Client;
using CardanoSharp.Wallet.Models.Transactions;
using MonkeyWallet.Core.Data.Models;
using CardanoSharp.Wallet.TransactionBuilding;
using MonkeyWallet.Core.Data;

namespace MonkeyWallet.Core.Services
{
    public interface ITransactionService
    {
        Task<Transaction?> BuildTransaction(Wallet wallet, List<TransactionOutput> outputs);
        Task<Transaction?> SignTransaction(string password);
    }

    public class TransactionService : ITransactionService
    {
        private readonly ITransactionClient _transactionClient;
        private readonly IEpochClient _epochClient;
        private readonly IAddressClient _addressClient;
        private readonly INetworkClient _networkClient;
        private readonly IAddressService _addressService;
        private readonly IWalletKeyDatabase _walletKeyDatabase;


        public TransactionService(ITransactionClient transactionClient, 
            IEpochClient epochClient, 
            IAddressClient addressClient, 
            IAddressService addressService, 
            INetworkClient networkClient, 
            IWalletKeyDatabase walletKeyDatabase)
        {
            _transactionClient = transactionClient;
            _epochClient = epochClient;
            _addressClient = addressClient;
            _addressService = addressService;
            _networkClient = networkClient;
            _walletKeyDatabase = walletKeyDatabase;
        }

        public async Task<Transaction?> BuildTransaction(Wallet wallet, List<TransactionOutput> outputs)
        {
            var transaction = new Transaction();

            ///2. Create the Body
            var transactionBody = TransactionBodyBuilder.Create;

            return transaction;
        }

        public Task<Transaction?> SignTransaction(string password)
        {
            throw new NotImplementedException();
        }
    }
}
