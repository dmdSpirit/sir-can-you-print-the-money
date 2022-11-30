#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using UniRx;

namespace NovemberProject.TechTree
{
    public class TechController : InitializableBehaviour
    {
        private readonly Subject<Unit> _onTechUnlocked = new();
        private readonly ReactiveProperty<bool> _canRaiseSalary = new();
        private readonly ReactiveProperty<bool> _canLowerSalary = new();
        private readonly ReactiveProperty<bool> _canRaiseTax = new();
        private readonly ReactiveProperty<bool> _canLowerTax = new();
        private readonly ReactiveProperty<bool> _canPrintMoney = new();
        private readonly ReactiveProperty<bool> _canBurnMoney = new();
        private readonly ReactiveProperty<bool> _canUseMine = new();
        private readonly ReactiveProperty<bool> _canBuildArena = new();

        public IReadOnlyReactiveProperty<bool> CanRaiseSalary => _canRaiseSalary;
        public IReadOnlyReactiveProperty<bool> CanLowerSalary => _canLowerSalary;
        public IReadOnlyReactiveProperty<bool> CanRaiseTax => _canRaiseTax;
        public IReadOnlyReactiveProperty<bool> CanLowerTax => _canLowerTax;
        public IReadOnlyReactiveProperty<bool> CanPrintMoney => _canPrintMoney;
        public IReadOnlyReactiveProperty<bool> CanBurnMoney => _canBurnMoney;
        public IReadOnlyReactiveProperty<bool> CanUseMine => _canUseMine;
        public IReadOnlyReactiveProperty<bool> CanBuildArena => _canBuildArena;
        public IObservable<Unit> OnTechUnlocked => _onTechUnlocked;

        public void InitializeGameData()
        {
            _canRaiseSalary.Value = false;
            _canLowerSalary.Value = false;
            _canRaiseTax.Value = false;
            _canLowerTax.Value = false;
            _canPrintMoney.Value = false;
            _canBurnMoney.Value = false;
            _canBuildArena.Value = false;
            _canUseMine.Value = false;
        }

        public void UnlockRaiseSalary()
        {
            _canRaiseSalary.Value = true;
            _onTechUnlocked.OnNext(Unit.Default);
        }

        public void UnlockLowerSalary()
        {
            _canLowerSalary.Value = true;
            _onTechUnlocked.OnNext(Unit.Default);
        }

        public void UnlockRaiseTax()
        {
            _canRaiseTax.Value = true;
            _onTechUnlocked.OnNext(Unit.Default);
        }

        public void UnlockLowerTax()
        {
            _canLowerTax.Value = true;
            _onTechUnlocked.OnNext(Unit.Default);
        }

        public void UnlockPrintMoney()
        {
            _canPrintMoney.Value = true;
            _onTechUnlocked.OnNext(Unit.Default);
        }

        public void UnlockBurnMoney()
        {
            _canBurnMoney.Value = true;
            _onTechUnlocked.OnNext(Unit.Default);
        }

        public void UnlockBuildArena()
        {
            _canBuildArena.Value = true;
            _onTechUnlocked.OnNext(Unit.Default);
        }

        public void UnlockMine()
        {
            _canUseMine.Value = true;
            _onTechUnlocked.OnNext(Unit.Default);
        }
    }
}