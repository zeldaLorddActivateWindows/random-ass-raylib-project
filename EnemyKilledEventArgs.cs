using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{ 
    internal class EnemyKilledEventArgs : EventArgs
    {
        public float EnemySize { get; }

        public EnemyKilledEventArgs(float enemySize)
        {
            EnemySize = enemySize;
            
        }
    }
}
