using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CreatingA2DSprite

{
   
     public  class Input
        {
            KeyboardState keyBoard;
            KeyboardState lastKeyBoard = Keyboard.GetState();
           
            public void Update()
            {
                lastKeyBoard = keyBoard;
                keyBoard = Keyboard.GetState();
            }
            public bool Press(Keys key)
            {
                return keyBoard.IsKeyDown(key);
            }
            public bool Release(Keys key)
            {
                return (keyBoard.IsKeyUp(key) && lastKeyBoard.IsKeyDown(key));
            }
        }
    
}
