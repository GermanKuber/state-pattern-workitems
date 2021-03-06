using Xunit;

namespace AtmDemo.Tests
{

    public class AccountTestsShould
    {
        private Account _sut;
        private AtmMachine _atmMachine;

        public AccountTestsShould()
        {
            _sut = new Account(false, false, false, 1000, "Germ�n K�ber", () => { });

            _atmMachine = new AtmMachine(_sut);
        }
        [Fact]
        public void Cant_Deposit_Money_Is_Not_Closed()
        {
            _sut = new Account(true, false, false, 1000, "Germ�n K�ber", () => { });

            _atmMachine = new AtmMachine(_sut);

            Assert.Throws<AccountClosedException>(() => _atmMachine.Deposit(100));
        }
        [Fact]
        public void Cant_Deposit_Money_Is_Not_Verified()
        {
            Assert.Throws<AccountNotVerifiedException>(() => _atmMachine.Deposit(100));
        }
        [Fact]
        public void Can_Deposit_Money()
        {
            _atmMachine.HolderVirfied();
            _atmMachine.Deposit(100);
            Assert.Equal(1100, _atmMachine.Summary());
        }

        [Fact]
        public void Cant_WithDraw_Money_Is_Not_Closed()
        {
            _sut = new Account(true, false, false, 1000, "Germ�n K�ber", () => { });

            _atmMachine = new AtmMachine(_sut);
            Assert.Throws<AccountClosedException>(() => _atmMachine.WithDraw(100));
        }
        [Fact]
        public void Cant_WithDraw_Money_Is_Not_Verified()
        {
            Assert.Throws<AccountNotVerifiedException>(() => _atmMachine.WithDraw(100));
        }
        [Fact]
        public void Can_WithDraw_Money()
        {
            _atmMachine.HolderVirfied();
            _atmMachine.WithDraw(100);
            Assert.Equal(900, _atmMachine.Summary());
        }

        [Fact]
        public void Cant_Verified_Account_Is_Not_Open()
        {
            _sut = new Account(true, false, false, 1000, "Germ�n K�ber", () => { });

            _atmMachine = new AtmMachine(_sut);
            Assert.Throws<AccountClosedException>(() => _atmMachine.HolderVirfied());
        }
        [Fact]
        public void Cant_Close_Account_Has_Money()
        {
            Assert.Throws<AccountHasMoneyException>(() => _atmMachine.CloseAccount());
        }
        [Fact]
        public void Can_Close_Account()
        {
            AccountAlreadyToOperate();
            _atmMachine.WithDraw(1000);
            _atmMachine.CloseAccount();

            Assert.Equal(AccountState.Close, _atmMachine.StateAccount());
        }

        [Fact]
        public void Execute_UnFrozen()
        {
            var execute = false;
            var sut = new Account(true, true, true, 1000, "Germ�n K�ber - Demo", () => { execute = true; });
            var atmMachine = new AtmMachine(sut);

            atmMachine.WithDraw(1000);

            Assert.True(execute);
        }

        private void AccountAlreadyToOperate() => _atmMachine.HolderVirfied();
    }
}