using Game.Scripts.SaveLoad.DataProviders;
using SaveLoad;

namespace Game.Gameplay
{
    public class WalletDataProvider : GameDataProvider<Wallet, int>
    {
        public override int GetData(ISaveLoadContext context)
        {
            var wallet = GetModel(context);
           
            return wallet.Coins;
        }

        public override void SetData(int coins, ISaveLoadContext context)
        {
            var wallet = GetModel(context);
            wallet.Coins = coins;
        }
    }
}