namespace _Scripts.BattleScripts
{
    public interface IDamageTaker
    {
        public void TakeDamage(float damageAmount);
        public bool IsAlive { get; }
    }
}