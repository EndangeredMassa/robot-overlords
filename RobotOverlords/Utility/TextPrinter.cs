using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace RobotOverlords.Utility
{
    /// <summary>
    /// The TextPrinter class can be used to simplify drawing text in XNA games.  This class was developed
    /// as a tutorial on http://www.xnaresources.com by Kurt Jaegers. Copyright 2009 by Kurt Jaegers.
    ///
    /// This code can be freely used in any project, both commercial and non-commercial.
    /// </summary>
    static class TextPrinter
    {
        #region Declarations and Properties

        // A dictionary to hold all of the SpriteFont objects we may use to draw text
        private static readonly Dictionary<string, SpriteFont> _fonts = new Dictionary<string, SpriteFont>();

        // The current font we are drawing in when the call to DrawText does not include a font
        // specification (Can be overridden by specifying a font in the DrawText call.)
        private static string _currentFont = "";

        // The current color to use when drawing text (Can be overridden by specifying a color in the
        // DrawText call.
        private static Color _currentColor = Color.White;

        /// <summary>
        /// When using the "Print" function, CursorLocation tracks the current drawing position for future
        /// calls to the Print command.
        /// </summary>
        public static Vector2 CursorLocation { get; set; }

        /// <summary>
        /// Whenever we draw text, we will use the CurrentFont if we haven't specified otherwise.
        /// </summary>
        static public string CurrentFont
        {
            get { return _currentFont; }
            set
            {
                if (_fonts.ContainsKey(value))
                    _currentFont = value;
            }
        }

        /// <summary>
        /// Whenever we draw text, we will use the CurrentColor if we haven't specified otherwise.
        /// </summary>
        static public Color CurrentColor
        {
            get { return _currentColor; }
            set { _currentColor = value; }
        }

        /// <summary>
        /// We need a SpriteBatch object to do any drawing.
        /// </summary>
        static public SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// We will use a ContentManager object to load our SpriteFonts.
        /// </summary>
        static public ContentManager ContentManager { get; set; }

        #endregion

        #region SpriteFont Loading Methods

        /// <summary>
        /// Load a SpriteFont given the fontName and the resourceName (Usually identical)
        /// </summary>
        static public void LoadFont(string fontName, string resourceName)
        {
            // Throw an exception if we don't have a content manager.
            if (ContentManager == null)
                throw new Exception("TextHandler ContentManager Not Initialized");

            _fonts.Add(fontName, ContentManager.Load<SpriteFont>(resourceName));
            if (CurrentFont == "")
                CurrentFont = fontName;
        }

        /// <summary>
        /// Load multiple fonts by passing a folder and a semi-colon delimited list of fonts to load.
        /// For example, if your content folder is "Content\Fonts" and you want to load Pericles10 and Pericles12,
        /// Pass @"Fonts\" as the FontsFolderPrevis and "Pericles10;Pericles12" as the FontList
        /// </summary>
        /// <param name="fontsFolderPrefix">Folder Prefix</param>
        /// <param name="fontList">Semicolon-delimited list of fonts</param>
        static public void LoadFonts(string fontsFolderPrefix, string fontList)
        {
            // Throw an exception if we don't have a content manager.
            if (ContentManager == null)
                throw new Exception("TextHandler ContentManager Not Initialized");

            var fontsToLoad = fontList.Split(';');
            foreach (var s in fontsToLoad)
                LoadFont(s.Trim(), fontsFolderPrefix + s.Trim());
        }

        /// <summary>
        /// Automatically loads all SpriteFonts from the passed folder (as a subfolder of the
        /// content manager root folder (ie, if your fonts are in "Content\Fonts" and your ContentManager's
        /// base folder is "Content", pass "Fonts" as the ContentFontsFolder.
        /// </summary>
        static public void AutoLoadFonts(string contentFontsFolder)
        {
            if (ContentManager == null)
                throw new Exception("TextHandler ContentManager Not Initialized");

            // Get all of the files in the passed folder (they will be .XNB files)
            // It will try to load any .XNB files in this folde as SpriteFonts, so that
            // should be the only type of content located there.
            var info = new DirectoryInfo(ContentManager.RootDirectory + @"\" + contentFontsFolder);
            foreach (var file in info.GetFiles("*.XNB"))
            {
                // Trim off the .XNB extension
                var fontName = file.Name.Substring(0, file.Name.Length - 4);

                // Load the font, giving it a dictionary name equal to the .spritefont asset name.
                LoadFont(fontName, contentFontsFolder + @"\" + fontName);
            }
        }
        #endregion

        #region Class Initialization

        /// <summary>
        /// Initializes the TextHandler class for usage.  Call this in your game's LoadContent.  If you pass
        /// a folder prefix (ie, @"Fonts\") the class will automatically load all spritefonts in that content folder.
        /// </summary>
        static public void Initialize(SpriteBatch spriteBatch, ContentManager contentManager, string fontsFolderPrefix)
        {
            SpriteBatch = spriteBatch;
            ContentManager = contentManager;
            
            if (fontsFolderPrefix != "")
                AutoLoadFonts(fontsFolderPrefix);
        }
        #endregion

        #region DrawText and it's Overloads
        
        /// <summary>
        /// Draw a text string.  This is our full method, with all parameters specifiable.  All of the other
        /// DrawText methods will be overloads of this function.
        /// </summary>
        static public void DrawText(string text, int x, int y, float angle, 
            string font, Color color, Vector2 origin, float scale, 
            SpriteEffects spriteEffects, float layerDepth)
        {
            if (SpriteBatch == null)
                throw new Exception("TextHandler SpriteBatch Not Initialized");

            // Make sure the font exists in our font dictionary.  Otherwise, default to the CurrentFont
            var fontToUse = font;
            if (!_fonts.ContainsKey(fontToUse))
                fontToUse = CurrentFont;

            // Use SpriteBatch to draw the text string.
            SpriteBatch.DrawString(_fonts[fontToUse],
                text,
                new Vector2(x, y),
                color,
                angle,
                Vector2.Zero,
                scale,
                spriteEffects,
                layerDepth);
        }

        static public void DrawText(string text, int x, int y)
        {
            DrawText(text, x, y, 0, CurrentFont, CurrentColor, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }
        static public void DrawText(string text, int x, int y, string font)
        {
            DrawText(text, x, y, 0, font, CurrentColor, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }
        static public void DrawText(string text, int x, int y, Color color)
        {
            DrawText(text, x, y, 0, CurrentFont, color, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }
        static public void DrawText(string text, int x, int y, string font, Color color)
        {
            DrawText(text, x, y, 0, font, color, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }
        #endregion

        #region "Print" function - Keeps a cursor location

        static public void Print(string text)
        {
            DrawText(text, (int)CursorLocation.X, (int)CursorLocation.Y);
            CursorLocation += new Vector2(0, _fonts[CurrentFont].MeasureString(text).Y);
        }
        #endregion

        #region Parsed Text

        /// <summary>
        /// Allows special codes to be embedded in a text string to allow for color and font changes.
        /// 
        /// ![#FFFFFF] - Specifies a color in hex (html-style) notation.  (ie, #FFFFFF=white, #000000=black)
        /// ![F:FontName] - Change to the specified font
        /// ![\n] - Created manually or automatically... specifies a new line
        /// </summary>
        static public void DrawTextParsed(string text, int x, int y, string font, int wrapLength)
        {
            // We will use the CursorLocaion to draw our string correctly, so we need to preserve the
            // location so we can restore it after we are done.
            var saveCursor = CursorLocation;

            // Preserve the current DefaultColor
            var saveColor = CurrentColor;

            // Preserve the current Font
            var saveFont = CurrentFont;

            // Set the cursor to our drawing location
            CursorLocation = new Vector2(x, y);

            string midText, postText;

            var workText = text;

            // If a Wrap Length has been passed, call our WrapText function and replace all of the
            // newline characters (\n) with ![\n] so they are "coded" with our standard code format.
            if (wrapLength > 0)
                workText = WrapText(workText, CurrentFont, wrapLength, true).Replace("\n", "![\n]");

            // Loop while the working text string contains a formatting code.
            while (workText.Contains("!["))
            {
                // PreText holds the portion of the string before the code.
                var preText = workText.Substring(0, workText.IndexOf("!["));

                midText = workText.Substring(workText.IndexOf("![") + 2);

                // PostText holds the portion of the string after the code.
                postText = midText.Substring(midText.IndexOf("]") + 1);

                // MidText holds the code itself
                midText = midText.Substring(0, (midText.Length - postText.Length) - 1);

                // Reset WorkText to contain everything after the code.
                workText = postText;

                // Draw PreText at the current location, in the current font and color.
                DrawText(preText, (int)CursorLocation.X, (int)CursorLocation.Y);

                // Move the cursor horizontally to the end of what we just drew.
                CursorLocation += new Vector2(_fonts[CurrentFont].MeasureString(preText).X, 0);

                // If we have a formatting code...
                if (midText.Length > 0)
                {
                    // The # sign signifies an HTML-like color code (ie, #FFFFFF = White)
                    if (midText.StartsWith("#"))
                    {
                        // Pull out the characters
                        byte r = byte.Parse(midText.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                        byte g = byte.Parse(midText.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                        byte b = byte.Parse(midText.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);

                        // Set current color to the desired color.
                        CurrentColor = new Color(r, g, b);
                    }
                    // A code beginning with "F:" designates a font change.
                    if (midText.StartsWith("F:"))
                    {
                        // Set the current font to the desired font.
                        CurrentFont = midText.Substring(2);
                    }
                    // The newline codes we wrapped earlier:
                    if (midText == "\n")
                    {
                        // Reset the horizontal position of the cursor, and bring the vertical position down
                        // by the size of the PreText line.
                        CursorLocation = new Vector2(x, CursorLocation.Y + _fonts[CurrentFont].MeasureString(preText).Y);
                    }
                }
            }

            // We have no codes left in WorkText, so simply draw whatever portion of it is left.
            DrawText(workText, (int)CursorLocation.X, (int)CursorLocation.Y);

            // Restore the saved positions
            CursorLocation = saveCursor;
            CurrentColor = saveColor;
            CurrentFont = saveFont;
        }
        #endregion

        #region Text Wrapping and Formatting

        /// <summary>
        /// Returns a string with all of our "special" formatting codes ( ![CODE] ) stripped out.
        /// We will use this when measureing text for wrapping purposes to make sure our codes don't
        /// impact our line lengths.
        /// </summary>
        static public string StripTextCodes(string text)
        {
            var resultText = "";
            var workText = text;
            
            while (workText.Contains("!["))
            {
                // pull apart the string, removing the ![, the ], and the code between them from the text
                var preText = workText.Substring(0, workText.IndexOf("!["));
                var midText = workText.Substring(workText.IndexOf("![") + 2);
                var postText = midText.Substring(midText.IndexOf("]") + 1);
                //todo: this line has an error, possibly:
                //todo: midText = midText.Substring(0, (midText.Length - postText.Length) - 1);
                workText = postText;
                
                // Append the actual text to our result
                resultText += preText;
            }
            resultText += workText;
            
            return resultText;
        }

        /// <summary>
        /// it returns a string that has newline characters (\n) inserted into it at the points where
        /// they would need to go to wrap a text string to a certain width.
        /// </summary>
        static public string WrapText(string text, string fontName, float maxLineWidth, bool stripCodes)
        {
            // Create an array (words) with one entry for each word in the passed text string
            var words = text.Split(' ');

            // A StringBuilder lets us add to a string and finally return the result
            var sb = new StringBuilder();

            // How long is the line we are currently working on so far
            var lineWidth = 0.0f;

            // Store a measurement of the size of a space in the font we are using.
            var spaceWidth = _fonts[fontName].MeasureString(" ").X;

            foreach (var word in words)
            {
                Vector2 size;
                
                if (stripCodes)
                    size = _fonts[fontName].MeasureString(StripTextCodes(word));
                else
                    size = _fonts[fontName].MeasureString(word);

                // If this word will fit on the current line, add it and keep track
                // of how long the line has gotten.
                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }
            
            return sb.ToString();
        }

        // Overloads for WrapText that supply defaults for some parameters
        static public string WrapText(string text, float maxLineWidth)
        {
            return WrapText(text, CurrentFont, maxLineWidth, false);
        }

        static public string WrapText(string text, string fontName, float maxLineWidth)
        {
            return WrapText(text, fontName, maxLineWidth, false);
        }

        #endregion
    }
}