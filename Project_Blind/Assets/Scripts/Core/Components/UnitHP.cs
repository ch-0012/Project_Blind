namespace Blind
{
    /// <summary>
    /// 유닛의 체력을 관리하는 클래스입니다. 
    /// </summary>
    public class UnitHP : IDamageable<float>
    {
        private float _health;
        private float _maxHealth;

        public UnitHP(int maxHealth) {
            _maxHealth = maxHealth;
            _health = _maxHealth;
        }
        public void GetDamage(float damage) {
            _health -= damage;
        }
        public void GetHeal(float heal) {
            if(_health + heal > _maxHealth) {
                _health = _maxHealth;
                return;
            }
            _health += heal;
        }
        public float GetHP() {
            return _health;
        }
    }
}