using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CreatingA2DSprite;

namespace Object
{
    class Camera
    {
        public Matrix transform;
        Viewport view;
        
        public Camera(Viewport newView)
        {
           this.view = newView;
        }
        public void Update(GameTime gameTime, Game1 ship,  Vector2 centre , float zoom , int left)
        {
            
            transform = Matrix.CreateScale(new Vector3(1.95f, 1.95f , 0)) *
                        Matrix.CreateTranslation(new Vector3(    Building.CELL_WIDTH  - 2*(int)(zoom*Building.CELL_WIDTH) - left + 30 , 1.5f * (-centre.Y)    , 0)) * 
                        Matrix.CreateScale(new Vector3(zoom, zoom, zoom));
        }
    }
}
