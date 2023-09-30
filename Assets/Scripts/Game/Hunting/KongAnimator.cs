
namespace Game.Hunting
{
    public class KongAnimator : PreyAnimator
    {

        public void KongFree()
        {
            _animator.SetTrigger(_runKey);
        }
    }
}