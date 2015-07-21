using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Animation
{
    public class Sprite
    {
        public int numberFrame;             // Số frame của sprite
        public int x, y;                    // Tọa độ hiện tại của góc trái trên của sprite
        public int width, height;           // Chiều ngang, chiều dọc của sprite
        public int widthDraw, heightDraw;   // Chiều ngang, chiều dọc anition đk vẽ
        public int currentFrame;           // Frame đang được vẽ

        public float TimePerFrame;
        public float TotalElapsed;

        public Texture2D mSpriteTexture;    // 

        public Sprite(ContentManager theContentManager, int x, int y, int numberFrame, int FramePerSecond, string theAssetName)
        {
            this.mSpriteTexture = LoadTextureF(theContentManager, theAssetName);
            System.Console.WriteLine("Width: " + this.mSpriteTexture.Width);
            System.Console.WriteLine("Height: " + this.mSpriteTexture.Height);
            this.setX(x);
            this.setY(y);
            this.setWidth(this.mSpriteTexture.Width / numberFrame);
            this.setHeight(this.mSpriteTexture.Height);
            this.numberFrame = numberFrame;
            this.TimePerFrame = (float)1 / FramePerSecond;
            this.TotalElapsed = 0f;  
        }

        public int getX()
        {
            return this.x;
        }

        public int getY()
        {
            return this.y;
        }

        public int getCurrentFrame()
        {
            return this.currentFrame;
        }

        public int getWidth()
        {
            return this.width;
        }

        public int getHeight()
        {
            return this.height;
        }

        public int getWidthDraw()
        {
            return this.widthDraw;
        }

        public int getHeightDraw()
        {
            return this.heightDraw;
        }

        public void setX(int x)
        {
            this.x = x;
        }

        public void setY(int y)
        {
            this.y = y;
        }

        public void setCurrentFrame(int theCurrentFrame)
        {
            this.currentFrame = theCurrentFrame;
        }

        public void setWidth(int theWidth)
        {
            this.width = theWidth;
        }

        public void setHeight(int theHeight)
        {
            this.height = theHeight;
        }

        public void setWidthDraw(int theWidthDraw)
        {
            this.widthDraw = theWidthDraw;
        }

        public void setHeightDraw(int theHeightDraw)
        {
            this.heightDraw = theHeightDraw;
        }

        public Rectangle getBound()
        {
            return new Rectangle(this.getX(), this.getY(), this.getWidthDraw(), this.getHeightDraw());
        }

        public Boolean CheckCollides(Sprite sprite)
        {
            return this.getBound().Intersects(sprite.getBound());
        }

        /*********************************************/
        //Load the texture for the sprite using the Content Pipeline
        public static Texture2D LoadTextureF(ContentManager theContentManager, string theAssetName)
        {
            Texture2D texture = null;
            try
            {
                texture = theContentManager.Load<Texture2D>(theAssetName);
            }
            catch (ObjectDisposedException e) { System.Console.WriteLine("ObjectDisposedException " + theAssetName); }
            catch (ArgumentNullException e) { System.Console.WriteLine("ArgumentNullException " + theAssetName); }
            catch (ContentLoadException e) { System.Console.WriteLine("ContentLoadException: " + theAssetName); }
            System.Console.WriteLine("Loaded image! " + theAssetName);

            return texture;
        }

        public void LoopFrameAToFrameB(int frameA, int frameB)
        {
            if( this.currentFrame < frameA || this.currentFrame > frameB ) this.setCurrentFrame(frameA);
            else{
            this.currentFrame++;
            if (this.currentFrame == frameB+1) this.currentFrame = frameA ;
            }
        }

        public void GotoAndStopFrame(int frame)
        {
            // Chi su dung cho currentFrame < frameMax
            if (this.currentFrame >= frame) ;
            else this.currentFrame++;
        }

        public void GotoAndStopFrameBack(int frame)
        {
            if (this.currentFrame <= frame) ;
            else this.currentFrame--;
        }
        
        // Vẽ sprite 
        public void DrawFrame(SpriteBatch theSpriteBatch, int frame, Boolean theIsLeft)
        { 
            Rectangle src = new Rectangle(frame * this.width, 0, this.width, this.height);
            //this.widthDraw = this.width;
            //this.heightDraw = this.height;
            Rectangle dst = new Rectangle(this.x, this.y, this.widthDraw, this.heightDraw);
   
            try
            {
                if (!theIsLeft) theSpriteBatch.Draw(mSpriteTexture, dst, src, Color.White);
                else theSpriteBatch.Draw(mSpriteTexture, dst, src, Color.White, 0, new Vector2(0,0),
                                        SpriteEffects.FlipHorizontally, 1);
            }
            catch (ArgumentNullException e) { System.Console.WriteLine("ArgumentNullException"); }
            catch (InvalidOperationException e) { System.Console.WriteLine("InvalidOperationException"); }    
        }

        /* Tao random 1 doi tuong moi */
        
    }
}
