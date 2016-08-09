using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxlib
{
    public interface IState
    {
        void Enter(int ticks);
        bool Update(int ticks);             // 返回 false 表示要自杀. 需要把下个要进入的状态放入 NextState
        void Leave();

        //void Init(params object[] ps);    // 初始化接口( 从池中拿出来后用这个替代构造函数传参 )
        //void Dispose();                   // 析构接口（放入池之前执行用）
    }

    public abstract class StateBase : IState
    {
        public IState prevState;
        public IState currState;
        public Func<IState> nextState;

        public abstract void Enter(int ticks);     // init code here, 设初始状态，调其 Enter()
        public virtual void Leave() { }            // dispose code here

        // 调用子状态的代码
        public virtual bool Update(int ticks)
        {
            if (currState == null)
                return true;
            if (!currState.Update(ticks))
            {
                currState.Leave();
                if (nextState == null)
                    return false;
                prevState = currState;
                currState = nextState();
                if (currState == null)
                    return false;
                nextState = null;
                currState.Enter(ticks);
            }
            return true;
        }
    }

    // todo: StatePool ? StateSequense ? StateStack ?
}
