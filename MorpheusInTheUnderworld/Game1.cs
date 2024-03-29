﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens;
using MorpheusInTheUnderworld.Screens;
using MorpheusInTheUnderworld.Classes;
using System.IO;
using MonoGame.Extended.Screens.Transitions;
using GeonBit.UI;

namespace MorpheusInTheUnderworld
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // A FramePerSecondCounter used only in Debug Mode.
        FramesPerSecondCounter fps;

        BitmapFont bitmapFont;

        Texture2D circle32;

        private readonly ScreenManager screenManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024; // set screen dimensions 4:3 as per Bark's sketch
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            MusicPlayer.Initialize();
            string currentDir = Directory.GetCurrentDirectory();
            MusicPlayer.AddSong(currentDir + "\\Content\\117BPMKickin_new.mp3");
            MusicPlayer.BPM = 117;

            screenManager = Components.Add<ScreenManager>();
            screenManager.DrawOrder = 99;
            GameSettings.Read();

            MusicPlayer.MasterVolume = GameSettings.MasterVolume / 100f;
            MusicPlayer.MusicVolume = (GameSettings.MusicVolume/100f) * (MusicPlayer.MasterVolume);
            MusicPlayer.SFXVolume = (GameSettings.EffectsVolume/100f) * (MusicPlayer.MasterVolume);
           
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            fps = new MonoGame.Extended.FramesPerSecondCounter();
            IsMouseVisible = false; // avoids doubled mouse pointer
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bitmapFont = Content.Load<BitmapFont>("Fonts/fixedsys");
            circle32 = Content.Load<Texture2D>("Graphics/circle32");
            UserInterface.Initialize(Content, BuiltinThemes.editor);
            screenManager.LoadScreen(new MainMenuScreen(this), new FadeTransition(GraphicsDevice, Color.Black, 0.5f));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MusicPlayer.Update(gameTime);
            fps.Update(gameTime);

            UserInterface.Active.Update(gameTime);

            MusicPlayer.MasterVolume = GameSettings.MasterVolume / 100f;
            MusicPlayer.MusicVolume = (GameSettings.MusicVolume/100f) * (MusicPlayer.MasterVolume);
            MusicPlayer.SFXVolume = (GameSettings.EffectsVolume/100f) * (MusicPlayer.MasterVolume);

        }


        float elapsed;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Viewport viewport = GraphicsDevice.Viewport;
            base.Draw(gameTime);


           // Only draw if we are Debugging
            #if DEBUG
            fps.Draw(gameTime);
            string fpsText = "FPS: " + fps.FramesPerSecond;
            float fpsTextWidth = bitmapFont.MeasureString(fpsText).Width;
            spriteBatch.Begin();
            spriteBatch.DrawString(bitmapFont, fpsText, new Vector2(viewport.Width - fpsTextWidth, 0), Color.White);
            spriteBatch.End();
            #endif
            if(MusicPlayer.GotBeat())
            {
                spriteBatch.Begin();
                spriteBatch.Draw(circle32, new Rectangle((int)(viewport.Width - 256), 8, 16, 16), Color.Red);
                spriteBatch.End();
            }

            UserInterface.Active.Draw(spriteBatch);
        }
    }
}
